using iText.Layout;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using MetaheuristicOptimizer.Storage;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

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
                    // Create Bold Fonts
                    PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

                    Document document = new Document(pdf);
                    document.Add(new Paragraph("Raport z testów metaheurystycznych").SetFont(boldFont).SetFontSize(16));
                    document.Add(new Paragraph($"Data wygenerowania: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n"));

                    document.Add(new Paragraph("Wyniki testów:\n"));
                    document.Add(new Paragraph(results));

                    document.Close();
                }
            }

            return filePath;
        }
    }
}


