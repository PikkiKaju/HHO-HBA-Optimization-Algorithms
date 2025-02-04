namespace MetaheuristicOptimizer.Storage
{
    public static class FileStorage
    {
        private static readonly string FilePath = "results.txt";

        public static void SaveResult(string result)
        {
            // File.AppendAllText(FilePath, $"{DateTime.Now}: {result}{Environment.NewLine}");
        }

        public static string ReadResults()
        {
            return File.Exists(FilePath) ? File.ReadAllText(FilePath) : "";
        }
    }
}
