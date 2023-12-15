using ConfigApi;
using Microsoft.AspNetCore.Mvc.Formatters;

string command = @"
                    CREATE TABLE IF NOT EXISTS storage (
                        key TEXT NOT NULL,
                        value TEXT NOT NULL,
                        UNIQUE(key) ON CONFLICT REPLACE
                    );
                ";
DatabaseUtility.ExecuteCommand(command);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options => options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();