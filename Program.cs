using System.Text.Json.Serialization;
using BlogApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions( options => {
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddDbContext<BlogApiContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
