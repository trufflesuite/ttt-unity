# Web3 Tic Tac Toe with Consensys!

This repo contains a Unity tic tac toe game with a betting and payout system powered by an Ethereum smart contract.

## Getting Started

1. Extract this repo where you normally keep your Unity projects.
2. Open the folder in Unity Hub. You may need to download another version of Unity, do so if prompted.
3. Run the game!

## Playing the Game

When you first start up the game, you'll be prompted to connect your MetaMask Mobile wallet via a QR code. Use your phone to scan the QR code. Once you approve the connection, the game will move to the main menu.

### Starting a New Game

Because this is meant to be a simple tutorial about connecting Unity games with smart contracts on the Ethereum blockchain, we have the setup of the initial game, including the supply of jackpot funds and payout addresses, come from the connected wallet. If you'd like to demo the payout features end-to-end, use your own address as one of the payout addresses.

### Winning a Game

There are 2 possible outcomes to end a tic tac toe game:

1. Victory: A player positions 3 of their marks in a row. In the case of a win, the wallet that setup the game will need to approve a request to end the game, which releaes the jackpot to the winner and declares the game over on chain.
2. Draw: All possible moves have been exhausted. In the case of a draw, the game will prompt for a rematch until a definitive winner emerges.

### Collecting Payouts

When you're on the main menu, the game periodically checks if you have any winnings awaiting withdrawal. If so, you'll see the winnings in the upper-right corner in the header, along with a "Collect" button. Click that button and approve the withdrawal to receive your winnings!
