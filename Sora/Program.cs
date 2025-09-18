using Microsoft.AspNetCore.Mvc;
using Sora.Data;
using Sora.Services;
using Sora.Utils;

DotNetEnv.Env.Load();
_ = GlobalServices.TokenManager;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<LessonService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // default 1.0
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<SoraAuthMiddleware>();
app.MapControllers();
app.Run();