# Blokora

Blokora is an original premium competitive block puzzle game for Android and iOS. Phase 1 focuses on an excellent offline solo endless loop: drag pieces onto a grid, clear rows/columns, chain combos, and chase a high score.

## Current build

The project is a Unity 6 + URP Android build with an offline Solo Endless loop. Unity `6000.0.81f1` is used locally with Android and iOS modules. The game can be played without an account or network connection.

Implemented in source:

- deterministic seeded piece generation
- 8×8 board model with valid/invalid placement
- row, column, and multi-line clear detection
- centralized scoring and combo rules
- accurate game-over detection
- drag/drop interaction contracts with valid preview states
- local solo modes, persistent player progression, Coins/Gems, and profile statistics
- original home, Solo, Shop, Profile, and bottom navigation UI
- Firebase/payment/ads interfaces with no credentials or external configuration
- Unity Test Framework tests for core rules

## Run

1. Install Unity `6000.0.81f1` with Android Build Support, iOS Build Support, and the Universal Render Pipeline.
2. Open `/Users/theohajana/Blokora` in Unity Hub.
3. Open `Assets/Blokora/Scenes/Blokora.unity`.
4. Press Play, or build the development APK with `Blokora.Editor.BlokoraBuild.BuildAndroidDevelopment`.

The current development APK is written to `Builds/Android/Blokora-development.apk`.

## Safety

Blokora is independent of Reaction Duel and EventPops. No Firebase config, AdMob IDs, signing keys, store credentials, or service accounts are committed. See `Documentation/ARCHITECTURE.md` and `Documentation/RELEASE.md`.
