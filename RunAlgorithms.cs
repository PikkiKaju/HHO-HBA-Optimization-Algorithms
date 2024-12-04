using System;

// Represents the best performance of a fitness function based on test results.
public class BestFunction 
{
    // The fitness function being evaluated.
    public required FitnessFunction fitnessFunction { get; set; }
    // The test results associated with the fitness function.
    public required TestResults testResults { get; set; }

    // Returns a string representation of the fitness function and its test results.
    public string ToString(int roundingDigits)
    {
        return "" +
            fitnessFunction.Name +
            testResults.ToString(roundingDigits);
    }
}

// Stores the results of running optimization algorithms.
public class RunAlgorithmsResult
{
    // A list of all test results generated during the runs.
    public required List<TestResults> TestResultsList { get; set; }
    // A list of the best-performing functions based on their test results.
    public required List<BestFunction> BestFunctionsList { get; set; }

    // Clears both the test results and best functions lists.
    public void Clear()
    {
        TestResultsList.Clear();
        BestFunctionsList.Clear();
    }
}

// Orchestrates running multiple optimization algorithms on various fitness functions.
public class RunAlgorithms
{
    // Entry point to execute the algorithms and collect results.
    public static RunAlgorithmsResult Run()
    {
        // Propertie to store tests results
        List<TestResults> TestResultsList = new List<TestResults>();
        List<BestFunction> BestFunctionsList = new List<BestFunction>();

        // Define population and iteration values
        int[] populationSizes = { 10, 20, 40, 80 };
        int[] iterations = { 5, 10, 20, 40, 60, 80 };
        int[] dimensions = { 30 };

        // Initialize algorithms from HBA.cs and any other provided algorithms
        List<IOptimizationAlgorithm> algorithms = new List<IOptimizationAlgorithm>
        {
            new HoneyBadgerAlgorithm(),  
            new HarrisHawksOptimization()
        };

        // Counter to keep track of fitness function tests.
        int functionTestCount = 0;

        // Loop through all algorithms and fitness functions.
        foreach (var algorithm in algorithms)
        {
            foreach (var function in FitnessFunctions.List)
            {
                functionTestCount++;

                // Initialize a BestFunction object for the current function.
                BestFunctionsList.Add(new BestFunction { fitnessFunction = function, testResults = new TestResults() });

                // Run the tests for each population size and iteration combination.
                foreach (int popSize in populationSizes)
                {
                    foreach (int iter in iterations)
                    {
                        foreach (int dimension in dimensions)
                        {
                            // Run the algorithm with the current parameters and collect results.
                            TestResults result = RunAlgorithmTests(algorithm, function, popSize, iter, dimension);
                            TestResultsList.Add(result);

                            // Update the best function results if the new result is better.
                            if (result.ResultF < BestFunctionsList[functionTestCount].testResults.ResultF)
                            {
                                BestFunctionsList[functionTestCount] = new BestFunction { fitnessFunction = function, testResults = result };
                            }
                        }
                    }
                }

                functionTestCount++;
            }
        }

        // Return the collected results and best functions.
        return new RunAlgorithmsResult { TestResultsList = TestResultsList, BestFunctionsList = BestFunctionsList };
    }

    // Executes the tests for a specific algorithm and fitness function.
    private static TestResults RunAlgorithmTests(IOptimizationAlgorithm algorithm, FitnessFunction function, int populationSize, int maxIterations, int dimension)
    {
        // Store results of each run.
        List<double> results = new List<double>();
        for (int i = 0; i < 10; i++)
        {
            // Run the algorithm and capture the result.
            double result = algorithm.Solve(function, populationSize, maxIterations, dimension);
            results.Add(result);
        }

        // Analyze the results and generate a TestResults object.
        return AnalyzeAndPrintResults(results, algorithm, function, populationSize, maxIterations, false);
    }

    // Analyzes the results of the algorithm runs and optionally prints the output.
    private static TestResults AnalyzeAndPrintResults(List<double> results, IOptimizationAlgorithm algorithm, FitnessFunction function, int populationSize, int maxIterations, bool print = false)
    {
        // Calculate statistics for the results.
        double mean = CalculateMean(results);
        double stdDev = CalculateStandardDeviation(results, mean);
        double coefficientOfVariation = (stdDev / mean) * 100;

        // Optionally print details of the results.
        if (print)
        {
            Console.WriteLine($"Algorithm: {algorithm.Name}, Pop Size: {populationSize}, Iterations: {maxIterations}");
            //Console.WriteLine($"Function: {function.Name}, Domain: [{function.DomainMin} ; {function.DomainMax}], GlobalMin: {function.GlobalMin}");
            Console.WriteLine($"Mean: {mean}, Std Dev: {stdDev}, Coefficient of Variation: {coefficientOfVariation}%");
            Console.WriteLine($"Best: {results.Min()}, Worst: {results.Max()}");
            Console.WriteLine("--------------------------------------------------");
        }

        // Create and return a TestResults object with the statistics.
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
