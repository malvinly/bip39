# bip39

A minimal static site that generates a 12-word BIP39 mnemonic using the English wordlist.

Self-contained implementation built on the browser's native `crypto` and `SubtleCrypto` APIs.

> ⚠️ Do not use with cryptocurrency wallets. Do not use the phrases it produces to secure real funds.

## Changing the word count

`generateMnemonic()` accepts an optional word count parameter. BIP39 supports 12, 15, 18, 21, or 24 words. Any other value throws an exception.

```js
generateMnemonic();     // defaults to 12
generateMnemonic(24);   // 24 words
```

Also update `grid-template-columns` in `src/index.html` if you want a different number of words per row.
