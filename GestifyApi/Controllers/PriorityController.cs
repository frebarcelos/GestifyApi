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
public class PriorityController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PriorityController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Priorities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Priority>>> GetPriorities()
    {
        return await _context.Priorities.ToListAsync();
    }

    // GET: api/Priorities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Priority>> GetPriority(int id)
    {
        var priority = await _context.Priorities.FindAsync(id);

        if (priority == null)
        {
            return NotFound();
        }

        return priority;
    }

    // PUT: api/Priorities/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPriority(int id, Priority priority)
    {
        if (id != priority.ID)
        {
            return BadRequest();
        }

        _context.Entry(priority).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PriorityExists(id))
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

    // POST: api/Priorities
    [HttpPost]
    public async Task<ActionResult<Priority>> PostPriority(Priority priority)
    {
        _context.Priorities.Add(priority);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPriority", new { id = priority.ID }, priority);
    }

    // DELETE: api/Priorities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePriority(int id)
    {
        var priority = await _context.Priorities.FindAsync(id);
        if (priority == null)
        {
            return NotFound();
        }

        _context.Priorities.Remove(priority);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PriorityExists(int id)
    {
        return _context.Priorities.Any(e => e.ID == id);
    }
}
