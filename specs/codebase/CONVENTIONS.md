# Convenções de Código - SprintUp Monolito Modular

## 📋 1. Estrutura do Projeto

- ✅ **Projeto ÚNICO** em `.NET 8` (sem múltiplos .csproj desnecessários)
- ✅ Organização por **Módulos** dentro de `src/Modules/`
- ✅ Cada módulo contém: `Domain/`, `Features/` e `Infrastructure/`
- ✅ Testes espelham a estrutura: `tests/Modules/{NomeModulo}/Features/`

## 🎯 2. Implementação de Features (SOLID - Single Responsibility)

### Estrutura de uma Feature
```
src/Modules/Strava/Features/ImportActivity/
├── ImportActivityEndpoint.cs      (Minimal API - HTTP)
├── ImportActivityHandler.cs        (Lógica de Caso de Uso - Single Responsibility)
├── ImportActivityDto.cs            (Contrato de Entrada/Saída)
└── ImportActivityRequest.cs        (Validações)
```

### Regras Obrigatórias:

1. **Não use Controllers** - Use `MapPost`, `MapGet`, etc. (Minimal APIs)
2. **Endpoint é FINO** - Apenas validação de input e chamada ao Handler
3. **Handler contém a LÓGICA** - Todo cálculo, regra de negócio vai aqui
4. **Separação Clara**:
   - Endpoint = Exposição HTTP
   - Handler = Lógica de Negócio
   - DTO = Contrato (não é lógica)

### Exemplo de Endpoint (ImportActivityEndpoint.cs):
```csharp
public static void MapImportActivityEndpoint(this WebApplication app)
{
    app.MapPost("/api/v1/activities/import", ImportActivity)
        .WithName("ImportActivity")
        .WithOpenApi();
}

private static async Task<IResult> ImportActivity(
    ImportActivityRequest request,
    IImportActivityHandler handler,
    CancellationToken ct)
{
    var result = await handler.Handle(request, ct);
    return result.Match(
        success => Results.Ok(success),
        failure => Results.Problem(detail: failure)
    );
}
```

## 💎 3. Domínio e DDD (Rich Domain Model)

### Entidades Ricas (Encapsulam Regras de Negócio):
```csharp
public class StravaActivity
{
    public Guid Id { get; private set; }
    public required string StravaActivityId { get; private set; }
    public required double Distance { get; private set; }
    public required int MovingTimeSeconds { get; private set; }
    public required double PaceMinPerKm { get; private set; } // Calculado
    
    // ✅ Construtor privado força regras
    private StravaActivity() { }
    
    // ✅ Factory Method com validações
    public static Result<StravaActivity> CreateFromStrava(StravaActivityDto dto)
    {
        if (dto.Type != "Run")
            return Result.Failure<StravaActivity>("Apenas corridas são importadas");
        
        if (dto.Distance <= 0 || dto.MovingTimeSeconds <= 0)
            return Result.Failure<StravaActivity>("Distância ou tempo inválidos");
        
        var pace = CalculatePace(dto.Distance, dto.MovingTimeSeconds);
        
        return Result.Success(new StravaActivity
        {
            Id = Guid.NewGuid(),
            StravaActivityId = dto.Id.ToString(),
            Distance = dto.Distance,
            MovingTimeSeconds = dto.MovingTimeSeconds,
            PaceMinPerKm = pace
        });
    }
    
    // ✅ Método de Negócio
    private static double CalculatePace(double distanceMeters, int movingTimeSeconds)
    {
        var distanceKm = distanceMeters / 1000.0;
        var timeMinutes = movingTimeSeconds / 60.0;
        return distanceKm > 0 ? timeMinutes / distanceKm : 0;
    }
}
```

### Regras Obrigatórias no Domínio:

1. **Construtor Privado** - Força uso de Factory Methods
2. **Propriedades Críticas com `required`** - Não deixar null acidental
3. **Sem setters públicos** - Use métodos: `activity.UpdatePace()`
4. **Cálculos e Validações dentro da Entidade** - Não no Handler
5. **Retornar `Result<T>` para erros** - Não exceções

## 🧪 4. Harness e Testes (xUnit + FluentAssertions)

### Estrutura de Teste:
```
tests/Modules/Strava/Features/ImportActivityTests.cs
```

