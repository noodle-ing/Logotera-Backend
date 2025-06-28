using Logotera.Context;
using Logotera.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logotera.Controllers;

[ApiController]
[Route("api/reports")]

public class WeeklyReportsController(CalendarDbContext context) : ControllerBase
{
    [HttpGet("{weekStart}")]
    public async Task<ActionResult<WeeklyReport?>> GetReport(DateTime weekStart)
    {
        var report = await context.WeeklyReports
            .FirstOrDefaultAsync(r => r.WeekStart.Date == weekStart.Date);

        return Ok(report);
    }

    [HttpPost]
    public async Task<ActionResult> SaveReport([FromBody] WeeklyReport report)
    {
        var existing = await context.WeeklyReports
            .FirstOrDefaultAsync(r => r.WeekStart.Date == report.WeekStart.Date);

        if (existing != null)
        {
            existing.Text = report.Text;
        }
        else
        {
            context.WeeklyReports.Add(report);
        }

        await context.SaveChangesAsync();
        return Ok(report);
    }
}