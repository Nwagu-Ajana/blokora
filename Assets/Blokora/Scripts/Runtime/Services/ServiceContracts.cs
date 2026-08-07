using System;
using System.Collections.Generic;

namespace Blokora.Services
{
    public interface IAuthenticationService { void SignInAnonymously(Action<string> onComplete); void LinkAccount(Action<bool> onComplete); }
    public interface IPlayerRepository { void Load(string playerId, Action<PlayerSnapshot> onComplete); void Save(PlayerSnapshot snapshot, Action<bool> onComplete); }
    public interface IMatchService { void FindRankedMatch(Action<MatchSnapshot> onComplete); void SubmitResult(MatchResult result, Action<bool> onComplete); }
    public interface ILeaderboardService { void Load(string scope, Action<IReadOnlyList<LeaderboardEntry>> onComplete); }
    public interface IEconomyService { void SpendCoins(int amount, Action<bool> onComplete); void GrantReward(string rewardId, Action<bool> onComplete); }
    public interface IInventoryService { bool Owns(string itemId); void Equip(string itemId, Action<bool> onComplete); }
    public interface IPurchaseService { void LoadProducts(Action<IReadOnlyList<StoreProduct>> onComplete); void Purchase(string productId, Action<PurchaseState> onComplete); void Restore(Action<IReadOnlyList<string>> onComplete); }
    public interface IFriendsService { void SendRequest(string playerId, Action<bool> onComplete); void LoadFriends(Action<IReadOnlyList<PlayerSnapshot>> onComplete); }
    public interface IAnalyticsService { void Track(string eventName, IReadOnlyDictionary<string, object> parameters = null); }
    public interface IAdService { void ShowRewarded(Action<bool> onComplete); }

    [Serializable] public sealed class PlayerSnapshot { public string Id; public string Name; public int Level; public int Coins; public int Gems; public int Rating; }
    [Serializable] public sealed class MatchSnapshot { public string MatchId; public int Seed; public string RulesVersion; }
    [Serializable] public sealed class MatchResult { public string MatchId; public int Score; public int DurationSeconds; }
    [Serializable] public sealed class LeaderboardEntry { public string PlayerId; public string Name; public int Rating; }
    [Serializable] public sealed class StoreProduct { public string Id; public string LocalizedPrice; }
    public enum PurchaseState { Purchased, Pending, Cancelled, Failed, AlreadyOwned, Unavailable }
}
