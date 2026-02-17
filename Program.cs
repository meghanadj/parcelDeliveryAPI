using Microsoft.EntityFrameworkCore;
// DAO registration removed — Department is now an enum
using ParcelDelivery.Api.Data;
using ParcelDelivery.Api.Interfaces;
using ParcelDelivery.Api.Services;
using ParcelDelivery.Api.DAO;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// CORS: allow the local Next.js frontend during development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// parcel classifier (business rules service)
builder.Services.AddSingleton<IParcelClassifier, ParcelClassifier>();
builder.Services.AddScoped<IApprovalClassifier, ApprovalClassifier>();
// DAOs
builder.Services.AddScoped<IOrderDao, OrderDao>();
builder.Services.AddScoped<IParcelDao, ParcelDao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS for local frontend during development
app.UseCors("AllowLocal");

app.UseAuthorization();
app.MapControllers();

app.Run();