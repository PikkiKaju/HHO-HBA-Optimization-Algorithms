using MetaheuristicOptimizer.Models;
using MetaheuristicOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicOptimizer.Controllers
{
    [ApiController]
    [Route("api/algorithm")]
    public class AlgorithmController : ControllerBase
    {
        private readonly AlgorithmService _algorithmService = new();

        [HttpPost("run-single")]
        public IActionResult RunSingleAlgorithm([FromBody] SingleAlgorithmTestConfig request)
        {
            if (string.IsNullOrEmpty(request.AlgorithmName))
                return BadRequest("AlgorithmName is required.");

            SingleAlgorithmTestResponse result = _algorithmService.RunSingleAlgorithm(request);
            return Ok(new { message = "Algorithm executed", result });
        }

        [HttpPost("run-multi")]
        public IActionResult RunMultiAlgorithms([FromBody] MultiAlgorithmsTestConfig request)
        {
            if (request.AlgorithmName == null || request.AlgorithmName.Count == 0)
                return BadRequest("AlgorithmName is required");

            MultiAlgorithmsTestResponse result = _algorithmService.RunMultiAlgorithms(request);
            return Ok(new { message = "Algorithm executed", result });
        }
    }
}
