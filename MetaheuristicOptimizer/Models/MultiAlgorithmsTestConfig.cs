namespace MetaheuristicOptimizer.Models
{
    public class MultiAlgorithmsTestConfig
    {
        public int Id { get; set; }
        public string FitnessFunction { get; set; } = "";
        public List<string> AlgorithmName { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
