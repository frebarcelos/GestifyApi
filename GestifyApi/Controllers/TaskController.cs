using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestifyApi.Data;
using GestifyApi.Models;  
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }

    // GET: api/Tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    // PUT: api/Tasks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTask(int id, TaskModel task)
    {
        if (id != task.ID)
        {
            return BadRequest();
        }

        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Tasks
    [HttpPost]
    public async Task<ActionResult<TaskModel>> PostTask(TaskModel task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTask", new { id = task.ID }, task);
    }

    // DELETE: api/Tasks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.ID == id);
    }
}
