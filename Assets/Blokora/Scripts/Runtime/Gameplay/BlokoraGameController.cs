using System.Collections.Generic;
using Blokora.Domain;
using Blokora.Services;
using UnityEngine;

namespace Blokora.Gameplay
{
    public sealed class BlokoraGameController : MonoBehaviour
    {
        [SerializeField] private int developmentSeed = 42817;
        public SoloGameSession Session { get; private set; }
        public LocalPlayerProgress Progress { get; private set; }
        public LocalSettings Settings { get; private set; }
        public IReadOnlyList<PieceDefinition> Pieces => Session?.Tray;
        public void StartEndless() { Progress = LocalPlayerProgress.Load(); Settings = new LocalSettings(); Session = new SoloGameSession(developmentSeed, SoloMode.Endless); }
        public bool Place(int trayIndex, int x, int y)
        {
            if (Session == null || !Session.TryPlace(trayIndex, x, y, out _)) return false;
            if (Session.IsGameOver) Progress.RecordSoloRun(Session.Score, Session.LinesCleared, Session.BestCombo);
            return true;
        }
        public void Restart() { if (Session == null) StartEndless(); else Session.Restart(developmentSeed); }
    }
}
