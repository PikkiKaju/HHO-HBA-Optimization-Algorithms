namespace MetaheuristicOptimizer.Models
{
    public class SingleAlgorithmTestResult
    {
        public string FitnessFunctionName { get; set; }
        public double ResultF { get; set; }
        public double[] ResultX { get; set; }
        public double Mean { get; set; }
        public double StandardDeviation { get; set; }
        public double CoefficientOfVariation { get; set; }
    }
}
