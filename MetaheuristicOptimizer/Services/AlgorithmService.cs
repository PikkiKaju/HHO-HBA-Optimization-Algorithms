using MetaheuristicOptimizer.Calculations;
using MetaheuristicOptimizer.Calculations.Algorithms;
using MetaheuristicOptimizer.Calculations.HelperClasses;
using MetaheuristicOptimizer.Models;
using MetaheuristicOptimizer.Storage;
using System.Text.Json;

namespace MetaheuristicOptimizer.Services
{

    public class AlgorithmService
    {
        public AlgorithmTestResponse RunSingleAlgorithm(SingleAlgorithmTestConfig request)
        {
            IOptimizationAlgorithm algorithm = request.AlgorithmName switch
            {
                "HawkOptimization" => new HarrisHawksOptimization(),
                "HoneyBadger" => new HoneyBadgerAlgorithm(),
                _ => throw new ArgumentException($"Unknown algorithm: {request.AlgorithmName}")
            };

            if (algorithm == null)
            {
                throw new InvalidOperationException("Algorithm was not initialized.");
            }

            List<TestResult> testResults = new List<TestResult>();

            // Store results of each run.
            foreach (var functionName in request.FitnessFunctions)
            {
                FitnessFunction function = FitnessFunctions.List
                    .FirstOrDefault(f => f.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase));

                if (function == null)
                {
                    throw new ArgumentException($"Unknown fitness function: {functionName}");
                }

                var algorithmResult = AlgorithmCalculations.RunAlgorithmTest(algorithm, function, request.PopulationSize, request.Iterations, request.Dimension);

                testResults.Add(new TestResult
                {
                    FitnessFunctionName = functionName,
                    ResultF = algorithmResult.ResultF,
                    ResultX = algorithmResult.ResultX,
                    Mean = algorithmResult.Mean,
                    StandardDeviation = algorithmResult.StandardDeviation,
                    CoefficientOfVariation = algorithmResult.CoefficientOfVariation
                });
            }
           
            var response = new AlgorithmTestResponse
            {
                Id = Guid.NewGuid(),
                AlgorithmName = request.AlgorithmName,
                PopulationSize = request.PopulationSize,
                Iterations = request.Iterations,
                Dimension = request.Dimension,
                CreatedAt = DateTime.UtcNow,
                TestResults = testResults
            };

            string json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            //FileStorage.SaveResult(json);

            return response;
        }

        public string RunMultiAlgorithms(MultiAlgorithmsTestConfig request)
        {
            string algorithmNames = string.Join(", ", request.AlgorithmName);
            var response = $"Running {algorithmNames} on {request.FitnessFunction}";

            FileStorage.SaveResult(response);

            return response;
        }

    }
}