### Padrão AAA (Arrange, Act, Assert):
```csharp
[Fact]
public void ShouldCalculatePaceCorrectly()
{
    // Arrange
    var dto = new StravaActivityDto 
    { 
        Distance = 10000,
        MovingTimeSeconds = 3000,
        Type = "Run"
    };
    
    // Act
    var result = StravaActivity.CreateFromStrava(dto);
    
    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.PaceMinPerKm.Should().BeCloseTo(5.0, 0.1);
}
```

### Regras de Teste:

1. **Um cenário = Um `[Fact]`**
2. **Teste nomes descritivos**: `Should{Action}When{Condition}`
3. **Use FluentAssertions** - `.Should().Be()` em vez de `Assert.Equal()`
4. **Mock apenas dependências externas** - BD, APIs, não lógica local

## ⚡ 5. Padrões de Código C# Obrigatórios

### Async/Await
- ✅ Todo método de I/O deve ser `async`
- ✅ Use `CancellationToken` em TODOS os métodos async
- ✅ Exemplo:
```csharp
public async Task<Result<StravaActivity>> SaveAsync(
    StravaActivity activity, 
    CancellationToken ct)
{
    _context.StravaActivities.Add(activity);
    await _context.SaveChangesAsync(ct);
    return Result.Success(activity);
}
```

### Tipos Primitivos Fortes (Strong Types)
- ✅ Use `record` para DTOs
- ✅ Use `class` para Entidades
- ✅ Exemplo:
```csharp
// ✅ DTO como record
public record ImportActivityRequest(
    string StravaActivityId,
    double Distance,
    int MovingTimeSeconds,
    string Type
);

// ✅ Entidade como class
public class StravaActivity { ... }
```

### IResult Nativo
- ✅ Retorne `IResult` do .NET nativo
- ✅ Use `Results.Ok()`, `Results.BadRequest()`, `Results.Problem()`
- ✅ Nunca lance exceções em Handlers - use `Result<T>`

## 🗄️ 6. Banco de Dados (PostgreSQL + EF Core)

### DbContext Único
```csharp
public class SprintUpDbContext : DbContext
{
    public DbSet<StravaActivity> StravaActivities { get; set; }
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<StravaActivity>()
            .HasKey(x => x.Id);
        
        modelBuilder.Entity<StravaActivity>()
            .Property(x => x.Id)
            .HasDefaultValueSql("gen_random_uuid()");
    }
}
```

### Regras de BD:
- ✅ Chaves primárias como `Guid` (UUID)
- ✅ Uma Migration por Feature (quando necessária)
- ✅ Usar EF Core Fluent API (não Data Annotations)
- ✅ Migrations rodadas via Makefile: `make migrate`

## 📡 7. API e Versionamento

### Rotas
- ✅ Padrão: `/api/v1/{recurso}`
- ✅ Exemplo: `/api/v1/activities/import`

### Respostas de Erro
- ✅ Use ProblemDetails do RFC 7231
- ✅ Exemplo:
```csharp
Results.Problem(
    detail: "Email já cadastrado",
    statusCode: StatusCodes.Status400BadRequest,
    title: "Validação falhou"
)
```

## 🚀 8. Dependências Externas

### O que Usar:
- ✅ **Banco de Dados**: PostgreSQL (EF Core)
- ✅ **HTTP Client**: `HttpClient` nativo
- ✅ **Logging**: `ILogger<T>` nativo
- ✅ **Validação**: Fluentvalidation (opcional)
- ✅ **Testes**: xUnit + FluentAssertions + Moq

### O que EVITAR:
- ❌ Repository Pattern genérico
- ❌ AutoMapper
- ❌ MediatR (não é necessário em projeto pequeno)
- ❌ Service Locator Pattern

## 🎭 9. Injeção de Dependência (DI)

### Registro no Program.cs
```csharp
builder.Services.AddScoped<IStravaRepository, StravaRepository>();
builder.Services.AddScoped<IImportActivityHandler, ImportActivityHandler>();
builder.Services.AddScoped<SprintUpDbContext>();
```

### Regra de Ouro:
- ✅ Dependa de **Interfaces**
- ✅ Registre Implementações no DI
- ✅ Use Constructor Injection

## 📝 10. Commits Git (Automático via Aider)

Quando o Aider executar com sucesso:
```
feat: implement strava activity import
fix: correct pace calculation formula
test: add validation tests for activities
```

---

**Essas convenções são a "Constituição" do seu projeto. A IA as respeitará à risca.**