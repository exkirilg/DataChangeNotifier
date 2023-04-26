using DataChangeNotifier;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<DatabaseListener>();

var app = builder.Build();

app.MapGet("hello", () => "Hello world!");

app.Run();
