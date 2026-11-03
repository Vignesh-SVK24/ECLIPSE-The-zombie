using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Eclipse.Environment;
using Eclipse.Player;

namespace Eclipse.Build
{
    /// <summary>
    /// Editor automation tool that constructs, configures, and serializes
    /// the photorealistic Greyfall City (Batch B) scene into Assets/_Project/Scenes/03_Greyfall_Prototype.unity.
    /// Can be executed from the Unity Editor menu or headlessly via batchmode CLI.
    /// </summary>
    public static class GreyfallCitySceneGenerator
    {
        private const string SceneFolder = "Assets/_Project/Scenes";
        private const string ScenePath = "Assets/_Project/Scenes/03_Greyfall_Prototype.unity";

        [MenuItem("ECLIPSE/Environment/Generate Photorealistic Greyfall City (Batch B)", false, 20)]
        public static void GenerateAndSaveScene()
        {
            Debug.Log("[GreyfallCitySceneGenerator] Initializing Greyfall City (Batch B) generation pipeline...");

            // 1. Ensure target directory exists
            if (!Directory.Exists(SceneFolder))
            {
                Directory.CreateDirectory(SceneFolder);
                AssetDatabase.Refresh();
            }

            // 2. Create new empty scene
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // 3. Setup Directional Sun & URP Sky
            GameObject sunGo = new GameObject("Directional Light");
            Light sunLight = sunGo.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.color = new Color(0.88f, 0.90f, 0.92f); // Overcast cool daylight
            sunLight.intensity = 1.15f;
            sunLight.shadows = LightShadows.Soft;
            sunGo.transform.rotation = Quaternion.Euler(46f, -38f, 0);

            // 4. Setup Post-Processing Global Volume
            GameObject volumeGo = new GameObject("Global PostProcess Volume");
            Volume volume = volumeGo.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 1f;

            // 5. Setup Master Environment Builder
            GameObject masterGo = new GameObject("GreyfallCity_MasterController");
            var builder = masterGo.AddComponent<GreyfallCityEnvironmentBuilder>();
            builder.ConstructCompleteGreyfallCity();

            // 6. Setup Ethan Cross Player Instance
            GameObject playerGo = PlayerPrefabBuilder.BuildEthanCrossPlayerObject("EthanCross_Player");
            playerGo.transform.position = new Vector3(0, 0.2f, -95f);
            playerGo.transform.rotation = Quaternion.identity;

            // 7. Setup Main Cinemachine / Gameplay Camera
            GameObject camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            Camera cam = camGo.AddComponent<Camera>();
            cam.fieldOfView = 65f;
            cam.nearClipPlane = 0.2f;
            cam.farClipPlane = 400f;
            camGo.AddComponent<AudioListener>();

            // URP Additional Camera Data
            var camData = camGo.AddComponent<UniversalAdditionalCameraData>();
            camData.renderPostProcessing = true;
            camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;

            // Player Camera Controller
            var camController = camGo.AddComponent<PlayerCameraController>();
            camController.SetTarget(playerGo.transform);

            // Position camera behind player
            camGo.transform.position = new Vector3(0, 2.4f, -98.5f);
            camGo.transform.rotation = Quaternion.Euler(14f, 0, 0);

            // 8. Save Scene to disk
            bool saveSuccess = EditorSceneManager.SaveScene(scene, ScenePath);
            if (saveSuccess)
            {
                Debug.Log($"<color=#4CAF50><b>[GreyfallCitySceneGenerator] Successfully saved scene to: {ScenePath}</b></color>");
                AddSceneToBuildSettings(ScenePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError($"[GreyfallCitySceneGenerator] FAILED to save scene to {ScenePath}");
            }
        }

        private static void AddSceneToBuildSettings(string path)
        {
            var currentScenes = EditorBuildSettings.scenes;
            foreach (var s in currentScenes)
            {
                if (s.path == path) return;
            }

            var newScenes = new EditorBuildSettingsScene[currentScenes.Length + 1];
            for (int i = 0; i < currentScenes.Length; i++)
            {
                newScenes[i] = currentScenes[i];
            }
            newScenes[newScenes.Length - 1] = new EditorBuildSettingsScene(path, true);
            EditorBuildSettings.scenes = newScenes;
            Debug.Log($"[GreyfallCitySceneGenerator] Added {path} to EditorBuildSettings.asset");
        }
    }
}

