using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blokora.Services
{
    [Serializable]
    public sealed class LocalPlayerProgress
    {
        public string UserName = "Player";
        public int Level = 1;
        public int Xp;
        public int Coins = 500;
        public int Gems = 25;
        public int Trophies = 0;
        public int HighScore;
        public int GamesPlayed;
        public int GamesWon;
        public int LinesCleared;
        public int BestCombo;
        public string EquippedSkin = "classic";
        public List<string> OwnedCosmetics = new List<string> { "classic" };
        public string LastDailyRewardUtcDay = string.Empty;

        public float WinRate => GamesPlayed == 0 ? 0f : (float)GamesWon / GamesPlayed;

        public void RecordSoloRun(int score, int lines, int combo)
        {
            GamesPlayed++;
            LinesCleared += lines;
            BestCombo = Math.Max(BestCombo, combo);
            HighScore = Math.Max(HighScore, score);
            var reward = Math.Max(10, score / 100);
            Coins += reward;
            Xp += Math.Max(5, score / 20);
            while (Xp >= Level * 100) { Xp -= Level * 100; Level++; }
            Save(this);
        }

        public bool Owns(string cosmeticId) { return OwnedCosmetics != null && OwnedCosmetics.Contains(cosmeticId); }
        public bool TryPurchaseAndEquip(string cosmeticId, int cost)
        {
            if (Owns(cosmeticId)) { EquippedSkin = cosmeticId; Save(this); return true; }
            if (cost < 0 || Coins < cost) return false;
            Coins -= cost; OwnedCosmetics.Add(cosmeticId); EquippedSkin = cosmeticId; Save(this); return true;
        }

        public bool TryClaimDailyReward(out int coins)
        {
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            if (LastDailyRewardUtcDay == today) { coins = 0; return false; }
            coins = 100; Coins += coins; LastDailyRewardUtcDay = today; Save(this); return true;
        }

        public static LocalPlayerProgress Load()
        {
            var json = PlayerPrefs.GetString("blokora.player.progress", string.Empty);
            if (string.IsNullOrEmpty(json)) return new LocalPlayerProgress();
            try
            {
                var progress = JsonUtility.FromJson<LocalPlayerProgress>(json) ?? new LocalPlayerProgress();
                if (progress.OwnedCosmetics == null || progress.OwnedCosmetics.Count == 0) progress.OwnedCosmetics = new List<string> { "classic" };
                return progress;
            }
            catch { return new LocalPlayerProgress(); }
        }

        public static void Save(LocalPlayerProgress progress)
        {
            PlayerPrefs.SetString("blokora.player.progress", JsonUtility.ToJson(progress));
            PlayerPrefs.Save();
        }
    }

    public sealed class LocalSettings
    {
        public bool Music { get; private set; } = true;
        public bool SoundEffects { get; private set; } = true;
        public bool Haptics { get; private set; } = true;

        public LocalSettings()
        {
            Music = PlayerPrefs.GetInt("blokora.settings.music", 1) == 1;
            SoundEffects = PlayerPrefs.GetInt("blokora.settings.sfx", 1) == 1;
            Haptics = PlayerPrefs.GetInt("blokora.settings.haptics", 1) == 1;
        }

        public void SetMusic(bool value) { Music = value; PlayerPrefs.SetInt("blokora.settings.music", value ? 1 : 0); PlayerPrefs.Save(); }
        public void SetSoundEffects(bool value) { SoundEffects = value; PlayerPrefs.SetInt("blokora.settings.sfx", value ? 1 : 0); PlayerPrefs.Save(); }
        public void SetHaptics(bool value) { Haptics = value; PlayerPrefs.SetInt("blokora.settings.haptics", value ? 1 : 0); PlayerPrefs.Save(); }
    }
}
