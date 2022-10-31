using Microsoft.Data.Sqlite;
using TicketingSolution.Persistence;
using Microsoft.EntityFrameworkCore;
using TicketingSolution.Core.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = "DataSource=:memory";
var conn = new SqliteConnection(connectionString);
conn.Open();

builder.Services.AddDbContext<TicketingSolutionDbContext>(opt => opt.UseSqlite(conn));
builder.Services.AddScoped<ITicketBookingRequestHandler, TicketBookingRequestHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
