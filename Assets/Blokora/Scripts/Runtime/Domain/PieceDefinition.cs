using System;
using System.Collections.Generic;

namespace Blokora.Domain
{
    [Serializable]
    public sealed class PieceDefinition
    {
        public readonly string Id;
        public readonly (int x, int y)[] Cells;
        public int CellCount => Cells.Length;
        public int Width { get { var max = 0; foreach (var cell in Cells) max = Math.Max(max, cell.x); return max + 1; } }
        public int Height { get { var max = 0; foreach (var cell in Cells) max = Math.Max(max, cell.y); return max + 1; } }

        public PieceDefinition(string id, params (int x, int y)[] cells) { Id = id; Cells = cells; }
    }

    public static class PieceCatalog
    {
        public static readonly IReadOnlyList<PieceDefinition> All = new[]
        {
            new PieceDefinition("single", (0, 0)),
            new PieceDefinition("line2", (0, 0), (1, 0)),
            new PieceDefinition("line3", (0, 0), (1, 0), (2, 0)),
            new PieceDefinition("line4", (0, 0), (1, 0), (2, 0), (3, 0)),
            new PieceDefinition("line5", (0, 0), (1, 0), (2, 0), (3, 0), (4, 0)),
            new PieceDefinition("line2v", (0, 0), (0, 1)),
            new PieceDefinition("line3v", (0, 0), (0, 1), (0, 2)),
            new PieceDefinition("line4v", (0, 0), (0, 1), (0, 2), (0, 3)),
            new PieceDefinition("line5v", (0, 0), (0, 1), (0, 2), (0, 3), (0, 4)),
            new PieceDefinition("square", (0, 0), (1, 0), (0, 1), (1, 1)),
            new PieceDefinition("rectangle", (0, 0), (1, 0), (2, 0), (0, 1), (1, 1), (2, 1)),
            new PieceDefinition("corner", (0, 0), (0, 1), (1, 0)),
            new PieceDefinition("l4", (0, 0), (0, 1), (0, 2), (1, 2)),
            new PieceDefinition("l4r", (1, 0), (1, 1), (1, 2), (0, 2)),
            new PieceDefinition("t4", (0, 0), (1, 0), (2, 0), (1, 1)),
            new PieceDefinition("t4v", (0, 1), (1, 0), (1, 1), (1, 2)),
            new PieceDefinition("stair", (0, 0), (1, 0), (1, 1), (2, 1)),
            new PieceDefinition("stairv", (1, 0), (0, 1), (1, 1), (0, 2)),
            new PieceDefinition("plus", (1, 0), (0, 1), (1, 1), (2, 1), (1, 2)),
        };
    }
}
