using Logotera.Context;
using Logotera.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logotera.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController(CalendarDbContext context) : ControllerBase
{
    [HttpGet("{date}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByDate(DateTime date)
    {
        var tasks = await context.Tasks
            .Where(t => t.Date.Date == date.Date)
            .ToListAsync();

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult> AddTask([FromBody] TaskItem task)
    {
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
        return Ok(task);
    }

    [HttpPut("{id}/toggle")]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task == null) return NotFound();

        task.IsCompleted = !task.IsCompleted;
        await context.SaveChangesAsync();

        return Ok(task);
    }
}