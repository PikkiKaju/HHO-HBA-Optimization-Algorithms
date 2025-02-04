using MetaheuristicOptimizer.Calculations.HelperClasses;

namespace FitnessFunctions
{
    public class FitnessFunctionToDll : IFitnessFunction
    {
        public required string Name { get; set; } = "Function1";
        public required double[] MinDomain { get; set; } = new double[] { -10 };
        public required double[] MaxDomain { get; set; } = new double[] { 10 };
        public required int MaxDimensions { get; set; } = 0;
        public required double GlobalMin { get; set; } = 0;
        public required Func<double[], double> Function { get; set; } = delegate (double[] x)
        {
            double sum = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                sum += x[i] * x[i];
            }
            return sum;
        };
    }
}
