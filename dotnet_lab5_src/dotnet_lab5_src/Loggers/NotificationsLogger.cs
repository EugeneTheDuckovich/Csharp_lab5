using dotnet_lab5_src.Loggers.Abstract;
using dotnet_lab5_src.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_lab5_src.Loggers;

public class NotificationsLogger : NotifyPropertyChanged, ILogger<string>
{
    private string _notifications;
    public string Notifications 
    {
        get 
        {
            return _notifications;
        } 
        private set
        {
            _notifications = value;
            OnPropertyChanged(nameof(Notifications));
        } 
    }

    public NotificationsLogger()
    {
        Notifications = string.Empty;
    }

    public void Log(string notification)
    {
        Notifications = Notifications + notification.Trim('\n') + '\n';
        if (Notifications.Where(c => c == '\n').Count() > 50)
        {
            Notifications = Notifications.Remove(0, Notifications.IndexOf('\n') + 1);
        }
    }
}