# BIP39 CLI

A .NET command-line tool that generates BIP39 mnemonic phrases.

> ⚠️ Do not use with cryptocurrency wallets. Do not use the phrases it produces to secure real funds.

## Usage

```
Bip39Cli [options]
```

## Options

| Option | Description |
|--------|-------------|
| `--count`, `-n` | Number of words (12, 15, 18, 21, or 24). Default: 12 |
| `--separator`, `-s` | Separator between words. Default: space |
| `--no-colors` | Disable colored output |
| `--help` | Show help and usage information |

The exit code is `0` on success and `1` on invalid arguments.

## Examples

Run from source with `dotnet run` (arguments after `--` are passed to the tool):

```sh
# Generate a 12-word mnemonic
dotnet run --project cli

# Generate a 24-word mnemonic
dotnet run --project cli -- -n 24

# Generate without colors
dotnet run --project cli -- --no-colors
```

Once published or installed on your `PATH`, invoke it directly as `Bip39Cli [count] [options]`.

## Build

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
dotnet build cli
```
