using Microsoft.EntityFrameworkCore;
using RacePodBackend.Data;
using RacePodBackend.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? connectionString = builder.Configuration.GetConnectionString("connectionString") ?? throw new InvalidOperationException("Connection String 'connectionString' Not Found");

builder.Services.AddScoped<FeedReader>();
builder.Services.AddTransient<IDataServices, DataServices>();

builder.Services.AddDbContext<ApplicationDbContext>(opions =>
{
    opions.UseSqlite(connectionString);
    opions.EnableDetailedErrors();
    opions.EnableSensitiveDataLogging();
});

builder.WebHost.UseUrls("http://0.0.0.0.:5050");
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure the HTTP request pipeline.

var networkPolicy = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(networkPolicy, policy =>
    {
        policy
        //.AllowAnyOrigin()
        .WithOrigins("http://localhost", "http://192.168.0.0/0")
        .AllowAnyHeader()
        .AllowAnyMethod()
        //.AllowAnyHeader();
        ;
    });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseCors(networkPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
