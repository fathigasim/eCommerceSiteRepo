using efcoreApi.BasketService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcoreApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        IReportService reportService;
        public ReportController(IReportService _reportService) {
        reportService = _reportService;
        }
        [HttpGet("OrderReport")]
        public async Task< IActionResult> GetAsync()
        {
            var orders=await reportService.OrderReport();
            return Ok(orders);
        }

        [HttpGet("OrderByDate")]
        public async Task<IActionResult> GetDateAsync(DateTime dt)
        {
            var orders = await reportService.OrderByDateReport(dt);
            return Ok(orders);
        }
    }
}
