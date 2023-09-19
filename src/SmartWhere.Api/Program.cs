using SmartWhere.Sample.Api.ApplicationSpecific.Contexts;
using SmartWhere.Sample.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MemoryContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

PublisherData.FillDummyData();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
