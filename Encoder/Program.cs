using Encoder.ConversionService;
using Encoder.ConversionService.Abstraction;
using Encoder.ConversionService.Settings;
using EncoderServer.Abstraction;
using EncoderServer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IConvertionService, ConvertionService>();
builder.Services.AddSingleton<ITokenStorage, CancellationTokenStorage>();
builder.Services.AddMvc();

var origin = builder.Configuration.GetSection("AllowedOrigin").Value;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllHeaders",
        builder =>
        {
            builder
            .WithOrigins(origin)
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();

        });
});
builder.Services.AddSignalR();
builder.Services.Configure<DelaySettings>(builder.Configuration.GetSection("DelaySettings"));

var app = builder.Build();
app.MapGet("/", () => "Converter is running");
app.MapHub<SignalRHub>("/ws");
app.UseWebSockets();
app.UseCors("AllowAllHeaders");
app.MapControllers();
app.Run();
