using NashvilleNumbers;

Console.WriteLine("Welcome to Nashville Numbers Converter");

Console.WriteLine(
    """
    1) Single chord sheet
    2) Chord sheets folder
    3) Manually enter chords
    """);
Console.Write("Choice: ");

var inputMethodAsString = Console.ReadLine();
if (int.TryParse(inputMethodAsString, out int inputMethod) && inputMethod is < 1 or > 3)
{
    Console.WriteLine("Invalid selection. Please run the program again.");
    return;
}

if (inputMethod == 1)
{
    Console.Write("Enter file path to chord sheet: ");
    var filePath = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(filePath))
    {
        Console.WriteLine("Invalid file path. Please run the program again.");
        return;
    }

    var key = MusicPrompter.GetKey();

    var fileName = Path.GetFileNameWithoutExtension(filePath);
    var outputPath = filePath.Replace(fileName, $"{fileName} - Numbers");
    new ChordSheetPdf(filePath, key).ToNashvilleNumberPdf();

    return;
}

if (inputMethod == 2)
{
    Console.Write("Enter folder path to chord sheets: ");
    var folderPath = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(folderPath))
    {
        Console.WriteLine("Invalid folder path. Please run the program again.");
        return;
    }

    var chordSheets = new List<ChordSheetPdf>();
    foreach (var filePath in Directory.GetFiles(folderPath))
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        Console.WriteLine($"Enter key for {filePath}");
        var key = MusicPrompter.GetKey();

        chordSheets.Add(new ChordSheetPdf(filePath, key));
    }

    chordSheets.ForEach(c => c.ToNashvilleNumberPdf());

    return;
}

var enteredKey = MusicPrompter.GetKey();

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