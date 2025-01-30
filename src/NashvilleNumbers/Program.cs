using NashvilleNumbers;

Console.Write("Enter the key of the song (e.g., C, G, D, etc.): ");
var enteredKey = Console.ReadLine();

if (string.IsNullOrWhiteSpace(enteredKey) || !MusicConstants.KeysWithChords.ContainsKey(enteredKey))
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
    var nashvilleNumber = chord.NashvilleNumber ?? $"Not found in the key of {enteredKey}";

    Console.WriteLine($"{chord.OriginalChord} => {nashvilleNumber}");
}