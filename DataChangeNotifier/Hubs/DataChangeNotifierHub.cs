using Microsoft.AspNetCore.SignalR;

namespace DataChangeNotifier.Hubs;

public class DataChangeNotifierHub : Hub<INotifyOnDataChanged>
{
    public async Task NotifyOnDataChanged(string payload)
    {
        await Clients.All.NotifyOnDataChanged(payload);
    }
}
