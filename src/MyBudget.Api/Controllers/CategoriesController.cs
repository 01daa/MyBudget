using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBudget.Core.Entities;
using MyBudget.Infrastructure.Data;

namespace MyBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> Get([FromQuery] Guid userId)
        => Ok(await _db.Categories.Where(x => x.UserId == userId)
                                  .OrderBy(x => x.Name)
                                  .ToListAsync());

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] Category c, CancellationToken ct)
    {
        if (c.Id == Guid.Empty) c.Id = Guid.NewGuid();
        _db.Categories.Add(c);
        await _db.SaveChangesAsync(ct);
        return Ok(c);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Category c, CancellationToken ct)
    {
        var exists = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (exists is null) return NotFound();
        exists.Name = c.Name;
        exists.Type = c.Type;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var entity = await _db.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null) return NotFound();
        _db.Categories.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }
}
