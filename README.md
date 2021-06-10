# algorand-unity-integration

Example Unity app to integrate Algorand payments inside Unity in C#, using dotnet-algorand-sdk

- dotNET Algorand SDK - [github.com/RileyGe/dotnet-algorand-sdk](https://github.com/RileyGe/dotnet-algorand-sdk)

## Scenario

This app contains a simple store scenario, with the player creating (or loading) their own wallet, being able to buy items in a store, and a method to reward the player with ALGO for playing the app.

The app requires a wallet that acts as a central place for rewards to be sent from to the player, as well as store payments being sent to the wallet. 

### App Wallet & Config

[AppConfig.cs](algorand-unity-integration/Assets/Scripts/AppConfig.cs) need updating with the required information:

- [PureStake](https://developer.purestake.io/) API key for interacting with Algo API services
- APP_WALLET_MNEMONIC: Mnemonic phrase for the game's wallet. A central wallet that gives rewards to players and recieves payments from players
- APP_WALLET_ADDRESS: The address of the wallet used in the mnemonic.
