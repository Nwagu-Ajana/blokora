using System;
using System.Collections.Generic;
using Blokora.Domain;

namespace Blokora.Gameplay
{
    public enum SoloMode { Endless, Practice, DailyChallenge }

    public sealed class SoloGameSession
    {
        public BoardModel Board { get; private set; }
        public List<PieceDefinition> Tray { get; } = new List<PieceDefinition>(3);
        public SoloMode Mode { get; }
        public int Score { get; private set; }
        public int Combo { get; private set; }
        public int PiecesPlaced { get; private set; }
        public int LinesCleared { get; private set; }
        public int RowsCleared { get; private set; }
        public int ColumnsCleared { get; private set; }
        public int BestCombo { get; private set; }
        public bool IsGameOver { get; private set; }
        private PieceGenerator generator;

        public SoloGameSession(int seed, SoloMode mode = SoloMode.Endless, int boardSize = 8)
        {
            Mode = mode; Board = new BoardModel(boardSize, boardSize); generator = new PieceGenerator(seed); RefillTray();
        }

        public bool TryPlace(int trayIndex, int x, int y, out ClearResult clear)
        {
            clear = default;
            if (IsGameOver || trayIndex < 0 || trayIndex >= Tray.Count) return false;
            var piece = Tray[trayIndex]; if (!Board.CanPlace(piece, x, y)) return false;
            clear = Board.Place(piece, x, y); PiecesPlaced++; LinesCleared += clear.Lines; RowsCleared += clear.Rows; ColumnsCleared += clear.Columns;
            Combo = clear.Lines > 0 ? Combo + 1 : 0; BestCombo = Math.Max(BestCombo, Combo); Score += ScoreRules.Placement(piece.CellCount) + ScoreRules.Lines(clear.Lines, Combo);
            Tray.RemoveAt(trayIndex); Tray.Add(generator.Next());
            IsGameOver = !Board.HasAnyValidPlacement(Tray); return true;
        }

        public void Restart(int seed) { Board.Reset(); generator = new PieceGenerator(seed); Tray.Clear(); Score = 0; Combo = 0; BestCombo = 0; PiecesPlaced = 0; LinesCleared = 0; RowsCleared = 0; ColumnsCleared = 0; IsGameOver = false; RefillTray(); }
        private void RefillTray() { while (Tray.Count < 3) Tray.Add(generator.Next()); }
    }
}
