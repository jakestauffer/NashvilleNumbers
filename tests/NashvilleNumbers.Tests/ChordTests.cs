namespace NashvilleNumbers.Tests
{
    public class ChordTests
    {
        [Theory]
        [InlineData("C", "C", "1")]
        [InlineData("C", "D", "2")]
        [InlineData("C", "E", "3")]
        [InlineData("C", "F", "4")]
        [InlineData("C", "G", "5")]
        [InlineData("C", "A", "6")]
        [InlineData("C", "B", "7")]
        public void GivenValidKeyAndMajorChord_ShouldCorrectlyMapToNashvilleNumber(string key, string chord, string expectedNumber)
        {
            var testChord = new Chord(key, chord);
            Assert.Equal(expectedNumber, testChord.NashvilleNumber);
        }

        [Theory]
        [InlineData("C", "Cm", "1m")]
        [InlineData("C", "Dm", "2m")]
        [InlineData("C", "Em", "3m")]
        [InlineData("C", "Fm", "4m")]
        [InlineData("C", "Gm", "5m")]
        [InlineData("C", "Am", "6m")]
        [InlineData("C", "Bm", "7m")]
        public void GivenValidKeyAndMinorChord_ShouldCorrectlyMapToNashvilleNumber(string key, string chord, string expectedNumber)
        {
            var testChord = new Chord(key, chord);
            Assert.Equal(expectedNumber, testChord.NashvilleNumber);
        }

        [Theory]
        [InlineData("C", "C7", "1(7)")]
        [InlineData("C", "G7", "5(7)")]
        [InlineData("C", "Dm7", "2m(7)")]
        [InlineData("C", "Emaj7", "3(7)")]
        public void GivenValidKeyAndChordWithExtension_ShouldCorrectlyMapToNashvilleNumber(string key, string chord, string expectedNumber)
        {
            var testChord = new Chord(key, chord);
            Assert.Equal(expectedNumber, testChord.NashvilleNumber);
        }

        [Theory]
        [InlineData("C", "D/F#", "2/#4")]
        [InlineData("C", "G/B", "5/7")]
        [InlineData("C", "C/E", "1/3")]
        [InlineData("G", "D/F#", "5/7")]
        public void GivenValidKeyAndSlashChord_ShouldCorrectlyMapToNashvilleNumber(string key, string chord, string expectedNumber)
        {
            var testChord = new Chord(key, chord);
            Assert.Equal(expectedNumber, testChord.NashvilleNumber);
        }

        [Theory]
        [InlineData("C", "Bb", "b7")]
        [InlineData("C", "F#", "#4")]
        [InlineData("C", "Ab", "b6")]
        public void GivenValidKeyAndAccidentalChord_ShouldCorrectlyMapToNashvilleNumber(string key, string chord, string expectedNumber)
        {
            var testChord = new Chord(key, chord);
            Assert.Equal(expectedNumber, testChord.NashvilleNumber);
        }

        [Theory]
        [InlineData("C", "H")]
        [InlineData("C", "X")]
        public void GivenValidKeyAndInvalidChord_ShouldReturnNull(string key, string chord)
        {
            var testChord = new Chord(key, chord);
            Assert.Null(testChord.NashvilleNumber);
        }
    }
}
