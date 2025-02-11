using MetaheuristicOptimizer.Calculations.Algorithms;
using MetaheuristicOptimizer.Calculations.HelperClasses;
using MetaheuristicOptimizer.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC;

namespace MetaheuristicOptimizer.Controllers
{
    [ApiController]
    [Route("api/update")]
    public class UpdateNamesController : ControllerBase
    {
        [HttpGet("functions")]
        public IActionResult UpdateFunctionsName()
        {
            // Load functions from Built-In functions memory
            var functionNames = FitnessFunctions.List.Select(f => f.Name);

            // Load functions from Dynamic Memory
            var newFunctions = ReadDllFileService.LoadFitnessFunctionsFromDll();

            // Concat these Lists but without duplicates if there are any new functions
            if (newFunctions != null && newFunctions.Any())
            {
                foreach (var function in newFunctions)
                {
                    if (!FitnessFunctions.List.Any(a => a.Name == function.Name))
                    {
                        FitnessFunctions.AddFitnessFunction(function);
                    }
                }
            }

            return Ok(functionNames);
        }

        [HttpGet("algorithms")]
        public IActionResult UpdateAlgorithmsName()
        {
            // Load algorithms from Built-In algorithms memory
            var algorithmNames = OptimizationAlgorithms.List.Select(f => f.Name);

            // Load algorithms from Dynamic Memory
            var newAlgorithms = ReadDllFileService.LoadAlgorithmsFromDll();

            // Concat these Lists but without duplicates if there are any newAlgorithms
            if (newAlgorithms != null && newAlgorithms.Any())
            {
                foreach (var algorithm in newAlgorithms)
                {
                    if (!OptimizationAlgorithms.List.Any(a => a.Name == algorithm.Name))
                    {
                        OptimizationAlgorithms.AddOptimizationAlgorithm(algorithm);
                    }
                }
            }

            // return updated algorithms Names
            return Ok(algorithmNames);
        }
    }
}
