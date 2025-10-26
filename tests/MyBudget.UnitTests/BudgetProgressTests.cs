using Xunit;
using FluentAssertions;
public sealed class BudgetProgressTests
{
    [Fact]
    public void Progress_Caps_At_1()
    {
        decimal limit = 100m;
        decimal spent = 150m;
        var progress = Math.Min(1m, limit == 0 ? 0 : spent / limit);
        progress.Should().Be(1m);
    }
}
