using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Solvers;
using MetaheuristicOptimizer.Calculations;
using MetaheuristicOptimizer.Calculations.Algorithms;
using MetaheuristicOptimizer.Calculations.HelperClasses;
using MetaheuristicOptimizer.Models;
using MetaheuristicOptimizer.Storage;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MetaheuristicOptimizer.Services
{

    public class AlgorithmService
    {
        private CancellationTokenSource tokenSource = new();

        public SingleAlgorithmTestResponse RunSingleAlgorithm(SingleAlgorithmTestConfig request)
        {
            IOptimizationAlgorithm algorithm = request.AlgorithmName switch
            {
                "HHO" => new HarrisHawksOptimization(),
                "HBA" => new HoneyBadgerAlgorithm(),
                _ => throw new ArgumentException($"Unknown algorithm: {request.AlgorithmName}")
            };

            if (algorithm == null)
            {
                throw new InvalidOperationException("Algorithm was not initialized.");
            }

            List<SingleAlgorithmTestResult> testResults = new List<SingleAlgorithmTestResult>();

            // Store results of each run.
            foreach (var functionName in request.FitnessFunctions)
            {
                IFitnessFunction function = FitnessFunctions.List
                    .FirstOrDefault(f => f.Name.Equals(functionName, StringComparison.OrdinalIgnoreCase));

                if (function == null)
                {
                    throw new ArgumentException($"Unknown fitness function: {functionName}");
                }

                // Initialize an object to store the best results for a given function test on different parameters
                AlgorithmTestResult bestResult = new AlgorithmTestResult();
                int bestPopulationsSize = 0;
                int bestIterations = 0;

                foreach (int popSize in request.PopulationSizes)
                {
                    foreach (int iter in request.Iterations)
                    {
                        var algorithmResult = AlgorithmCalculations.RunAlgorithmTest(algorithm, function, popSize, iter, request.Dimension);

                        if (algorithmResult.ResultF < bestResult.ResultF)
                        {
                            bestResult = algorithmResult;
                            bestPopulationsSize = popSize;
                            bestIterations = iter;
                        }
                    }
                }

                testResults.Add(new SingleAlgorithmTestResult
                {
                    FitnessFunctionName = functionName,
                    PopulationSize = bestPopulationsSize,
                    Iterations = bestIterations,
                    ResultF = bestResult.ResultF,
                    ResultX = bestResult.ResultX,
                    Mean = bestResult.Mean,
                    StandardDeviation = bestResult.StandardDeviation,
                    CoefficientOfVariation = bestResult.CoefficientOfVariation
                });
            }

            var response = new SingleAlgorithmTestResponse
            {
                Id = Guid.NewGuid(),
                AlgorithmName = request.AlgorithmName,
                PopulationSizes = request.PopulationSizes,
                Iterations = request.Iterations,
                Dimension = request.Dimension,
                CreatedAt = DateTime.UtcNow,
                TestResults = testResults
            };

            // string json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            //FileStorage.SaveResult(json);

            return response;
        }

        public MultiAlgorithmsTestResponse RunMultiAlgorithms(MultiAlgorithmsTestConfig request)
        {
            var testResults = new List<MultiAlgorithmsTestResult>();

            var selectedFitnessFunction = FitnessFunctions.List.FirstOrDefault(f => f.Name == request.FitnessFunction);
            if (selectedFitnessFunction == null)
            {
                throw new Exception($"Fitness function {request.FitnessFunction} not found.");
            }

            int[] populationSizes = { 10, 20, 40, 80 };
            int[] iterations = { 5, 10, 20, 40, 60, 80 };
            int[] dimensions = { 3 };

            if (request.AlgorithmName == null || !request.AlgorithmName.Any())
            {
                throw new ArgumentException("No algorithms specified in the request.");
            }

            foreach (var algorithmName in request.AlgorithmName)
            {            
                IOptimizationAlgorithm algorithm = algorithmName switch
                {
                    "HHO" => new HarrisHawksOptimization(),
                    "HBA" => new HoneyBadgerAlgorithm(),
                    _ => throw new ArgumentException($"Unknown algorithm: {algorithmName}")
                };

                var bestResult = new MultiAlgorithmsTestResult
                {
                    AlgorithmName = algorithmName,
                    ResultF = double.MaxValue
                };

                var resultsList = new List<double>();

                foreach (int popSize in populationSizes)
                {
                    foreach (int iter in iterations)
                    {
                        foreach (int dimension in dimensions)
                        {
                            var result = AlgorithmCalculations.RunAlgorithmTest(algorithm, selectedFitnessFunction, popSize, iter, dimension);
                            resultsList.Add(result.ResultF);

                            if (result.ResultF < bestResult.ResultF)
                            {
                                bestResult = new MultiAlgorithmsTestResult
                                {
                                    AlgorithmName = algorithmName,
                                    PopulationSize = popSize,
                                    Iterations = iter,
                                    ResultF = result.ResultF,
                                    ResultX = result.ResultX
                                };
                            }
                        }
                    }
                }
                if (resultsList.Any())
                {
                    bestResult.Mean = resultsList.Average();
                    bestResult.StandardDeviation = Math.Sqrt(resultsList.Average(v => Math.Pow(v - bestResult.Mean, 2)));
                    bestResult.CoefficientOfVariation = bestResult.Mean != 0 ? bestResult.StandardDeviation / bestResult.Mean : 0;
                }

                testResults.Add(bestResult);
            }

            var response = new MultiAlgorithmsTestResponse
            {
                Id = Guid.NewGuid(),
                FitnessFunctionName = request.FitnessFunction,
                CreatedAt = DateTime.UtcNow,
                TestResults = testResults
            };

            // string json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            //FileStorage.SaveResult(json);

            return response;

        }
    }
}
