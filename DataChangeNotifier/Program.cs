using DataChangeNotifier;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<DatabaseListener>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
