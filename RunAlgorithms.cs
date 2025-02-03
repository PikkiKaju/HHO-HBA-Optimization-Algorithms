using System;
using System.Diagnostics;
using System.Reflection;

// Represents the best performance of a fitness function based on test results.
public class BestFunction
{
    // The fitness function being evaluated.
    public required IFitnessFunction fitnessFunction { get; set; }
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
    // Define the cancelation token used for stoping the running algorithm tests
    private static CancellationTokenSource tokenSource = new();
    private static bool isRunning = false;

    // Initialize algorithms from HBA.cs and any other provided algorithms
    private static List<IOptimizationAlgorithm> algorithms = new List<IOptimizationAlgorithm>();
    private static List<IFitnessFunction> functions = new List<IFitnessFunction>();

    // Entry point to execute the algorithms and collect results.
    public static RunAlgorithmsResult Run(CancellationToken cancellationToken = new())
    {
        // Check if the algorithms are loaded
        if (algorithms.Count == 0)
        {
            throw new Exception("algorithms List is empty");
        }

        // Propertie to store tests results
        List<TestResults> TestResultsList = new List<TestResults>();
        List<BestFunction> BestFunctionsList = new List<BestFunction>();

        // Define population and iteration values
        int[] populationSizes = { 10, 20, 40, 80 };
        int[] iterations = { 5, 10, 20, 40, 60, 80 };
        int[] dimensions = { 3 };


        // Counter to keep track of fitness function tests.
        int functionTestCount = 0;

        // Loop through all algorithms and fitness functions.
        Parallel.ForEach(algorithms, (algorithm, state) =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                state.Stop();
                return;
            }
            foreach (var function in DefaultFitnessFunctions.List)
            {
                // Initialize a BestFunction object for the current function.
                BestFunction bestFunction = new BestFunction { fitnessFunction = function, testResults = new TestResults() };

                // Run the tests for each population size and iteration combination.
                foreach (int popSize in populationSizes)
                {
                    foreach (int iter in iterations)
                    {
                        foreach (int dimension in dimensions)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                state.Stop();
                                return;
                            }
                            // Run the algorithm with the current parameters and collect results.
                            TestResults result = RunAlgorithmTests(algorithm, function, popSize, iter, dimension);
                            lock (TestResultsList)
                            {
                                TestResultsList.Add(result);
                            }

                            // Update the best function results if the new result is better.
                            lock (BestFunctionsList)
                            {
                                if (result.ResultF < bestFunction.testResults.ResultF)
                                {
                                    bestFunction = new BestFunction { fitnessFunction = function, testResults = result };
                                    BestFunctionsList.Add(bestFunction);
                                }
                            }
                        }
                    }
                }

