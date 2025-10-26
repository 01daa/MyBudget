using Microsoft.AspNetCore.Mvc;
using MyBudget.Core.Entities;
using MyBudget.Core.Services;

namespace MyBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BudgetsController : ControllerBase
{
    private readonly IBudgetService _svc;
    public BudgetsController(IBudgetService svc) { _svc = svc; }

    [HttpPost]
    public async Task<ActionResult<Budget>> Create([FromBody] Budget b, CancellationToken ct)
        => Ok(await _svc.CreateAsync(b, ct));

    [HttpGet("progress")]
    public async Task<ActionResult<decimal>> Progress([FromQuery] Guid userId, [FromQuery] Guid categoryId, [FromQuery] string period, CancellationToken ct)
        => Ok(await _svc.GetProgressAsync(userId, categoryId, period, ct));
}
