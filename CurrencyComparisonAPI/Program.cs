using Business.Abstract;
using Business.Concrete;
using DataAccess.Dapper.Abstract;
using DataAccess.Dapper.Concrete;
using DataAccess.Dapper.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<DapperDbContext>();

builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ILogDal, LogDal>();
builder.Services.AddScoped<ILogService, LogService>();

string connectionString = builder.Configuration.GetConnectionString("SqlConnection");




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
