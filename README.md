# bip39

A minimal static site that generates a 12-word BIP39 mnemonic using the English wordlist.

Self-contained implementation built on the browser's native `crypto` and `SubtleCrypto` APIs.

This repo contains two components: the static web demo (`docs/`) and a .NET CLI (`cli/`) — see [cli/README.md](cli/README.md).

> ⚠️ Do not use with cryptocurrency wallets. Do not use the phrases it produces to secure real funds.

## Entropy

Random entropy is generated, then the leading bits of its SHA-256 hash are appended as a checksum. The combined bits are split into 11-bit groups, each mapping to one of 2048 wordlist words.

| Words | Entropy     | Checksum | Total bits |
|------:|------------:|---------:|-----------:|
| 12    | 128 bits    | 4 bits   | 132 bits   |
| 15    | 160 bits    | 5 bits   | 165 bits   |
| 18    | 192 bits    | 6 bits   | 198 bits   |
| 21    | 224 bits    | 7 bits   | 231 bits   |
| 24    | 256 bits    | 8 bits   | 264 bits   |

## Live demo

https://malvinly.github.io/bip39/

## Changing the word count

`generateMnemonic()` accepts an optional word count parameter. BIP39 supports 12, 15, 18, 21, or 24 words. Any other value throws an exception.

```js
generateMnemonic();     // defaults to 12
generateMnemonic(24);   // 24 words
```

Also update `grid-template-columns` in `index.html` if you want a different number of words per row.

## CLI

A .NET command-line tool. Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```sh
dotnet run --project cli         # 12 words
dotnet run --project cli -- 24   # 24 words
```

See [cli/README.md](cli/README.md) for options, examples, and build instructions.
