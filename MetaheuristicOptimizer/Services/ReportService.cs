using iText.Layout;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using MetaheuristicOptimizer.Storage;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using System.Text.RegularExpressions;

namespace MetaheuristicOptimizer.Services
{
    public class ReportService
    {
        private static readonly string ReportsDirectory = "Reports";

        public string GenerateReport(bool isMultiAlgorithm)
        {
            // Check if folder is existing
            EnsureReportsDirectory();
            string filePath = GetReportFilePath();

            // Read a data from a file with test result or inform controller that the file is empty
            string results = FileStorage.ReadResults();
            if (string.IsNullOrWhiteSpace(results))
                throw new Exception("Brak danych do raportu.");

            // Create and write Pdf Document
            using (PdfWriter writer = new PdfWriter(filePath))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf))
            {
                // Fonts Settings
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

                // Create Title
                AddReportHeader(document, boldFont, regularFont);

                // For singleAlgorithm endpoint create a Dictionary of Algorithms which contains a Dictionary of Fitness Functions with parameters
                // For multiAlgorithm endpoint create a Dictionary of Fitness Function which contains Algorithms with parameters
                var groupedResults = GroupResults(results, isMultiAlgorithm);

                // Create a results table for report with multiAlgorithm or single Algorithm
                GenerateReportContent(document, groupedResults, boldFont, regularFont, isMultiAlgorithm);
            }

            // Delete Reports file
            FileStorage.ClearResults();
            return filePath;
        }

        private void EnsureReportsDirectory()
        {
            if (!Directory.Exists(ReportsDirectory))
            {
                Directory.CreateDirectory(ReportsDirectory);
            }
        }

        private string GetReportFilePath()
        {
            string fileName = $"report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
            return Path.Combine(ReportsDirectory, fileName);
        }

        private void AddReportHeader(Document document, PdfFont boldFont, PdfFont regularFont)
        {
            document.Add(new Paragraph("Raport z testów Metaheurystycznych")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph($"Data wygenerowania: {DateTime.Now:yyyy-MM-dd}\n")
                .SetFont(regularFont)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new LineSeparator(new SolidLine()));
            document.Add(new Paragraph("\nWyniki testów\n")
                .SetFont(boldFont)
                .SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER));
        }

        private Dictionary<string, Dictionary<string, List<string[]>>> GroupResults(string results, bool isMultiAlgorithm)
        {
            var groupedResults = new Dictionary<string, Dictionary<string, List<string[]>>>();
            string[] lines = results.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i += 10)
            {
                if (i + 9 >= lines.Length) break;

                string algorithmName = lines[i].Trim();
                string functionName = lines[i + 1].Trim();
                string[] values = {
                    lines[i + 2].Trim(), lines[i + 3].Trim(), lines[i + 4].Trim(),
                    $"[{lines[i + 5].Trim().Replace(";", ", ")}]", lines[i + 6].Trim(),
                    lines[i + 7].Trim(), lines[i + 8].Trim()
                };

                string primaryKey = isMultiAlgorithm ? functionName : algorithmName;
                string secondaryKey = isMultiAlgorithm ? algorithmName : functionName;

                if (!groupedResults.ContainsKey(primaryKey))
                    groupedResults[primaryKey] = new Dictionary<string, List<string[]>>();

                if (!groupedResults[primaryKey].ContainsKey(secondaryKey))
                    groupedResults[primaryKey][secondaryKey] = new List<string[]>();

                groupedResults[primaryKey][secondaryKey].Add(values);
            }

            return groupedResults;
        }

        private void GenerateReportContent(Document document, Dictionary<string, Dictionary<string, List<string[]>>> groupedResults,
                                           PdfFont boldFont, PdfFont regularFont, bool isMultiAlgorithm)
        {
            foreach (var primaryEntry in groupedResults)
            {
                document.Add(new Paragraph($"{(isMultiAlgorithm ? "FitnessFunction" : "Algorytm")}: {primaryEntry.Key}")
                    .SetFont(boldFont)
                    .SetFontSize(14));

                document.Add(new LineSeparator(new SolidLine()));

                foreach (var secondaryEntry in primaryEntry.Value)
                {
                    document.Add(new Paragraph($"{(isMultiAlgorithm ? "Algorithm" : "Funkcja Celu")}: {secondaryEntry.Key}")
                        .SetFont(boldFont)
                        .SetFontSize(12));

                    document.Add(CreateResultsTable(secondaryEntry.Value, boldFont, regularFont));
                    document.Add(new Paragraph("\n"));
                }
            }
        }

        private Table CreateResultsTable(List<string[]> results, PdfFont boldFont, PdfFont regularFont)
        {
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 10, 15, 25, 10, 10, 10 }))
                .UseAllAvailableWidth();

            string[] headers = { "Populacja", "Iteracja", "WynikF", "WynikX", "Średnia", "SD", "CV" };
            foreach (var header in headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(header)).SetFont(boldFont));
            }

            foreach (var entry in results)
            {
                foreach (var value in entry)
                {
                    table.AddCell(new Cell().Add(new Paragraph(value)).SetFont(regularFont));
                }
            }

            return table;
        }
    }
}


