using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashvilleNumbers
{
    public static class MusicConstants
    {
        public static Dictionary<string, string[]> KeysWithChords = new Dictionary<string, string[]>()
        {
            { "C", new[] { "C", "D", "E", "F", "G", "A", "B" } },
            { "G", new[] { "G", "A", "B", "C", "D", "E", "F#" } },
            { "D", new[] { "D", "E", "F#", "G", "A", "B", "C#" } },
            { "A", new[] { "A", "B", "C#", "D", "E", "F#", "G#" } },
            { "E", new[] { "E", "F#", "G#", "A", "B", "C#", "D#" } },
            { "B", new[] { "B", "C#", "D#", "E", "F#", "G#", "A#" } },
            { "F#", new[] { "F#", "G#", "A#", "B", "C#", "D#", "E#" } },
            { "Db", new[] { "Db", "Eb", "F", "Gb", "Ab", "Bb", "C" } },
            { "Ab", new[] { "Ab", "Bb", "C", "Db", "Eb", "F", "G" } },
            { "Eb", new[] { "Eb", "F", "G", "Ab", "Bb", "C", "D" } },
            { "Bb", new[] { "Bb", "C", "D", "Eb", "F", "G", "A" } },
            { "F", new[] { "F", "G", "A", "Bb", "C", "D", "E" } }
        };
    }
}
