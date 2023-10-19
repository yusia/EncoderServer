using Encoder.ConversionService;
using Encoder.ConversionService.Abstraction;
using EncoderServer.Abstraction;
using EncoderServer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConvertionService, ConvertionService>();
builder.Services.AddSingleton<ITokenStorage, CancellationTokenStorage>();

builder.Services.AddMvc();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
        builder =>
        {
            builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();

        });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.MapGet("/", () => "Converter is running");

app.MapHub<SignalRHub>("/ws");
app.UseWebSockets();
app.UseCors("AllowAllHeaders");
app.MapControllers();
app.Run();
