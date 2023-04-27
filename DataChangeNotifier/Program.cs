using DataChangeNotifier;
using DataChangeNotifier.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<DatabaseListener>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapBlazorHub();
app.MapHub<DataChangeNotifierHub>("/hub");
app.MapFallbackToPage("/_Host");

app.Run();
