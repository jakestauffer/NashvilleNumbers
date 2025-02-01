using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Pdf.Canvas.Parser.Data;

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
            var fileName = System.IO.Path.GetFileNameWithoutExtension(_filePath);
            var outputFilePath = _filePath.Replace(fileName, $"{fileName} - Numbers");

            using PdfReader reader = new PdfReader(_filePath);
            using PdfWriter writer = new PdfWriter(outputFilePath);
            using PdfDocument pdfDoc = new PdfDocument(reader, writer);

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                PdfPage page = pdfDoc.GetPage(i);
                var chords = ExtractChordsFromPage(page);
                ReplaceChordsOnPage(page, chords);
            }

            Console.WriteLine($"PDF saved: {outputFilePath}");
        }

        private List<(string text, Rectangle rectangle, float fontSize, string fontStyle)> ExtractChordsFromPage(PdfPage page)
        {
            var chords = new List<(string text, Rectangle rectangle, float fontSize, string fontStyle)>();

            // Define regex pattern for valid chords (e.g., C, Am, Bb, D/F#)
            string chordPattern = @"\b[A-G](#|b)?(maj|min|m|dim|aug|sus\d*|add\d*|7|9|11|13|6|b5|#5|b9|#9|#11|b13|no\d*)?(\/[A-G](#|b)?)?\b";

            // Use iText's built-in regex strategy
            var strategy = new RegexBasedLocationExtractionStrategy(new Regex(chordPattern));

            PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
            parser.ProcessPageContent(page);


            foreach (IPdfTextLocation match in strategy.GetResultantLocations())
            {
                string text = match.GetText();
                var rectangle = match.GetRectangle();
                float fontSize = rectangle.GetHeight();
                string fontStyle = "Helvetica";

                chords.Add((text, rectangle, fontSize, fontStyle));
            }

            return chords;
        }

        private void ReplaceChordsOnPage(PdfPage page, List<(string text, Rectangle rectangle, float fontSize, string fontStyle)> chords)
        {
            PdfCanvas canvas = new PdfCanvas(page);

            foreach (var (originalChord, rectangle, fontSize, fontStyle) in chords)
            {
                string nashvilleNumber = ConvertChordToNashville(originalChord);
                if (nashvilleNumber == null) continue;

                PdfFont font = PdfFontFactory.CreateFont(fontStyle);
                float textWidth = font.GetWidth(nashvilleNumber, fontSize);
                float textHeight = fontSize;

                // Adjust erasure box size
                var width = rectangle.GetWidth();
                var height = rectangle.GetHeight();

                float x = rectangle.GetX();
                float y = rectangle.GetY();

                canvas.SaveState();
                canvas.SetFillColor(iText.Kernel.Colors.ColorConstants.WHITE);
                canvas.Rectangle(rectangle);
                canvas.Fill();
                canvas.RestoreState();

                canvas.BeginText();
                canvas.SetFontAndSize(font, fontSize);
                canvas.MoveText(x, y);
                canvas.ShowText(nashvilleNumber);
                canvas.EndText();
            }
        }


        private string ConvertChordToNashville(string chord)
        {
            var nashvilleChord = new Chord(_key, chord);
            return nashvilleChord.NashvilleNumber ?? chord;
        }
    }
 }

