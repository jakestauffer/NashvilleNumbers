using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;

namespace NashvilleNumbers
{
    public class ChordSheetPdf
    {
        private string _filePath;
        private string _key;

        public ChordSheetPdf(string filePath, string key)
        {
            _filePath = filePath;
            _key = key;
        }

        public void ToNashvilleNumberPdf()
        {
            var extractedText = ExtractTextFromPdf(_filePath);

            var modifiedText = ReplaceChordsWithNashvilleNumbers(extractedText);

            var fileName = Path.GetFileNameWithoutExtension(_filePath);
            var outputFilePath = _filePath.Replace(fileName, $"{fileName} - Numbers");

            SavePdf(modifiedText, outputFilePath);
        }

        private static string ExtractTextFromPdf(string pdfPath)
        {
            using PdfReader reader = new PdfReader(pdfPath);
            using PdfDocument pdfDoc = new PdfDocument(reader);

            var textWriter = new StringWriter();

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                textWriter.WriteLine(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i), new LocationTextExtractionStrategy()));
            }

            return textWriter.ToString();
        }

        private string ReplaceChordsWithNashvilleNumbers(string text)
        {
            string chordPattern = @"\b[A-G][#b]?(maj|min|m|dim|sus|aug)?\d*(\/[A-G][#b]?)?\b";

            return Regex.Replace(text, chordPattern, match =>
            {
                var chord = new Chord(_key, match.Value);
                return chord.NashvilleNumber ?? chord.OriginalChord;
            });
        }


        static void SavePdf(string text, string outputPdfPath)
        {
            using PdfWriter writer = new PdfWriter(outputPdfPath);
            using PdfDocument pdfDoc = new PdfDocument(writer);

            var document = new Document(pdfDoc);
            document.Add(new Paragraph(text));
        }
    }
}
