namespace NashvilleNumbers
{
    public class Chord
    {
        public string Key { get; }
        public string OriginalChord { get; }
        public string? Root { get; }
        public string? SharpFlat { get; }
        public Chord? Bass { get; }
        public bool IsMinor { get; }
        public string? Extension { get; }
        public string? NashvilleNumber { get; }

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

            NashvilleNumber =
                Bass != null
                ? $"{GetNashvilleNumber(this)}/{GetNashvilleNumber(Bass)}"
                : GetNashvilleNumber(this);
        }

        private static string? GetNashvilleNumber(Chord chord)
        {
            if (string.IsNullOrWhiteSpace(chord.Root))
            {
                return null;
            }

            var keyChords = NashvilleNumbers.MusicConstants.KeysWithChords[chord.Key];
            var rootIndex = Array.IndexOf(keyChords, chord.Root);

            var nashvilleNumber = string.Empty;

            if (rootIndex >= 0)
            {
                nashvilleNumber = $"{rootIndex + 1}";
            }
            else if (!string.IsNullOrWhiteSpace(chord.SharpFlat))
            {
                int accidentalIndex = HandleAccidental(chord, keyChords);

                if (accidentalIndex >= 0)
                {
                    nashvilleNumber = $"{chord.SharpFlat}{accidentalIndex + 1}";
                }
            }

            if (string.IsNullOrWhiteSpace(nashvilleNumber))
            {
                return null;
            }

            if (chord.IsMinor)
            {
                nashvilleNumber += "m";
            }

            if (!string.IsNullOrEmpty(chord.Extension))
            {
                nashvilleNumber += $"({chord.Extension})";
            }

            return nashvilleNumber;
        }

        private static int HandleAccidental(Chord chord, string[] keyChords)
        {
            var noteWithoutSharpFlat = chord.Root!.Replace(chord.SharpFlat!, string.Empty);

            return Array.IndexOf(keyChords, noteWithoutSharpFlat);
        }
    }
}
