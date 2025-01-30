Console.Write("Enter the key of the song (e.g., C, G, D, etc.): ");
var enteredKey = Console.ReadLine();

if (string.IsNullOrWhiteSpace(enteredKey) || !NashvilleNumbers.MusicConstants.KeysWithChords.ContainsKey(enteredKey))
{
    Console.WriteLine("Invalid key. Please run the program again.");
    return;
}

Console.Write("Enter the chords in the song separated by spaces (e.g., C G Am F): ");
var enteredChords = Console.ReadLine();

if (string.IsNullOrWhiteSpace(enteredChords))
{
    Console.WriteLine("Invalid chords. Please run the program again.");
    return;
}

var chords = enteredChords!.Split(' ').Select(c => new Chord(enteredKey!, c)).ToList();

Console.WriteLine("\nChord to Nashville Number Mapping:");

foreach (var chord in chords)
{
    var nashvilleNumber = chord.GetNashvilleNumber() ?? $"Not found in the key of {enteredKey}";

    Console.WriteLine($"{chord.OriginalChord} => {nashvilleNumber}");
}

class Chord
{
    public string Key { get; }
    public string OriginalChord { get; }
    public string? Root { get; }
    public string? SharpFlat { get; }
    public Chord? Bass { get; }
    public bool IsMinor { get; }
    public string? Extension { get; }

    public Chord(string key, string chord)
    {
        Key = key;
        OriginalChord = chord;

        var notes = chord.Split("/");
        var fullRoot = notes.ElementAtOrDefault(0);
        var fullBass = notes.ElementAtOrDefault(1);

        Root = fullRoot != null ? new string(fullRoot.Where(c => char.IsAsciiLetterUpper(c) || c is '#' or 'b').ToArray()) : null;

        SharpFlat = fullRoot?.FirstOrDefault(c => c is '#' or 'b').ToString();

        Bass = fullBass != null ? new Chord(key, fullBass) : null;

        IsMinor = fullRoot?.Contains('m') == true && !fullRoot?.Contains("maj") == true;

        Extension = fullRoot != null ? new string(fullRoot.Where(char.IsDigit).ToArray()) : null;
    }

    public string? GetNashvilleNumber()
    {
        if (string.IsNullOrWhiteSpace(Root))
        {
            return null;
        }

        var keyChords = NashvilleNumbers.MusicConstants.KeysWithChords[Key];
        var rootIndex = Array.IndexOf(keyChords, Root);

        var nashvilleNumber = string.Empty;

        if (rootIndex >= 0)
        {
            nashvilleNumber = $"{rootIndex + 1}";
        }
        else if (!string.IsNullOrWhiteSpace(SharpFlat))
        {
            int accidentalIndex = HandleAccidental(this, keyChords);

            if (accidentalIndex >= 0)
            {
                nashvilleNumber = $"{SharpFlat}{accidentalIndex + 1}";
            }
        }

        if (string.IsNullOrWhiteSpace(nashvilleNumber))
        {
            return null;
        }

        if (IsMinor)
        {
            nashvilleNumber += "m";
        }

        if (!string.IsNullOrEmpty(Extension))
        {
            nashvilleNumber += $"({Extension})";
        }

        if (Bass != null)
        {
            nashvilleNumber += $"/{Bass.GetNashvilleNumber()}";
        }

        return nashvilleNumber;
    }

    private static int HandleAccidental(Chord chord, string[] keyChords)
    {
        var noteWithoutSharpFlat = chord.Root!.Replace(chord.SharpFlat!, string.Empty);

        return Array.IndexOf(keyChords, noteWithoutSharpFlat);
    }
}

