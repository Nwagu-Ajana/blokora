using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Blokora.Editor
{
    public static class BlokoraTestRunner
    {
        public static void RunEditMode()
        {
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            api.RegisterCallbacks(new Callbacks());
            api.Execute(new ExecutionSettings(new Filter { testMode = TestMode.EditMode }) { runSynchronously = true });
        }

        public static void RunPlayMode()
        {
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Blokora/Scenes/Blokora.unity", true) };
            var api = ScriptableObject.CreateInstance<TestRunnerApi>();
            api.RegisterCallbacks(new Callbacks());
            api.Execute(new ExecutionSettings(new Filter { testMode = TestMode.PlayMode }));
        }

        private sealed class Callbacks : ICallbacks
        {
            public void RunStarted(ITestAdaptor testsToRun) { Debug.Log("BLOKORA TESTS STARTED"); }
            public void TestStarted(ITestAdaptor test) { }
            public void TestFinished(ITestResultAdaptor result) { if (result.TestStatus == TestStatus.Failed) Debug.LogError("FAILED " + result.Name + ": " + result.Message); }
            public void RunFinished(ITestResultAdaptor result)
            {
                Debug.Log("BLOKORA TESTS FINISHED: " + result.TestStatus);
                if (Application.isBatchMode) EditorApplication.Exit(result.TestStatus == TestStatus.Passed ? 0 : 1);
            }
        }
    }
}
