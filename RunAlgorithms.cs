using System;

public class BestFunction 
{
    public required FitnessFunction fitnessFunction { get; set; }
    public required TestResults testResults { get; set; }

    public string ToString(int roundingDigits)
    {
        return "" +
            fitnessFunction.Name +
            testResults.ToString(roundingDigits);
    }
}

public class RunAlgorithmsResult
{
    public required List<TestResults> TestResultsList { get; set; }
    public required List<BestFunction> BestFunctionsList { get; set; }

    public void Clear()
    {
        TestResultsList.Clear();
        BestFunctionsList.Clear();
    }
}

public class RunAlgorithms
{   
    public static RunAlgorithmsResult Run()
    {
        // Propertie to store tests results
        List<TestResults> TestResultsList = new List<TestResults>();
        List<BestFunction> BestFunctionsList = new List<BestFunction>();

        // Define population and iteration values
        int[] populationSizes = { 10, 20, 40, 80 };
        int[] iterations = { 5, 10, 20, 40, 60, 80 };
        int dimension = 3;

        // Initialize algorithms from HBA.cs and any other provided algorithms
        List<IOptimizationAlgorithm> algorithms = new List<IOptimizationAlgorithm>
        {
            new HoneyBadgerAlgorithm(),  // Assuming HBA implements IOptimizationAlgorithm
            new HarrisHawksOptimization()
        };

        int functionTestCount = 0;

        foreach (var algorithm in algorithms)
        {
            foreach (var function in FitnessFunctions.List)
            {
                functionTestCount++;

                BestFunctionsList.Add(new BestFunction { fitnessFunction = function, testResults = new TestResults() });

                foreach (int popSize in populationSizes)
                {
                    foreach (int iter in iterations)
                    {
                        TestResults result = RunAlgorithmTests(algorithm, function, popSize, iter, dimension);
                        TestResultsList.Add(result);
                        if (result.ResultF < BestFunctionsList[functionTestCount - 1].testResults.ResultF)
                        {
                            BestFunctionsList[functionTestCount - 1] = (new BestFunction { fitnessFunction = function, testResults = result});
                        }
                    }
                }
            }
        }

        return new RunAlgorithmsResult { TestResultsList = TestResultsList, BestFunctionsList = BestFunctionsList };
    }

    private static TestResults RunAlgorithmTests(IOptimizationAlgorithm algorithm, FitnessFunction function, int populationSize, int maxIterations, int dimension)
    {
        List<double> results = new List<double>();
        for (int i = 0; i < maxIterations; i++) 
        {
            double result = algorithm.Solve(function, populationSize, maxIterations, dimension);
            results.Add(result);
        }

        return AnalyzeAndPrintResults(results, algorithm, function, populationSize, maxIterations, false);
    }

    private static TestResults AnalyzeAndPrintResults(List<double> results, IOptimizationAlgorithm algorithm, FitnessFunction function, int populationSize, int maxIterations, bool print = false)
    {
        double mean = CalculateMean(results);
        double stdDev = CalculateStandardDeviation(results, mean);
        double coefficientOfVariation = (stdDev / mean) * 100;

        if (print)
        {
            Console.WriteLine($"Algorithm: {algorithm.Name}, Pop Size: {populationSize}, Iterations: {maxIterations}");
            //Console.WriteLine($"Function: {function.Name}, Domain: [{function.DomainMin} ; {function.DomainMax}], GlobalMin: {function.GlobalMin}");
            Console.WriteLine($"Mean: {mean}, Std Dev: {stdDev}, Coefficient of Variation: {coefficientOfVariation}%");
            Console.WriteLine($"Best: {results.Min()}, Worst: {results.Max()}");
            Console.WriteLine("--------------------------------------------------");
        }

        return new TestResults
        {
            Algorithm = algorithm,
            Function = function,
            PopulationSize = populationSize,
            Iterations = maxIterations,
            ResultF = results.Min(),
            ResultX = algorithm.XBest,
            Mean = mean,
            StandardDeviation = stdDev,
            CoefficientOfVariation = coefficientOfVariation
        };
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
