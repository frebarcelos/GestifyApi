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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TasksController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: api/Tasks/UserTasks
    [HttpGet("UserTasks")]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetUserTasks()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserID");
        if (userIdClaim == null)
        {
            return Unauthorized("User ID claim not found.");
        }

        var userId = userIdClaim.Value;
        var tasks = await _context.Tasks
                                  .Include(t => t.Category)
                                  .Include(t => t.Status)
                                  .Include(t => t.Priority)
                                  .Include(t => t.User)
                                  .Where(t => t.User.ID.ToString() == userId)
                                  .ToListAsync();

        return Ok(tasks);
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
        var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserID");

        if (userIdClaim == null)
        {
            return Unauthorized("User ID not found in token");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return BadRequest("Invalid User ID in token");
        }

        task.UserID = userId;
        task.CreationDate = DateTime.UtcNow;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.ID }, task);
    }

    // GET: api/Tasks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskModel>> GetTask(int id)
    {
        var task = await _context.Tasks
                                 .Include(t => t.Category)
                                 .Include(t => t.Status)
                                 .Include(t => t.Priority)
                                 .Include(t => t.User)
                                 .FirstOrDefaultAsync(t => t.ID == id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
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
