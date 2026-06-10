# BIP39 CLI

A .NET command-line tool that generates BIP39 mnemonic phrases.

> ⚠️ Do not use with cryptocurrency wallets. Do not use the phrases it produces to secure real funds.

## Usage

```
Bip39Cli [count] [options]
```

## Options

| Option | Description |
|--------|-------------|
| `count` | Number of words (12, 15, 18, 21, or 24). Default: 12 |
| `--no-colors` | Disable colored output |
| `--help` | Show help and usage information |
| `--version` | Show version information |

## Examples

```sh
# Generate a 12-word mnemonic
Bip39Cli

# Generate a 24-word mnemonic
Bip39Cli 24

# Generate without colors
Bip39Cli --no-colors
```

## Build

```sh
dotnet build --project cli
```