                functionTestCount++;
            }
        });

        // Return the collected results and best functions.
        return new RunAlgorithmsResult { TestResultsList = TestResultsList, BestFunctionsList = BestFunctionsList };
    }

    // Asynchronously run the algorithms and collect results.
    public static async Task<RunAlgorithmsResult> RunAsync()
    {
        tokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = tokenSource.Token;
        isRunning = true;

        RunAlgorithmsResult results = await Task.Run(() => Run(cancellationToken));
        tokenSource.Dispose();
        isRunning = false;
        return results; 
    }

    // Abort the execution of the algorithms.
    public static void StopAsync()
    {
        if (isRunning)
        {
            tokenSource.Cancel();
        }
    }

    public static bool IsRunning()
    {
        return isRunning;
    }

    // Dospose of the Cancellation Token
    public static void Dispose()
    {
        tokenSource.Dispose();
    }

    // Executes the tests for a specific algorithm and fitness function.
    private static TestResults RunAlgorithmTests(IOptimizationAlgorithm algorithm, IFitnessFunction function, int populationSize, int maxIterations, int dimension)
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
    private static TestResults AnalyzeAndPrintResults(List<double> results, IOptimizationAlgorithm algorithm, IFitnessFunction function, int populationSize, int maxIterations, bool print = false)
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


    // Loads all classes that implement IOptimizationAlgorithm from the current assembly.
    public static List<IOptimizationAlgorithm> LoadAlgorithmsDefault()
    {
        var algorithms = new List<IOptimizationAlgorithm>();
        var algorithmType = typeof(IOptimizationAlgorithm);
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => algorithmType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

        if (types.Count() != 0)
        {
            foreach (var type in types)
            {
                var algorithm = (IOptimizationAlgorithm)Activator.CreateInstance(type);
                algorithms.Add(algorithm);
                if (!RunAlgorithms.algorithms.Contains(algorithm))
                {
                    RunAlgorithms.algorithms.Add(algorithm);
                }
            }
        }
        else
        {
            throw new Exception($"The default DLL file(s) could not be read.");
        }
        return algorithms;
    }

    // Loads all classes that implement IOptimizationAlgorithm from the DLL in specified path.
    public static List<IOptimizationAlgorithm> LoadAlgorithmsFromDll(string dllDirectory = "C:\\.Projects\\Visual_Studio_cs_projects\\Zastosowania-Sztucznej-Inteligencji\\DLLs\\OptimizationAlgorithms")
    {
        var algorithms = new List<IOptimizationAlgorithm>();

        if (!Directory.Exists(dllDirectory))
        {
            throw new FileNotFoundException($"The specified DLL directory was not found: {dllDirectory}");
        }

        var dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

        if (dllFiles.Count() == 0)
        {
            throw new FileNotFoundException($"The specified DLL directory does not contain any files: {dllDirectory}");
        }

        foreach (var dll in dllFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dll);

                var algorithmTypes = assembly.GetTypes()
                    .Where(t => typeof(IOptimizationAlgorithm).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in algorithmTypes)
                {
                    if (Activator.CreateInstance(type) is IOptimizationAlgorithm algorithmInstance)
                    {
                        if (!RunAlgorithms.algorithms.Any(a => a.GetType() == algorithmInstance.GetType()))
                        //if (!RunAlgorithms.algorithms.Contains(algorithmInstance))
                        {
                            RunAlgorithms.algorithms.Add(algorithmInstance);
                            algorithms.Add(algorithmInstance);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception($"The DLL file(s) from the specified directory could not be read: {dllDirectory}");
            }
        }

        return algorithms;
    }

    // Add an optimization algorithm to the algorithms list.
    public static bool AddAlgorithm(IOptimizationAlgorithm algorithm)
    {
        if (!RunAlgorithms.algorithms.Contains(algorithm))
        {
            RunAlgorithms.algorithms.Add(algorithm);
            return true;
        }
        return false;
    }

    // Get the algorithms list.
    public static List<IOptimizationAlgorithm> GetAlgorithms()
    {
        return RunAlgorithms.algorithms;
    }


    // Loads the Fitness fiunctions from the current assembly.
    public static List<IFitnessFunction> LoadFitnessFunctionsDefault()
    {
        foreach (var function in DefaultFitnessFunctions.List)
        {
            if (!RunAlgorithms.functions.Contains(function))
            {
                RunAlgorithms.functions.Add(function);
            }
        }

        return DefaultFitnessFunctions.List;
    }

    public static List<IFitnessFunction> LoadFitnessFunctionsFromDll(string dllDirectory = "C:\\.Projects\\Visual_Studio_cs_projects\\Zastosowania-Sztucznej-Inteligencji\\DLLs\\FitnessFunctions")
    {
        var functions = new List<IFitnessFunction>();

        if (!Directory.Exists(dllDirectory))
        {
            throw new FileNotFoundException($"The specified DLL directory was not found: {dllDirectory}");
        }

        var dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

        if (dllFiles.Count() == 0)
        {
            throw new FileNotFoundException($"The specified DLL directory does not contain any files: {dllDirectory}");
        }

        foreach (string dll in dllFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dll);

                var functionTypes = assembly.GetTypes()
                    .Where(t => typeof(IFitnessFunction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                Debug.Write(functionTypes.Count());

                foreach (var type in functionTypes)
                {
                    if (Activator.CreateInstance(type) is IFitnessFunction functionInstance)
                    {
                        if (!RunAlgorithms.functions.Contains(functionInstance))
                        {
                            RunAlgorithms.functions.Add(functionInstance);
                            functions.Add(functionInstance);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception($"The DLL file(s) from the specified directory could not be read: {dllDirectory}");
            }
        }

        return functions;
    }

    public static bool AddFunction(IFitnessFunction function)
    {
        if (!RunAlgorithms.functions.Contains(function))
        {
            RunAlgorithms.functions.Add(function);
            return true;
        }
        return false;
    }

    public static List<IFitnessFunction> GetFunctions()
    {
        return RunAlgorithms.functions;
    }
}