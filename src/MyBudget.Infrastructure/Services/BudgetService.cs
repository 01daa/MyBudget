using Microsoft.EntityFrameworkCore;
using MyBudget.Core.Entities;
using MyBudget.Core.Services;
using MyBudget.Infrastructure.Data;

namespace MyBudget.Infrastructure.Services;
public sealed class BudgetService : IBudgetService
{
    private readonly AppDbContext _db;
    public BudgetService(AppDbContext db) { _db = db; }

    public async Task<Budget> CreateAsync(Budget b, CancellationToken ct)
    {
        if (b.Id == Guid.Empty) b.Id = Guid.NewGuid();
        _db.Set<Budget>().Add(b);
        await _db.SaveChangesAsync(ct);
        return b;
    }

    public async Task<decimal> GetProgressAsync(Guid userId, Guid categoryId, string period, CancellationToken ct)
    {
        var limit = await _db.Set<Budget>()
            .Where(x => x.UserId == userId && x.CategoryId == categoryId && x.Period == period)
            .Select(x => x.LimitAmount)
            .FirstOrDefaultAsync(ct);
        if (limit <= 0) return 0m;

        if (!DateTime.TryParse(period + "-01", out var from)) return 0m;
        var to = from.AddMonths(1);

        var spent = await _db.Transactions
            .Where(x => x.UserId == userId && x.CategoryId == categoryId && x.OccurredAt >= from && x.OccurredAt < to)
            .SumAsync(x => x.Amount, ct);

        var v = spent / limit;
        return v >= 1m ? 1m : v;
    }
}
