using Microsoft.AspNetCore.Mvc;
using Backend.Services.Report;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers.APIs
{
    [Route("api/report")]
    [ApiController]
    public class ReportApiController : Controller
    {
        private readonly IReportService _reportService;

        public ReportApiController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize]
        public GeneralReportDto Get()
        {
            var currentUser = HttpContext.User;

            var result = _reportService.GetAllGeneralGraphs();
            return result;
        }
    }
}