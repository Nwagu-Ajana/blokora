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
            new PieceDefinition("square", (0, 0), (1, 0), (0, 1), (1, 1)),
            new PieceDefinition("corner", (0, 0), (0, 1), (1, 0)),
            new PieceDefinition("l4", (0, 0), (0, 1), (0, 2), (1, 2)),
            new PieceDefinition("t4", (0, 0), (1, 0), (2, 0), (1, 1)),
        };
    }
}
