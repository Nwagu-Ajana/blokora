using System;

namespace Blokora.Domain
{
    public sealed class PieceGenerator
    {
        private readonly Random random;
        private readonly int boardSize;
        public PieceGenerator(int seed, int boardSize = 8) { random = new Random(seed); this.boardSize = boardSize; }

        public PieceDefinition Next()
        {
            var candidates = new System.Collections.Generic.List<PieceDefinition>();
            foreach (var piece in PieceCatalog.All) if (piece.Width <= boardSize && piece.Height <= boardSize) candidates.Add(piece);
            var template = candidates[random.Next(candidates.Count)];
            return new PieceDefinition(template.Id, template.Cells);
        }
    }
}
