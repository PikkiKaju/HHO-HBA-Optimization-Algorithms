using MetaheuristicOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetaheuristicOptimizer.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService = new();

        [HttpGet("single")]
        public IActionResult GenerateReportForSingleAlgorithm()
        {
            try
            {
                string filePath = _reportService.GenerateReport(false);
                return File(System.IO.File.ReadAllBytes(filePath), "application/pdf", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("multi")]
        public IActionResult GenerateReportForMultiAlgorithms()
        {
            try
            {
                string filePath = _reportService.GenerateReport(true);
                return File(System.IO.File.ReadAllBytes(filePath), "application/pdf", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
