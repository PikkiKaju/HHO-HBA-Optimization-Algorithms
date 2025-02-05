using MetaheuristicOptimizer.Models;
using MetaheuristicOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicOptimizer.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class DllFilesController : ControllerBase
    {
        private readonly AlgorithmService _algorithmService = new();

        [HttpPost("function")]
        public IActionResult UploadFunctionFile(IFormFile file)
        {
            return Ok(new FileUploadService().UploadFunction(file));
        }
        
        [HttpPost("algorithm")]
        public IActionResult UploadAlgorithmFile(IFormFile file)
        {
            return Ok(new FileUploadService().UploadAlgorithm(file));
        }
    }
}