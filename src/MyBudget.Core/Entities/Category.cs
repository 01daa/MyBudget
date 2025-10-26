namespace MyBudget.Core.Entities;
public sealed class Category
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "Expense";
}
