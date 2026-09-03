using FactoryFlow.Api.Data;
using FactoryFlow.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 如果有人需要 IMachineService, 提供 MachineService
builder.Services.AddScoped<IMachineService, MachineService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
// 如果目前執行環境是 Development，就開放 OpenAPI endpoint
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// 如果收到 HTTP request，盡量重新導向 HTTPS
app.UseHttpsRedirection();

// 把 Authorization Middleware 加進 request pipeline
app.UseAuthorization();

// 去尋找 Controller 裡定義的 route，並把 HTTP request 導過去
app.MapControllers();

// 啟動 Web Server，開始等待 HTTP request
app.Run();
