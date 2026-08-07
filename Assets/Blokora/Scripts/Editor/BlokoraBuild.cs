using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Blokora.Editor
{
    public static class BlokoraBuild
    {
        private const string GameplayScene = "Assets/Blokora/Scenes/Blokora.unity";
        private const string AndroidOutput = "Builds/Android/Blokora-development.apk";

        public static void BuildAndroidDevelopment()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(AndroidOutput)!);

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
            PlayerSettings.SetArchitecture(NamedBuildTarget.Android, unchecked((int)AndroidArchitecture.All));
            UnityEngine.Debug.Log($"Android architecture configured: legacy={PlayerSettings.Android.targetArchitectures}, named={PlayerSettings.GetArchitecture(NamedBuildTarget.Android)}, group={PlayerSettings.GetArchitecture(BuildTargetGroup.Android)}");
            EditorUserBuildSettings.development = true;
            EditorUserBuildSettings.connectProfiler = false;

            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { GameplayScene },
                locationPathName = AndroidOutput,
                target = BuildTarget.Android,
                options = BuildOptions.Development | BuildOptions.AllowDebugging
            });

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException($"Blokora Android build failed: {report.summary.result}");
            }

            UnityEngine.Debug.Log($"Blokora Android build succeeded: {report.summary.outputPath} ({report.summary.totalSize} bytes)");
        }
    }
}
