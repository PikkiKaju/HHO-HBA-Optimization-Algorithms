namespace MetaheuristicOptimizer.Models
{
    public class SingleAlgorithmTestResponse
    {
        public Guid Id { get; set; }
        public string AlgorithmName { get; set; }
        public int[] PopulationSizes { get; set; } = Array.Empty<int>();
        public int[] Iterations { get; set; } = Array.Empty<int>();
        public int Dimension { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SingleAlgorithmTestResult> TestResults { get; set; }
    }
}
