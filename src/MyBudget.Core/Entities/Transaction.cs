namespace MyBudget.Core.Entities;
public sealed class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime OccurredAt { get; set; }
    public string? Note { get; set; }
    public string? Merchant { get; set; }
}
