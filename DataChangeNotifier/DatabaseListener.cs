using Npgsql;

namespace DataChangeNotifier;

public class DatabaseListener : BackgroundService
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseListener> _logger;
    
    public DatabaseListener(IConfiguration config, ILogger<DatabaseListener> logger)
    {
        _logger = logger;
        _connectionString = config.GetSection("ConnectionStrings:Database").Value!;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database listener started");

        using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync(stoppingToken);
        connection.Notification += (o, e) => _logger.LogInformation("Received notification: " + e.Payload);

        var listenCommand = new NpgsqlCommand("listen datachange;", connection);
        await listenCommand.ExecuteNonQueryAsync(stoppingToken);

        while (stoppingToken.IsCancellationRequested == false)
        {
            await connection.WaitAsync(stoppingToken);
        }
    }
}