namespace DataChangeNotifier.Hubs;

public interface INotifyOnDataChanged
{
    Task NotifyOnDataChanged(string payload);
}
