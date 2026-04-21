using Microsoft.EntityFrameworkCore;
using Good_Hamburger.Repository;
using Good_Hamburger.Services;
using Good_Hamburger.Services.Discounts;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configuração do PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Injeção de Dependência - Repositórios
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Injeção de Dependência - Motor de Regras de Desconto
builder.Services.AddScoped<IDiscountRule, ComboCompletoRule>();
builder.Services.AddScoped<IDiscountRule, SandwichDrinkRule>();
builder.Services.AddScoped<IDiscountRule, SandwichSideRule>();

// Injeção de Dependência - Serviços
builder.Services.AddScoped<IOrderService, OrderService>();

// SEGURANÇA: Rate Limiting (Proteção contra brute force/vassouragem)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 30; // 30 requisições
        options.Window = TimeSpan.FromMinutes(1); // por minuto
        options.QueueLimit = 2;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // Retorno customizado para quando houver block
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// CORS: Permitir apenas o endereço do Web (Melhora a segurança)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        // No Docker, o frontend roda no 8080 e conversa com 5000.
        // Permitimos o localhost:8080 para desenvolvimento também.
        policy.WithOrigins("http://localhost:8080", "http://localhost")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Garantir que o banco de dados seja criado e os dados semeados
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica o Rate Limiter globalmente ou pode ser por controlador
app.UseRateLimiter();

app.UseCors("AllowBlazor");

app.UseHttpsRedirection();
app.UseAuthorization();

// Aplica a política de rate limit para todos os controllers da API
app.MapControllers().RequireRateLimiting("fixed");

app.Run();
