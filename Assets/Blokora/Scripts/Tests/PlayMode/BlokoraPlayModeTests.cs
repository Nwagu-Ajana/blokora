using System.Collections;
using Blokora.Gameplay;
using Blokora.Presentation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Blokora.Tests
{
    public sealed class BlokoraPlayModeTests
    {
        [UnityTest]
        public IEnumerator StartupCreatesPlayableSoloSession()
        {
            SceneManager.LoadScene("Blokora");
            yield return null;
            if (Object.FindFirstObjectByType<BlokoraAppBootstrap>() == null) new GameObject("BlokoraPlayModeBootstrap").AddComponent<BlokoraAppBootstrap>();
            yield return new WaitForSeconds(0.5f);
            var app = Object.FindFirstObjectByType<BlokoraAppBootstrap>();
            var controller = app == null ? null : app.GetComponent<BlokoraGameController>();
            Assert.That(app, Is.Not.Null);
            Assert.That(Object.FindFirstObjectByType<EventSystem>(), Is.Not.Null);
            Assert.That(controller, Is.Not.Null);
            Assert.That(controller.Session, Is.Not.Null);
            Assert.That(controller.Pieces, Has.Count.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator ValidRuntimePlacementUpdatesTheSession()
        {
            SceneManager.LoadScene("Blokora");
            yield return null;
            if (Object.FindFirstObjectByType<BlokoraAppBootstrap>() == null) new GameObject("BlokoraPlayModeBootstrap").AddComponent<BlokoraAppBootstrap>();
            yield return new WaitForSeconds(0.5f);
            var controller = Object.FindFirstObjectByType<BlokoraGameController>();
            var before = controller.Session.PiecesPlaced;
            Assert.That(controller.Place(0, 0, 0), Is.True);
            Assert.That(controller.Session.PiecesPlaced, Is.EqualTo(before + 1));
            Assert.That(controller.Pieces, Has.Count.EqualTo(3));
        }
    }
}
