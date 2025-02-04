using MetaheuristicOptimizer.Calculations.Algorithms;
using MetaheuristicOptimizer.Calculations.HelperClasses;
using MetaheuristicOptimizer.Models;

namespace MetaheuristicOptimizer.Calculations
{
    // Represents the best performance of a fitness function based on test results.
    public class BestFunction
    {
        // The fitness function being evaluated.
        public required IFitnessFunction fitnessFunction { get; set; }
        // The test results associated with the fitness function.
        public required AlgorithmTestResult testResults { get; set; }
    }

    // Stores the results of running optimization algorithms.
    public class RunAlgorithmsResult
    {
        // A list of all test results generated during the runs.
        public required List<AlgorithmTestResult> TestResultsList { get; set; }
        // A list of the best-performing functions based on their test results.
        public required List<BestFunction> BestFunctionsList { get; set; }

        // Clears both the test results and best functions lists.
        public void Clear()
        {
            TestResultsList.Clear();
            BestFunctionsList.Clear();
        }
    }
    public static class AlgorithmCalculations
    {
        // Executes the tests for a specific algorithm and fitness function.
        public static AlgorithmTestResult RunAlgorithmTest(IOptimizationAlgorithm algorithm, IFitnessFunction function, int populationSize, int maxIterations, int dimension)
        {
            

            List<double> results = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                // Run the algorithm and capture the result.
                double result = algorithm.Solve(function, populationSize, maxIterations, dimension);
                results.Add(result);
            }

            // Analyze the results and generate a TestResults object.
            return AnalyzeResults(results, algorithm, function, populationSize, maxIterations);
        }

        private static AlgorithmTestResult AnalyzeResults(List<double> results, IOptimizationAlgorithm algorithm, IFitnessFunction function, int populationSize, int maxIterations)
        {
            // Calculate statistics for the results.
            double mean = CalculateMean(results);
            double stdDev = CalculateStandardDeviation(results, mean);
            double coefficientOfVariation = (stdDev / mean) * 100;

            // Create and return a TestResults object with the statistics.
            return new AlgorithmTestResult
            {
                ResultF = results.Min(),
                ResultX = algorithm.XBest,
                Mean = mean,
                StandardDeviation = stdDev,
                CoefficientOfVariation = coefficientOfVariation
            };
        }
        
        // Calculates the mean value of a list of numbers.
        private static double CalculateMean(List<double> values)
        {
            double sum = 0;
            foreach (double value in values)
            {
                sum += value;
            }
            return sum / values.Count;
        }
       
        // Calculates the standard deviation of a list of numbers.
        private static double CalculateStandardDeviation(List<double> values, double mean)
        {
            double sumOfSquares = 0;
            foreach (double value in values)
            {
                sumOfSquares += Math.Pow(value - mean, 2);
            }
            return Math.Sqrt(sumOfSquares / values.Count);
        }
    }
}
