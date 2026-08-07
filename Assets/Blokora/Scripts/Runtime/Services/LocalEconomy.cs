using System;

namespace Blokora.Services
{
    public sealed class LocalEconomyService : IEconomyService
    {
        public int Coins { get; private set; } = 500;
        public int Gems { get; private set; } = 25;
        public void SpendCoins(int amount, Action<bool> onComplete) { var ok = amount >= 0 && Coins >= amount; if (ok) Coins -= amount; onComplete?.Invoke(ok); }
        public void GrantReward(string rewardId, Action<bool> onComplete) { if (rewardId == "starter_coins") Coins += 100; else if (rewardId == "starter_gems") Gems += 5; onComplete?.Invoke(true); }
    }
}
