using EncoderServer.Abstractions;
using EncoderServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConvertionService, ConvertionService>();
builder.Services.AddMvc();

var app = builder.Build();

app.MapGet("/", () => "Converter is running");

app.MapControllers();
app.Run();
