using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public sealed class TransactionsApiTests : IClassFixture<CustomWebAppFactory>
{
    private readonly HttpClient _client;
    public TransactionsApiTests(CustomWebAppFactory f) { _client = f.CreateClient(); }

    [Fact]
    public async Task Post_Then_Get_MonthlyTotal()
    {
        var userId = Guid.NewGuid();
        var t = new { id = Guid.Empty, userId, accountId = Guid.NewGuid(), amount = 50.0, occurredAt = DateTime.UtcNow };
        var r1 = await _client.PostAsJsonAsync("/api/transactions", t);
        r1.EnsureSuccessStatusCode();
        var now = DateTime.UtcNow;
        var r2 = await _client.GetAsync($"/api/transactions/monthly-total?userId={userId}&year={now.Year}&month={now.Month}");
        r2.EnsureSuccessStatusCode();
        var total = await r2.Content.ReadFromJsonAsync<decimal>();
        Assert.True(total >= 50m);
    }
}
