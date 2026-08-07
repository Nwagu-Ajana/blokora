using System;

namespace Blokora.Domain
{
    public sealed class PieceGenerator
    {
        private readonly Random random;
        public PieceGenerator(int seed) => random = new Random(seed);

        public PieceDefinition Next()
        {
            var template = PieceCatalog.All[random.Next(PieceCatalog.All.Count)];
            return new PieceDefinition(template.Id, template.Cells);
        }
    }
}
