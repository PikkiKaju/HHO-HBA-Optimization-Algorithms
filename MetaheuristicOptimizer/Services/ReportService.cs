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
        private static readonly string ReportsDirectory = "reports";

        public string GenerateReport()
        {
            if (!Directory.Exists(ReportsDirectory))
            {
                Directory.CreateDirectory(ReportsDirectory);
            }

            string fileName = $"report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";
            string filePath = Path.Combine(ReportsDirectory, fileName);

            string results = FileStorage.ReadResults();
            if (string.IsNullOrWhiteSpace(results))
                throw new Exception("Brak danych do raportu.");

            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    // Fonts Settings
                    PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                    PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

                    // Create Pdf Document
                    Document document = new Document(pdf);

                    // Create Title
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

                    // Create a Dictionary of Algorithms which contains a Dictionary of Fitness Functions with parameters
                    var groupedResults = new Dictionary<string, Dictionary<string, List<string[]>>>();

                    // Copy Everything from file to variable lines
                    string[] lines = results.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    // Group From Files to variable groupReasults 
                    for (int i = 0; i < lines.Length; i += 10)
                    {
                        if (i + 9 >= lines.Length) break;

                        string algorithmName = lines[i].Trim();
                        string functionName = lines[i + 1].Trim();
                        string populationSize = lines[i + 2].Trim();
                        string iteration = lines[i + 3].Trim();
                        string resultF = lines[i + 4].Trim();
                        string resultX = lines[i + 5].Trim().Replace(";", "; ");
                        string mean = lines[i + 6].Trim();
                        string standardDeviation = lines[i + 7].Trim();
                        string coefficientOfVariation = lines[i + 8].Trim();

                        if (!groupedResults.ContainsKey(algorithmName))
                            groupedResults[algorithmName] = new Dictionary<string, List<string[]>>();

                        if (!groupedResults[algorithmName].ContainsKey(functionName))
                            groupedResults[algorithmName][functionName] = new List<string[]>();

                        groupedResults[algorithmName][functionName].Add(new string[]
                        {
                        populationSize, iteration, resultF, $"[{resultX}]", mean, standardDeviation, coefficientOfVariation
                        });
                    }

                    foreach (var algorithmEntry in groupedResults)
                    {
                        document.Add(new Paragraph($"Algorytm: {algorithmEntry.Key}")
                            .SetFont(boldFont)
                            .SetFontSize(14));

                        // Create a results in Pdf file for every FitnessFunction with a custom parameters in table 
                        foreach (var functionEntry in algorithmEntry.Value)
                        {
                            document.Add(new Paragraph($"Funkcja Celu: {functionEntry.Key}")
                                .SetFont(boldFont)
                                .SetFontSize(12));

                            document.Add(new LineSeparator(new SolidLine()));

                            // Creating a header of table
                            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 10, 15, 25, 10, 10, 10 }))
                                .UseAllAvailableWidth();
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Populacja")).SetFont(boldFont));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Iteracja")).SetFont(boldFont));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("WynikF")).SetFont(boldFont));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("WynikX")).SetFont(boldFont));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("Srednia")).SetFont(boldFont));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("SD")).SetFont(boldFont));
                            table.AddHeaderCell(new Cell().Add(new Paragraph("CV")).SetFont(boldFont));

                            // Put a value in correct row
                            foreach (var entry in functionEntry.Value)
                            {
                                table.AddCell(new Cell().Add(new Paragraph(entry[0])).SetFont(regularFont));
                                table.AddCell(new Cell().Add(new Paragraph(entry[1])).SetFont(regularFont));
                                table.AddCell(new Cell().Add(new Paragraph(entry[2])).SetFont(regularFont));
                                table.AddCell(new Cell().Add(new Paragraph(entry[3])).SetFont(regularFont));
                                table.AddCell(new Cell().Add(new Paragraph(entry[4])).SetFont(regularFont));
                                table.AddCell(new Cell().Add(new Paragraph(entry[5])).SetFont(regularFont));
                                table.AddCell(new Cell().Add(new Paragraph(entry[6])).SetFont(regularFont));
                            }

                            document.Add(table);
                            document.Add(new Paragraph("\n"));
                        }
                    }

                    document.Close();
                }
            }

            FileStorage.ClearResults();
            return filePath;
        }
    }
}


