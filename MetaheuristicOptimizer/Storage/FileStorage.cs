using MetaheuristicOptimizer.Models;

namespace MetaheuristicOptimizer.Storage
{
    public static class FileStorage
    {
        private static readonly string FilePath = "results.txt";

        public static void SaveResult(AlgorithmTestResult result, int populationSize, int iteration, string algorithmName, string functionName)
        {
            // Save to a File with a 0.0001 Precision
            File.AppendAllText(FilePath, 
                $"{algorithmName}{Environment.NewLine}" +
                $"{functionName}{Environment.NewLine}" +
                $"{populationSize}{Environment.NewLine}" +
                $"{iteration}{Environment.NewLine}" +
                $"{result.ResultF:F4}{Environment.NewLine}" + 
                $"{String.Join(";", result.ResultX.Select(x => x.ToString("F4")))}{Environment.NewLine}" +
                $"{result.Mean:F4}{Environment.NewLine}" + 
                $"{result.StandardDeviation:F4}{Environment.NewLine}" + 
                $"{result.CoefficientOfVariation:F4}{Environment.NewLine}" + 
                $"--------------{Environment.NewLine}");
        }

        public static string ReadResults()
        {
            return File.Exists(FilePath) ? File.ReadAllText(FilePath) : "";
        }

        public static void ClearResults()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
