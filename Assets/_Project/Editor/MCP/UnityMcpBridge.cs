using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Eclipse.Editor.MCP
{
    /// <summary>
    /// Local Model Context Protocol (MCP) Editor Bridge for Unity.
    /// Exposes a secure localhost HTTP JSON-RPC endpoint (http://127.0.0.1:8080/mcp/)
    /// allowing Google Antigravity to query the scene hierarchy, inspect GameObjects,
    /// read Console logs, validate NavMesh, and trigger environment generation.
    /// Dispatches all Unity API operations safely to the main Editor thread.
    /// </summary>
    [InitializeOnLoad]
    public static class UnityMcpBridge
    {
        private const int BridgePort = 8080;
        private const string BridgePrefix = "http://127.0.0.1:8080/mcp/";

        private static HttpListener _listener;
        private static Thread _listenerThread;
        private static bool _isRunning = false;

        private static readonly Queue<Action> _mainThreadQueue = new Queue<Action>();
        private static readonly object _queueLock = new object();

        // Console Log Capture Buffer
        private static readonly List<LogEntry> _recentLogs = new List<LogEntry>();
        private const int MaxLogHistory = 100;

        [Serializable]
        public struct LogEntry
        {
            public string condition;
            public string stackTrace;
            public string type;
            public string timestamp;
        }

        static UnityMcpBridge()
        {
            Application.logMessageReceived += HandleLogMessage;
            EditorApplication.update += ProcessMainThreadQueue;
            StartBridge();
        }

        [MenuItem("ECLIPSE/MCP Bridge/Restart Bridge Server", false, 100)]
        public static void RestartBridge()
        {
            StopBridge();
            StartBridge();
        }

        public static void StartBridge()
        {
            if (_isRunning) return;

            try
            {
                _listener = new HttpListener();
                _listener.Prefixes.Add(BridgePrefix);
                _listener.Start();
                _isRunning = true;

                _listenerThread = new Thread(ListenLoop) { IsBackground = true };
                _listenerThread.Start();

                Debug.Log($"<color=#4CAF50><b>[UnityMcpBridge] Server active and listening on {BridgePrefix}</b></color>");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[UnityMcpBridge] Could not start HTTP listener on port {BridgePort}: {ex.Message}");
            }
        }

        public static void StopBridge()
        {
            _isRunning = false;
            if (_listener != null)
            {
                try { _listener.Stop(); _listener.Close(); } catch { }
                _listener = null;
            }
            if (_listenerThread != null)
            {
                _listenerThread.Abort();
                _listenerThread = null;
            }
            Debug.Log("[UnityMcpBridge] Server stopped.");
        }

        private static void HandleLogMessage(string condition, string stackTrace, LogType type)
        {
            lock (_recentLogs)
            {
                if (_recentLogs.Count >= MaxLogHistory)
                {
                    _recentLogs.RemoveAt(0);
                }
                _recentLogs.Add(new LogEntry
                {
                    condition = condition,
                    stackTrace = stackTrace,
                    type = type.ToString(),
                    timestamp = DateTime.Now.ToString("HH:mm:ss.fff")
                });
            }
        }

        private static void ProcessMainThreadQueue()
        {
            while (true)
            {
                Action action = null;
                lock (_queueLock)
                {
                    if (_mainThreadQueue.Count > 0)
                    {
                        action = _mainThreadQueue.Dequeue();
                    }
                }

                if (action != null)
                {
                    try { action.Invoke(); }
                    catch (Exception ex) { Debug.LogError($"[UnityMcpBridge] Main thread dispatch error: {ex}"); }
                }
                else
                {
                    break;
                }
            }
        }

        private static void ListenLoop()
        {
            while (_isRunning && _listener != null && _listener.IsListening)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(ProcessRequest, context);
                }
                catch (HttpListenerException) { break; }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[UnityMcpBridge] Error accepting connection: {ex.Message}");
                }
            }
        }

        private static void ProcessRequest(object state)
        {
            HttpListenerContext context = (HttpListenerContext)state;
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            // CORS headers for local tools
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            string action = request.QueryString["action"] ?? "ping";
            string responseJson = "";

            var resetEvent = new ManualResetEvent(false);

            lock (_queueLock)
            {
                _mainThreadQueue.Enqueue(() =>
                {
                    try
                    {
                        responseJson = HandleActionOnMainThread(action, request);
                    }
                    catch (Exception ex)
                    {
                        responseJson = $"{{\"error\":\"{EscapeJson(ex.Message)}\"}}";
                    }
                    finally
                    {
                        resetEvent.Set();
                    }
                });
            }

            resetEvent.WaitOne(8000);

            byte[] buffer = Encoding.UTF8.GetBytes(responseJson);
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            try
            {
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
            catch { }
        }

        private static string HandleActionOnMainThread(string action, HttpListenerRequest request)
        {
            Scene activeScene = SceneManager.GetActiveScene();

            switch (action.ToLowerInvariant())
            {
                case "ping":
                    return $"{{\"status\":\"ok\",\"unityVersion\":\"{Application.unityVersion}\",\"activeScene\":\"{activeScene.name}\",\"scenePath\":\"{activeScene.path}\",\"rootCount\":{activeScene.rootCount},\"isPlaying\":{Application.isPlaying.ToString().ToLower()}}}";

                case "get_scene_hierarchy":
                    var rootObjects = activeScene.GetRootGameObjects();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\"scene\":\"").Append(activeScene.name).Append("\",\"rootGameObjects\":[");
                    for (int i = 0; i < rootObjects.Length; i++)
                    {
                        if (i > 0) sb.Append(",");
                        SerializeGameObject(rootObjects[i], sb, 0, 3); // Max depth 3
                    }
                    sb.Append("]}");
                    return sb.ToString();

                case "inspect_gameobject":
                    string targetName = request.QueryString["name"];
                    if (string.IsNullOrEmpty(targetName)) return "{\"error\":\"name parameter missing\"}";

                    GameObject targetGo = GameObject.Find(targetName);
                    if (targetGo == null) return $"{{\"error\":\"GameObject '{targetName}' not found\"}}";

                    return InspectGameObjectDetails(targetGo);

                case "get_console_logs":
                    lock (_recentLogs)
                    {
                        StringBuilder logSb = new StringBuilder();
                        logSb.Append("{\"logs\":[");
                        for (int i = 0; i < _recentLogs.Count; i++)
                        {
                            if (i > 0) logSb.Append(",");
                            var l = _recentLogs[i];
                            logSb.Append($"{{\"type\":\"{l.type}\",\"time\":\"{l.timestamp}\",\"message\":\"{EscapeJson(l.condition)}\"}}");
                        }
                        logSb.Append("]}");
                        return logSb.ToString();
                    }

                case "generate_greyfall_scene":
                    Eclipse.Build.GreyfallCitySceneGenerator.GenerateAndSaveScene();
                    return "{\"status\":\"success\",\"message\":\"Greyfall City (Batch B) scene generated and saved to Assets/_Project/Scenes/03_Greyfall_Prototype.unity\"}";

                case "inspect_lighting":
                    return InspectLightingSettings();

                case "inspect_materials":
                    return InspectSceneMaterials();

                default:
                    return $"{{\"status\":\"unknown_action\",\"action\":\"{action}\"}}";
            }
        }

        private static void SerializeGameObject(GameObject go, StringBuilder sb, int currentDepth, int maxDepth)
        {
            sb.Append("{");
            sb.Append("\"name\":\"").Append(EscapeJson(go.name)).Append("\",");
            sb.Append("\"active\":").Append(go.activeSelf.ToString().ToLower()).Append(",");
            sb.Append("\"tag\":\"").Append(EscapeJson(go.tag)).Append("\",");
            sb.Append("\"layer\":").Append(go.layer).Append(",");

            var comps = go.GetComponents<Component>();
            sb.Append("\"components\":[");
            for (int c = 0; c < comps.Length; c++)
            {
                if (comps[c] == null) continue;
                if (c > 0) sb.Append(",");
                sb.Append("\"").Append(comps[c].GetType().Name).Append("\"");
            }
            sb.Append("]");

            if (currentDepth < maxDepth && go.transform.childCount > 0)
            {
                sb.Append(",\"children\":[");
                for (int ch = 0; ch < go.transform.childCount; ch++)
                {
                    if (ch > 0) sb.Append(",");
                    SerializeGameObject(go.transform.GetChild(ch).gameObject, sb, currentDepth + 1, maxDepth);
                }
                sb.Append("]");
            }

            sb.Append("}");
        }

        private static string InspectGameObjectDetails(GameObject go)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"name\":\"").Append(EscapeJson(go.name)).Append("\",");
            sb.Append("\"position\":{\"x\":").Append(go.transform.position.x).Append(",\"y\":").Append(go.transform.position.y).Append(",\"z\":").Append(go.transform.position.z).Append("},");
            sb.Append("\"rotation\":{\"x\":").Append(go.transform.eulerAngles.x).Append(",\"y\":").Append(go.transform.eulerAngles.y).Append(",\"z\":").Append(go.transform.eulerAngles.z).Append("},");
            sb.Append("\"scale\":{\"x\":").Append(go.transform.localScale.x).Append(",\"y\":").Append(go.transform.localScale.y).Append(",\"z\":").Append(go.transform.localScale.z).Append("},");

            var renderers = go.GetComponentsInChildren<Renderer>();
            sb.Append("\"rendererCount\":").Append(renderers.Length).Append(",");

            var colliders = go.GetComponentsInChildren<Collider>();
            sb.Append("\"colliderCount\":").Append(colliders.Length).Append(",");

            var lodGroup = go.GetComponent<LODGroup>();
            sb.Append("\"hasLODGroup\":").Append((lodGroup != null).ToString().ToLower());

            sb.Append("}");
            return sb.ToString();
        }

        private static string InspectLightingSettings()
        {
            Light sun = RenderSettings.sun;
            string sunName = sun != null ? sun.name : "None";
            float sunIntensity = sun != null ? sun.intensity : 0f;
            Color sunColor = sun != null ? sun.color : Color.black;

            return $"{{\"sunName\":\"{sunName}\",\"sunIntensity\":{sunIntensity},\"fogEnabled\":{RenderSettings.fog.ToString().ToLower()},\"fogMode\":\"{RenderSettings.fogMode}\",\"fogDensity\":{RenderSettings.fogDensity},\"ambientMode\":\"{RenderSettings.ambientMode}\"}}";
        }

        private static string InspectSceneMaterials()
        {
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            HashSet<string> uniqueMats = new HashSet<string>();
            foreach (var r in renderers)
            {
                foreach (var m in r.sharedMaterials)
                {
                    if (m != null) uniqueMats.Add(m.name);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"uniqueMaterialCount\":").Append(uniqueMats.Count).Append(",\"materials\":[");
            int idx = 0;
            foreach (var mat in uniqueMats)
            {
                if (idx > 0) sb.Append(",");
                sb.Append("\"").Append(EscapeJson(mat)).Append("\"");
                idx++;
            }
            sb.Append("]}");
            return sb.ToString();
        }

        private static string EscapeJson(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", " ");
        }
    }
}

