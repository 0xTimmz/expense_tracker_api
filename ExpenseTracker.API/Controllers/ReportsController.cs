using ExpenseTracker.Application.Queries;
using ExpenseTracker.Infrastructure.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly GetMonthlySummaryHandler _summaryHandler;
        private readonly GetMonthlyTotalHandler _totalHandler;

        public ReportsController(
            GetMonthlySummaryHandler summaryHandler,
            GetMonthlyTotalHandler totalHandler)
        {
            _summaryHandler = summaryHandler;
            _totalHandler = totalHandler;
        }

        [HttpGet("monthly-summary")]
        public async Task<IActionResult> GetMonthlySummary([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _summaryHandler.Handle(new GetMonthlySummaryQuery { Month = month, Year = year });
            return Ok(result);
        }

        [HttpGet("monthly-total")]
        public async Task<IActionResult> GetMonthlyTotal([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _totalHandler.Handle(new GetMonthlyTotalQuery { Month = month, Year = year });
            return Ok(new { month, year, total = result });
        }
    }
}