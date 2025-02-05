namespace MetaheuristicOptimizer.Models
{
    public class SingleAlgorithmTestConfig
    {
        public int Id { get; set; }
        public string AlgorithmName { get; set; } = "";
        public int[] PopulationSizes { get; set; } = Array.Empty<int>();    // {10, 20, 40, 80}
        public int[] Iterations { get; set; } = Array.Empty<int>();         // {5, 10, 20, 40, 60, 80}
        public int Dimension { get; set; }          // {from 1 to 30}
        public List<string> FitnessFunctions { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
