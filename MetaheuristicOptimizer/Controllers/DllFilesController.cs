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
            return Ok(_fileUploadService.UploadFunction(file));
        }
        
        [HttpPost("algorithm")]
        public IActionResult UploadAlgorithmFile(IFormFile file)
        {
            return Ok(_fileUploadService.UploadAlgorithm(file));
        }
    }
}