# Blokora architecture

`Domain` contains pure puzzle rules: `BoardModel`, `PieceDefinition`, `PieceGenerator`, `ScoreRules`, and `MatchSeed`. It has no Unity scene or UI dependencies and is deterministic from a seed.

`Gameplay` owns the solo session state and placement orchestration. `Presentation` owns drag visuals, board rendering, and screen composition. `Services` contains interfaces for Authentication, player data, matches, inventory, economy, purchases, friends, analytics, and ads. Firebase, Play Billing, and StoreKit adapters will implement those interfaces later.

Ranked multiplayer must use a server-controlled seed, rules version, validated placements, and server-authoritative score/economy results. Paid cosmetics and Gems must never affect board generation, scoring, placement rules, timer, or matchmaking.

Season passes, tournaments, events, Firebase, and live multiplayer are intentionally architecture-only in this phase. The UI must use Coming Soon/disabled states until their backend contracts exist.
