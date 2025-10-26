namespace MyBudget.Core.Entities;
public sealed class Budget
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string Period { get; set; } = string.Empty;
    public decimal LimitAmount { get; set; }
}
