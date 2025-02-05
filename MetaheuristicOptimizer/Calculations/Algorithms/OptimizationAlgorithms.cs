using MetaheuristicOptimizer.Calculations.HelperClasses;

namespace MetaheuristicOptimizer.Calculations.Algorithms
{
    public class OptimizationAlgorithms
    {
        public static List<IOptimizationAlgorithm> List { get; } = new List<IOptimizationAlgorithm>
        {
            new HoneyBadgerAlgorithm(),
            new HarrisHawksOptimization()
        };
        public static void AddOptimizationAlgorithm(IOptimizationAlgorithm algorithm)
        {
            List.Add(algorithm);
        }
        public static IOptimizationAlgorithm GetAlgorithm(string algorithmName)
        {
            return List.Where(algorithm => algorithm.Name == algorithmName).FirstOrDefault();
        }
    }
}
