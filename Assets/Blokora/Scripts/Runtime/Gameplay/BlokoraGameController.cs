using System.Collections.Generic;
using Blokora.Domain;
using UnityEngine;

namespace Blokora.Gameplay
{
    public sealed class BlokoraGameController : MonoBehaviour
    {
        [SerializeField] private int developmentSeed = 42817;
        public SoloGameSession Session { get; private set; }
        public IReadOnlyList<PieceDefinition> Pieces => Session?.Tray;
        public void StartEndless() { Session = new SoloGameSession(developmentSeed, SoloMode.Endless); }
        public bool Place(int trayIndex, int x, int y) { return Session != null && Session.TryPlace(trayIndex, x, y, out _); }
        public void Restart() { if (Session == null) StartEndless(); else Session.Restart(developmentSeed); }
    }
}
