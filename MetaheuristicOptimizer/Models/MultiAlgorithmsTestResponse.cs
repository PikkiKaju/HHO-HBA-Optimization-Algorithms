namespace MetaheuristicOptimizer.Models
{
    public class MultiAlgorithmsTestResponse
    {
        public Guid Id { get; set; }
        public string FitnessFunctionName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MultiAlgorithmsTestResult> TestResults { get; set; }
    }
}
