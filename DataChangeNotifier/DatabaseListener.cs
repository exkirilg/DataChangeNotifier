using DataChangeNotifier.Hubs;
using Microsoft.AspNetCore.SignalR;
using Npgsql;

namespace DataChangeNotifier;

public class DatabaseListener : BackgroundService
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseListener> _logger;
    private readonly IHubContext<DataChangeNotifierHub, INotifyOnDataChanged> _hub;

    public DatabaseListener(
        IConfiguration config, ILogger<DatabaseListener> logger,
        IHubContext<DataChangeNotifierHub, INotifyOnDataChanged> hub)
    {
        _logger = logger;
        _connectionString = config.GetSection("ConnectionStrings:Database").Value!;
        _hub = hub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database listener started");

        using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync(stoppingToken);
        connection.Notification += async (o, e) => await OnDataChanged(e.Payload);

        var listenCommand = new NpgsqlCommand("listen datachange;", connection);
        await listenCommand.ExecuteNonQueryAsync(stoppingToken);

        while (stoppingToken.IsCancellationRequested == false)
        {
            await connection.WaitAsync(stoppingToken);
        }
    }

    private async Task OnDataChanged(string payload)
    {
        _logger.LogInformation("Received notification: " + payload);
        await _hub.Clients.All.NotifyOnDataChanged(payload);
    }
}