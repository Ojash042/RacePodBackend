using Microsoft.EntityFrameworkCore;
using RacePodBackend.Data;
using RacePodBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? connectionString = builder.Configuration.GetConnectionString("connectionString") ?? throw new InvalidOperationException("Connection String 'connectionString' Not Found");
builder.Services.AddScoped<FeedReader>();
builder.Services.AddDbContext<ApplicationDbContext>(opions => {
	opions.UseSqlite(connectionString);
	opions.EnableDetailedErrors();
	opions.EnableSensitiveDataLogging();
});
builder.WebHost.UseUrls("http://0.0.0.0.:5050");
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI(c => {
		c.DisplayRequestDuration();
	});
}

app.UseHttpsRedirection();

app.UseCors(builder=> 
		builder
		.WithOrigins("http://localhost:5173")
		.AllowAnyMethod()
		.AllowAnyHeader()
		);
app.UseAuthorization();

app.MapControllers();

app.Run();
