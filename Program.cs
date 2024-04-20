using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.Mapper;
using Reddit.Repositories;
using System.Text.Json.Serialization;
using Reddit.Filters;
using Reddit.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddControllers(options => options.Filters.Add<ModelValidationActionFilter>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDb"));
    options.UseLazyLoadingProxies();
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
               builder => builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader());
});
builder.Services.AddSingleton<IMapper, Mapper>();
builder.Services.AddScoped<ICommunitiesRepository, CommunitiesRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "application/json");
    await next();
});


app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapGet("/throws", (context) => throw new Exception("Exception my bad"));



app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
