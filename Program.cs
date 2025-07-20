using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using store_api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbItem>(options =>

    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=store.db"));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
    policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Store API", 
        Version = "v1" 
    });
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001;
});

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseCors("AllowAll");
// Comment out HTTPS redirection for now
// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers().RequireCors("AllowAll");

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
