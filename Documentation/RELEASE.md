# Release preparation

Required before store submission:

- Install Unity 6000.0.43f1 and validate Android/iOS builds.
- Create a new Firebase project named for Blokora; never reuse EventPops. Add `google-services.json` / `GoogleService-Info.plist` locally only.
- Create a new AdMob app and provide Android/iOS App IDs and ad unit IDs. Use test IDs during development.
- Configure Google Play `com.blokora.game` and iOS bundle ID `com.blokora.game`.
- Configure private Android keystore and Apple signing in secure CI/local settings; never commit signing material.
- Add Play Billing and StoreKit products only after the economy and server receipt-validation APIs are ready.
