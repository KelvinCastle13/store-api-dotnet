using Microsoft.EntityFrameworkCore;
using store_api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbItem>(options =>

    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=store.db"));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", 
    policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:4200", "http://localhost:5173")
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var appSettingsDevPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
    if (app.Environment.IsDevelopment())
    {
        var devSettings = @"{
            ""ConnectionStrings"": {
                ""DefaultConnection"": ""Data Source=store.db""
            },
            ""Logging"": {
                ""LogLevel"": {
                    ""Default"": ""Information"",
                    ""Microsoft.AspNetCore"": ""Warning""
            }
         }
        }";
        File.WriteAllText(appSettingsDevPath, devSettings);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers().RequireCors("AllowFrontend");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbItem>();
    context.Database.EnsureCreated();
    if (app.Environment.IsDevelopment())
    {
        DbSeeder.SeedData(context);
    }
}
app.Run();