using System.CommandLine;
using System.CommandLine.Parsing;
using System.Security.Cryptography;

var countArgument = new Argument<int>("count") { Description = "Number of words (12, 15, 18, 21, or 24)" };
countArgument.DefaultValueFactory = _ => 12;
countArgument.Arity = ArgumentArity.ZeroOrOne;
countArgument.Validators.Add(result =>
{
    var value = result.GetValue(countArgument);
    if (!BipParams.WordCountParams.ContainsKey(value))
    {
        result.AddError($"Invalid word count: {value}. Must be 12, 15, 18, 21, or 24.");
    }
});

var noColorsOption = new Option<bool>("--no-colors") { Description = "Disable colored output" };

var rootCommand = new RootCommand("BIP39 mnemonic generator");
rootCommand.Add(countArgument);
rootCommand.Add(noColorsOption);

rootCommand.SetAction(parseResult =>
{
    var count = parseResult.GetValue(countArgument);
    var noColors = parseResult.GetValue(noColorsOption) || Console.IsOutputRedirected;

    var p = BipParams.WordCountParams[count];

    var entropy = RandomNumberGenerator.GetBytes(p.EntropyBytes);
    var hash = SHA256.HashData(entropy);

    var bits = string.Concat(entropy.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
    bits += Convert.ToString(hash[0], 2).PadLeft(8, '0')[..p.ChecksumBits];

    var words = new List<string>();
    for (var i = 0; i < count * 11; i += 11)
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
        try
        {
            for (var i = 0; i < words.Count; i++)
            {
                Console.ForegroundColor = colors[(i / 3) % colors.Length];
                Console.Write(words[i]);
                Console.ResetColor();
                Console.Write(i < words.Count - 1 ? " " : "");
            }
            Console.WriteLine();
        }
        finally
        {
            Console.ResetColor();
        }
    }

    return 0;
});

var parserConfig = new ParserConfiguration();
var parsed = rootCommand.Parse(args, parserConfig);
return await parsed.InvokeAsync(new InvocationConfiguration());

static class BipParams
{
    internal static readonly IReadOnlyDictionary<int, (int EntropyBytes, int ChecksumBits)> WordCountParams =
        new Dictionary<int, (int EntropyBytes, int ChecksumBits)>
        {
            [12] = (16, 4),
            [15] = (20, 5),
            [18] = (24, 6),
            [21] = (28, 7),
            [24] = (32, 8),
        };
}
