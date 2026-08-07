# Blokora external blockers

These items are intentionally represented by local interfaces/placeholders so offline development can continue safely.

## Blokora Firebase project

Before enabling cloud authentication, Firestore, Remote Config, Analytics, Crashlytics, or FCM, create a brand-new Firebase project for Blokora and provide only its Android/iOS configuration files. Do not use EventPops or Reaction Duel Firebase configuration.

Required later:

- Blokora Android `google-services.json`
- Blokora iOS `GoogleService-Info.plist`
- documented Blokora Firebase project ID
- Firestore rules review and server-authoritative Cloud Functions deployment

## Store and ads

- Google Play Console application and product IDs are required for production billing.
- Apple Developer/App Store Connect product IDs are required for iOS billing.
- Blokora AdMob application/ad unit IDs are required; development uses no production identifiers.
- Play App Signing and Apple signing/provisioning must be supplied by the owner.

## Multiplayer

Ranked matchmaking, server-created deterministic seeds, result validation, trophies, friends, leaderboards, and tournament rewards require a Blokora backend. The client contracts and telemetry model are kept separate so local Solo does not trust or simulate ranked authority.
