# MyBudget — облік особистих витрат

Персональний менеджер витрат: категорії, бюджети, місячні підсумки, Swagger та автоматизовані тести.

## Стек
- .NET 9 (ASP.NET Core Web API)
- EF Core + SQLite
- Swagger (OpenAPI)
- xUnit (unit + integration)
- Git/GitHub

## Запуск (локально)
```bash
dotnet restore
dotnet ef database update --project src/MyBudget.Infrastructure --startup-project src/MyBudget.Api
ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/MyBudget.Api --urls http://127.0.0.1:5080
