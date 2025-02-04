namespace MetaheuristicOptimizer.Models
{
    public class SingleAlgorithmTestResponse
    {
        public Guid Id { get; set; }
        public string AlgorithmName { get; set; }
        public int PopulationSize { get; set; }
        public int Iterations { get; set; }
        public int Dimension { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<SingleAlgorithmTestResult> TestResults { get; set; }
    }
}
