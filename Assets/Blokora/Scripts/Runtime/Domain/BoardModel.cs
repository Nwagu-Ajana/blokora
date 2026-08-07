using System;
using System.Collections.Generic;

namespace Blokora.Domain
{
    public readonly struct ClearResult
    {
        public readonly int Lines;
        public readonly int Rows;
        public readonly int Columns;
        public readonly int Cells;
        public ClearResult(int rows, int columns, int cells)
        {
            Rows = rows; Columns = columns; Lines = rows + columns; Cells = cells;
        }
    }

    public sealed class BoardModel
    {
        public int Width { get; }
        public int Height { get; }
        private readonly bool[,] occupied;

        public BoardModel(int width = 8, int height = 8) { Width = width; Height = height; occupied = new bool[width, height]; }
        public bool IsFilled(int x, int y) => IsInside(x, y) && occupied[x, y];
        public bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

        public bool CanPlace(PieceDefinition piece, int originX, int originY)
        {
            foreach (var cell in piece.Cells) if (!IsInside(originX + cell.x, originY + cell.y) || occupied[originX + cell.x, originY + cell.y]) return false;
            return true;
        }

        public ClearResult Place(PieceDefinition piece, int originX, int originY)
        {
            if (!CanPlace(piece, originX, originY)) throw new InvalidOperationException("Invalid piece placement");
            foreach (var cell in piece.Cells) occupied[originX + cell.x, originY + cell.y] = true;
            var fullRows = new List<int>(); var fullColumns = new List<int>();
            for (var y = 0; y < Height; y++) { var full = true; for (var x = 0; x < Width; x++) full &= occupied[x, y]; if (full) fullRows.Add(y); }
            for (var x = 0; x < Width; x++) { var full = true; for (var y = 0; y < Height; y++) full &= occupied[x, y]; if (full) fullColumns.Add(x); }
            var cleared = new HashSet<(int x, int y)>();
            foreach (var y in fullRows) for (var x = 0; x < Width; x++) cleared.Add((x, y));
            foreach (var x in fullColumns) for (var y = 0; y < Height; y++) cleared.Add((x, y));
            foreach (var cell in cleared) occupied[cell.x, cell.y] = false;
            return new ClearResult(fullRows.Count, fullColumns.Count, cleared.Count);
        }

        public bool HasAnyValidPlacement(IReadOnlyList<PieceDefinition> pieces)
        {
            foreach (var piece in pieces) for (var y = 0; y < Height; y++) for (var x = 0; x < Width; x++) if (CanPlace(piece, x, y)) return true;
            return false;
        }

        public void Reset() { Array.Clear(occupied, 0, occupied.Length); }
    }
}
