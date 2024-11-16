using System;
using System.Collections.Generic;
using System.Linq;


public class RunAlgorithms
{   
    public static void Main(string[] args)
    {
        // Define population and iteration values
        int[] populationSizes = { 10, 20, 40, 80 };
        int[] iterations = { 5, 10, 20, 40, 60, 80 };
        int dimension = 30;

        // Initialize algorithms from HBA.cs and any other provided algorithms
        List<IOptimizationAlgorithm> algorithms = new List<IOptimizationAlgorithm>
        {
            new HoneyBadgerAlgorithm(),  // Assuming HBA implements IOptimizationAlgorithm
            new HarrisHawksOptimization()
        };

        foreach (var algorithm in algorithms)
        {
            foreach (var function in TestFunctions.FunctionInfos)
            {
                foreach (int popSize in populationSizes)
                {
                    foreach (int iter in iterations)
                    {
                        RunAlgorithmTests(algorithm, function, popSize, iter, dimension);
                    }
                }
            }
        }
    }

    private static void RunAlgorithmTests(IOptimizationAlgorithm algorithm, TestFunctions.FunctionInfo function, int populationSize, int maxIterations, int dimension)
    {
        List<double> results = new List<double>();
        for (int i = 0; i < 10; i++)  // Run 10 times to evaluate stability
        {
            double result = algorithm.Solve(function.Function, populationSize, maxIterations, dimension);
            results.Add(result);
        }

        AnalyzeAndPrintResults(results, algorithm.Name, function, populationSize, maxIterations);
    }

    private static void AnalyzeAndPrintResults(List<double> results, string algorithmName, TestFunctions.FunctionInfo function, int populationSize, int maxIterations)
    {
        double mean = CalculateMean(results);
        double stdDev = CalculateStandardDeviation(results, mean);
        double coefficientOfVariation = (stdDev / mean) * 100;

        Console.WriteLine($"Algorithm: {algorithmName}, Pop Size: {populationSize}, Iterations: {maxIterations}");
        Console.WriteLine($"Function: {function.Name}, Domain: [{function.MinX} ; {function.MaxX}], GlobalMin: {function.GlobalMin}");
        Console.WriteLine($"Mean: {mean}, Std Dev: {stdDev}, Coefficient of Variation: {coefficientOfVariation}%");
        Console.WriteLine($"Best: {results.Min()}, Worst: {results.Max()}");
        Console.WriteLine("--------------------------------------------------");
    }

    private static double CalculateMean(List<double> values)
    {
        double sum = 0;
        foreach (double value in values)
        {
            sum += value;
        }
        return sum / values.Count;
    }

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
