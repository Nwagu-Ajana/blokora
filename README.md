# Blokora

Blokora is an original premium competitive block puzzle game for Android and iOS. Phase 1 focuses on an excellent offline solo endless loop: drag pieces onto a grid, clear rows/columns, chain combos, and chase a high score.

## Current phase

This repository is a clean Unity 6 + URP project foundation. Unity is not installed in the current development environment, so the project has not yet been opened or built by the Unity editor. Install Unity `6000.0.43f1` with Android/iOS modules, open this folder, and let Package Manager resolve the pinned packages.

Implemented in source:

- deterministic seeded piece generation
- 8×8 board model with valid/invalid placement
- row, column, and multi-line clear detection
- centralized scoring and combo rules
- accurate game-over detection
- drag/drop interaction contracts with valid preview states
- local solo modes and player economy model
- Firebase/payment/ads interfaces with no credentials or external configuration
- Unity Test Framework tests for core rules

## Run

1. Install Unity 6000.0.43f1 with Android Build Support, iOS Build Support, and the Universal Render Pipeline.
2. Open `/Users/theohajana/Blokora` in Unity Hub.
3. Open `Assets/Blokora/Scenes/Blokora.unity`.
4. Press Play, or switch the active build target to Android/iOS.

## Safety

Blokora is independent of Reaction Duel and EventPops. No Firebase config, AdMob IDs, signing keys, store credentials, or service accounts are committed. See `Documentation/ARCHITECTURE.md` and `Documentation/RELEASE.md`.
