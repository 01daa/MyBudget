using Microsoft.EntityFrameworkCore;
using MyBudget.Core.Entities;
using MyBudget.Core.Services;
using MyBudget.Infrastructure.Data;
namespace MyBudget.Infrastructure.Services;
public sealed class TransactionService : ITransactionService
{
    private readonly AppDbContext _db;
    public TransactionService(AppDbContext db) { _db = db; }
    public async Task<Transaction> AddAsync(Transaction t, CancellationToken ct)
    {
        if (t.Id == Guid.Empty) t.Id = Guid.NewGuid();
        _db.Transactions.Add(t);
        await _db.SaveChangesAsync(ct);
        return t;
    }
    public async Task<decimal> GetMonthlyTotalAsync(Guid userId, int year, int month, CancellationToken ct)
    {
        var from = new DateTime(year, month, 1);
        var to = from.AddMonths(1);
        return await _db.Transactions
            .Where(x => x.UserId == userId && x.OccurredAt >= from && x.OccurredAt < to)
            .SumAsync(x => x.Amount, ct);
    }
}
