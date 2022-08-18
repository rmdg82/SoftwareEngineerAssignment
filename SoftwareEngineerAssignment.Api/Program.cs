using SoftwareEngineerAssignment.Api.Constants;
using SoftwareEngineerAssignment.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IAdviceSlipService, AdviceSlipService>(client =>
{
    client.BaseAddress = new Uri(RouteConstants.AdviceSlipServiceBaseUrl);
});
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddMemoryCache();

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