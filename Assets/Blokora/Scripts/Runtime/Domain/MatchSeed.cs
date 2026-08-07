using System;

namespace Blokora.Domain
{
    public interface IMatchSeedProvider { int GetSeed(); }

    public sealed class LocalMatchSeedProvider : IMatchSeedProvider
    {
        private readonly int seed;
        public LocalMatchSeedProvider(int seed) => this.seed = seed;
        public int GetSeed() => seed;
    }
}
