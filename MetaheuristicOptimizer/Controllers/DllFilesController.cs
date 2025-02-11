using MetaheuristicOptimizer.Models;
using MetaheuristicOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicOptimizer.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class DllFilesController : ControllerBase
    {
        private readonly FileUploadService _fileUploadService = new();

        [HttpPost("function")]
        public IActionResult UploadFunctionFile(IFormFile file)
        {
            string result = _fileUploadService.UploadFunction(file);
            return result.StartsWith("Error") ? BadRequest(result) : Ok(result);
        }
        
        [HttpPost("algorithm")]
        public IActionResult UploadAlgorithmFile(IFormFile file)
        {
            string result = _fileUploadService.UploadAlgorithm(file);
            return result.StartsWith("Error") ? BadRequest(result) : Ok(result);
        }
    }
}