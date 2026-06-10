using System.CommandLine;
using System.CommandLine.Parsing;
using System.Security.Cryptography;

var countArgument = new Argument<int>("count") { Description = "Number of words (12, 15, 18, 21, or 24)" };
countArgument.DefaultValueFactory = _ => 12;
countArgument.Arity = ArgumentArity.ZeroOrOne;

var noColorsOption = new Option<bool>("--no-colors") { Description = "Disable colored output" };

var rootCommand = new RootCommand("BIP39 mnemonic generator");
rootCommand.Add(countArgument);
rootCommand.Add(noColorsOption);

rootCommand.SetAction(parseResult =>
{
    var count = parseResult.GetValue(countArgument);
    var noColors = parseResult.GetValue(noColorsOption);

    var wordCountParams = new Dictionary<int, (int EntropyBytes, int ChecksumBits, int TotalBits)>
    {
        [12] = (16, 4, 132),
        [15] = (20, 5, 165),
        [18] = (24, 6, 198),
        [21] = (28, 7, 231),
        [24] = (32, 8, 264),
    };

    if (!wordCountParams.TryGetValue(count, out var p))
    {
        Console.Error.WriteLine($"Invalid word count: {count}. Must be 12, 15, 18, 21, or 24.");
        return;
    }

    var entropy = RandomNumberGenerator.GetBytes(p.EntropyBytes);
    var hash = SHA256.HashData(entropy);

    var bits = string.Concat(entropy.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
    bits += Convert.ToString(hash[0], 2).PadLeft(8, '0')[..p.ChecksumBits];

    var words = new List<string>();
    for (var i = 0; i < p.TotalBits; i += 11)
    {
        words.Add(WordList.Words[Convert.ToInt32(bits.Substring(i, 11), 2)]);
    }

    if (noColors)
    {
        Console.WriteLine(string.Join(' ', words));
    }
    else
    {
        var colors = new[] { ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta };
        for (var i = 0; i < words.Count; i++)
        {
            Console.ForegroundColor = colors[(i / 3) % colors.Length];
            Console.Write(words[i]);
            Console.ResetColor();
            Console.Write(i < words.Count - 1 ? " " : "");
        }
        Console.WriteLine();
    }
});

var parserConfig = new ParserConfiguration();
var parsed = rootCommand.Parse(args, parserConfig);
return await parsed.InvokeAsync(new InvocationConfiguration());
