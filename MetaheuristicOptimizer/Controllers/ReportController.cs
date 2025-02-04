using MetaheuristicOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicOptimizer.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService = new();

        [HttpGet("generate")]
        public IActionResult GenerateReport()
        {
            try
            {
                string filePath = _reportService.GenerateReport();
                return File(System.IO.File.ReadAllBytes(filePath), "application/pdf", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
