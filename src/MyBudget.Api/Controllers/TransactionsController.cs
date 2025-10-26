using Microsoft.AspNetCore.Mvc;
using MyBudget.Core.Entities;
using MyBudget.Core.Services;

namespace MyBudget.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TransactionsController : ControllerBase
{
    private readonly ITransactionService _svc;
    public TransactionsController(ITransactionService svc) { _svc = svc; }

    [HttpPost]
    public async Task<ActionResult<Transaction>> Create([FromBody] Transaction t, CancellationToken ct)
        => Ok(await _svc.AddAsync(t, ct));

    [HttpGet("monthly-total")]
    public async Task<ActionResult<decimal>> MonthlyTotal([FromQuery] Guid userId, [FromQuery] int year, [FromQuery] int month, CancellationToken ct)
        => Ok(await _svc.GetMonthlyTotalAsync(userId, year, month, ct));
}
