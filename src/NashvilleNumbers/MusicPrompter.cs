using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashvilleNumbers
{
    public static class MusicPrompter
    {
        public static string GetKey()
        {
            Console.Write("Enter the key of the song (e.g., C, G, D, etc.): ");

            string? enteredKey = null;
            while (string.IsNullOrWhiteSpace(enteredKey) || !MusicConstants.KeysWithChords.ContainsKey(enteredKey))
            {
                enteredKey = Console.ReadLine();
            }

            return enteredKey;
        }
    }
}
