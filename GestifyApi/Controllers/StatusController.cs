﻿using Microsoft.AspNetCore.Mvc;
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
public class StatusController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StatusController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Status
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
    {
        return await _context.Statuses.ToListAsync();
    }

    // GET: api/Status/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Status>> GetStatus(int id)
    {
        var status = await _context.Statuses.FindAsync(id);

        if (status == null)
        {
            return NotFound();
        }

        return status;
    }

    // PUT: api/Status/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStatus(int id, Status status)
    {
        if (id != status.ID)
        {
            return BadRequest();
        }

        _context.Entry(status).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StatusExists(id))
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

    // POST: api/Status
    [HttpPost]
    public async Task<ActionResult<Status>> PostStatus(Status status)
    {
        _context.Statuses.Add(status);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStatus", new { id = status.ID }, status);
    }

    // DELETE: api/Status/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStatus(int id)
    {
        var status = await _context.Statuses.FindAsync(id);
        if (status == null)
        {
            return NotFound();
        }

        _context.Statuses.Remove(status);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StatusExists(int id)
    {
        return _context.Statuses.Any(e => e.ID == id);
    }
}
