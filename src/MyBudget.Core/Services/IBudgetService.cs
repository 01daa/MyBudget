using MyBudget.Core.Entities;
namespace MyBudget.Core.Services;
public interface IBudgetService
{
    Task<Budget> CreateAsync(Budget b, CancellationToken ct);
    Task<decimal> GetProgressAsync(Guid userId, Guid categoryId, string period, CancellationToken ct);
}
