using System.CommandLine;
using System.CommandLine.Parsing;
using System.Security.Cryptography;

var countOption = new Option<int>("--count", "-n") { Description = "Number of words (12, 15, 18, 21, or 24)" };
countOption.DefaultValueFactory = _ => 12;
countOption.Validators.Add(result =>
{
    var value = result.GetValue(countOption);
    if (!BipParams.WordCountParams.ContainsKey(value))
    {
        result.AddError($"Invalid word count: {value}. Must be 12, 15, 18, 21, or 24.");
    }
});

var separatorOption = new Option<string>("--separator", "-s") { Description = "Separator between words" };
separatorOption.DefaultValueFactory = _ => " ";

var noColorsOption = new Option<bool>("--no-colors") { Description = "Disable colored output" };

var rootCommand = new RootCommand("BIP39 mnemonic generator");
rootCommand.Add(countOption);
rootCommand.Add(separatorOption);
rootCommand.Add(noColorsOption);

rootCommand.SetAction(parseResult =>
{
    var count = parseResult.GetValue(countOption);
    var separator = parseResult.GetValue(separatorOption)!;
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
        Console.WriteLine(string.Join(separator, words));
    }
    else
    {
        var colors = new[] { ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta };
        for (var i = 0; i < words.Count; i++)
        {
            Console.ForegroundColor = colors[(i / 3) % colors.Length];
            Console.Write(words[i]);
            Console.ResetColor();
            if (i < words.Count - 1) Console.Write(separator);
        }
        Console.WriteLine();
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
