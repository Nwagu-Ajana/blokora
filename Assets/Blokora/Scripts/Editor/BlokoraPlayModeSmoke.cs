using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Blokora.Editor
{
    public static class BlokoraPlayModeSmoke
    {
        private const string ScenePath = "Assets/Blokora/Scenes/Blokora.unity";
        private static double startedAt;

        public static void Run()
        {
            EditorSceneManager.OpenScene(ScenePath);
            EditorApplication.update -= Poll;
            EditorApplication.update += Poll;
            startedAt = EditorApplication.timeSinceStartup;
            EditorApplication.isPlaying = true;
        }

        private static void Poll()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.update -= Poll;
                return;
            }

            if (EditorApplication.timeSinceStartup - startedAt < 2.0) return;
            var app = Object.FindFirstObjectByType<Blokora.Presentation.BlokoraAppBootstrap>();
            var canvas = app == null ? null : app.GetComponentInChildren<Canvas>();
            var events = Object.FindFirstObjectByType<EventSystem>();
            var controller = app == null ? null : app.GetComponent<Blokora.Gameplay.BlokoraGameController>();
            var valid = app != null && canvas != null && events != null && controller != null && controller.Session != null && controller.Pieces.Count == 3;
            Debug.Log(valid ? "BLOKORA PLAY MODE SMOKE PASSED: bootstrap, canvas, event system, session, and three-piece tray are live." : "BLOKORA PLAY MODE SMOKE FAILED: required runtime objects are missing.");
            EditorApplication.isPlaying = false;
            EditorApplication.update -= Poll;
            if (Application.isBatchMode) EditorApplication.Exit(valid ? 0 : 1);
        }
    }
}
