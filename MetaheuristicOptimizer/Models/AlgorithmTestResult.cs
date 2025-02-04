namespace MetaheuristicOptimizer.Models
{
    public class AlgorithmTestResult
    {
        // Result of the fitness function
        public double ResultF { get; set; } = double.MaxValue;
        public double[] ResultX { get; set; } = null;
        public double Mean { get; set; } = double.MaxValue;
        public double StandardDeviation { get; set; } = double.MaxValue;
        public double CoefficientOfVariation { get; set; } = double.MaxValue;
    }
}
