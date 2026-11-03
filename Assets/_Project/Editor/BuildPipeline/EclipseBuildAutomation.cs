using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Eclipse.Build
{
    public static class EclipseBuildAutomation
    {
        private const string ApplicationId = "com.svkcreationz.eclipse";
        private const string ProductName = "ECLIPSE: THE LAST CIVILIZATION";
        private const string CompanyName = "SVK Creationz";
        private const string AppVersion = "1.0.0";
        private const int BuildVersionCode = 1;

        [MenuItem("ECLIPSE/Build/Build Android Development APK")]
        public static void BuildAndroidDevelopmentAPK()
        {
            Debug.Log("[ECLIPSE Build] Initializing Android Development Build...");

            // 1. Configure Android Player Settings
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, ApplicationId);
            PlayerSettings.productName = ProductName;
            PlayerSettings.companyName = CompanyName;
            PlayerSettings.bundleVersion = AppVersion;
            PlayerSettings.Android.bundleVersionCode = BuildVersionCode;

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersion.AndroidApiLevel26; // Android 8.0 Oreo
            PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersion)34; // Android 14
            PlayerSettings.Android.forceInternetPermission = true;

            // 2. Output Destination
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "Android");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string apkPath = Path.Combine(outputDirectory, "ECLIPSE-Android-Development.apk");

            // 3. Register Build Scenes
            string[] scenes = GetBuildScenes();

            // 4. Build Player Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = apkPath,
                target = BuildTarget.Android,
                options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.ConnectWithProfiler
            };

            Debug.Log($"[ECLIPSE Build] Executing BuildPlayer to destination: {apkPath}");
            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[ECLIPSE Build] SUCCESS: Android APK generated at '{apkPath}' ({summary.totalSize / (1024 * 1024)} MB).");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"[ECLIPSE Build] FAILED with {summary.totalErrors} errors.");
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }
            }
        }

        [MenuItem("ECLIPSE/Build/Export iOS Xcode Project")]
        public static void ExportiOSXcodeProject()
        {
            Debug.Log("[ECLIPSE Build] Initializing iOS Xcode Project Export...");

            // 1. Configure iOS Player Settings
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, ApplicationId);
            PlayerSettings.productName = ProductName;
            PlayerSettings.companyName = CompanyName;
            PlayerSettings.bundleVersion = AppVersion;
            PlayerSettings.iOS.buildNumber = BuildVersionCode.ToString();

            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetArchitecture(NamedBuildTarget.iOS, 1); // ARM64

            // 2. Output Destination
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "iOS", "ECLIPSE-iOS-Xcode-Project");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // 3. Build Player Options
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = GetBuildScenes(),
                locationPathName = outputDirectory,
                target = BuildTarget.iOS,
                options = BuildOptions.AcceptExternalModificationsToPlayer | BuildOptions.Development
            };

            Debug.Log($"[ECLIPSE Build] Exporting Xcode project to: {outputDirectory}");
            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[ECLIPSE Build] SUCCESS: iOS Xcode Project generated at '{outputDirectory}'.");
            }
            else
            {
                Debug.LogError($"[ECLIPSE Build] iOS Export FAILED with {summary.totalErrors} errors.");
                if (Application.isBatchMode)
                {
                    EditorApplication.Exit(1);
                }
            }
        }

        private static string[] GetBuildScenes()
        {
            // Scan project scenes
            string[] defaultScenes = new string[]
            {
                "Assets/_Project/Scenes/00_Bootstrap.unity",
                "Assets/_Project/Scenes/01_MainMenu.unity",
                "Assets/_Project/Scenes/03_Greyfall_Prototype.unity"
            };

            return defaultScenes;
        }
    }
}
