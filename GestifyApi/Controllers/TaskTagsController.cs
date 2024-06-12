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
public class TaskTagsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskTagsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/TaskTags
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskTag>>> GetTaskTags()
    {
        return await _context.TaskTags.ToListAsync();
    }

    // GET: api/TaskTags/5
    [HttpGet("{taskId}/{tagId}")]
    public async Task<ActionResult<TaskTag>> GetTaskTag(int taskId, int tagId)
    {
        var taskTag = await _context.TaskTags
            .Where(tt => tt.TaskID == taskId && tt.TagID == tagId)
            .FirstOrDefaultAsync();

        if (taskTag == null)
        {
            return NotFound();
        }

        return taskTag;
    }

    // POST: api/TaskTags
    [HttpPost]
    public async Task<ActionResult<TaskTag>> PostTaskTag(TaskTag taskTag)
    {
        _context.TaskTags.Add(taskTag);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTaskTag", new { taskId = taskTag.TaskID, tagId = taskTag.TagID }, taskTag);
    }

    // DELETE: api/TaskTags/5
    [HttpDelete("{taskId}/{tagId}")]
    public async Task<IActionResult> DeleteTaskTag(int taskId, int tagId)
    {
        var taskTag = await _context.TaskTags
            .Where(tt => tt.TaskID == taskId && tt.TagID == tagId)
            .FirstOrDefaultAsync();

        if (taskTag == null)
        {
            return NotFound();
        }

        _context.TaskTags.Remove(taskTag);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
