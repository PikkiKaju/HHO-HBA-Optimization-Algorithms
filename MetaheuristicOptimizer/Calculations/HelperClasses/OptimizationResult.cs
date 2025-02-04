namespace MetaheuristicOptimizer.Calculations.HelperClasses
{
    public class OptimizationResult
    {
        public required double[] xBest { get; set; }
        public required double fBest { get; set; }
        public required int numberOfEvaluationFitnessFunction { get; set; }
    }
}
