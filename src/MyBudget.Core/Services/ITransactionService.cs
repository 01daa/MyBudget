using MyBudget.Core.Entities;
namespace MyBudget.Core.Services;
public interface ITransactionService
{
    Task<Transaction> AddAsync(Transaction t, CancellationToken ct);
    Task<decimal> GetMonthlyTotalAsync(Guid userId, int year, int month, CancellationToken ct);
}
