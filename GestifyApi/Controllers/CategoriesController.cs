using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestifyApi.Data;
using GestifyApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MySqlConnector;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CategoriesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    // GET: api/Categories/UserCategories
    [HttpGet("UserCategories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetUserCategories()
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

        var categories = await _context.Categories
                                       .Where(c => c.UserID == userId)
                                       .ToListAsync();

        return Ok(categories);
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.ID)
        {
            return BadRequest();
        }

        // Verifique se a categoria existe
        if (!CategoryExists(id))
        {
            return NotFound();
        }

        // Recupera a categoria do banco de dados
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound();
        }

        // Atualiza apenas o nome da categoria
        existingCategory.CategoryName = category.CategoryName;

        _context.Entry(existingCategory).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        catch (DbUpdateException dbEx)
        {
            // Log the error details
            Console.WriteLine($"DbUpdateException: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
            }

            // Check if the exception is due to foreign key constraint violation
            if (dbEx.InnerException is MySqlException sqlEx &&
                sqlEx.Number == 1452) // 1452 is the MySQL error code for foreign key constraint violation
            {
                return Conflict("Cannot update category due to foreign key constraint violation.");
            }

            throw;
        }

        return NoContent();
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
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

        category.UserID = userId;

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCategory", new { id = category.ID }, category);
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.ID == id);
    }
}
