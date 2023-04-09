using dotnet_lab5_src.Library;
using dotnet_lab5_src.Loggers;
using dotnet_lab5_src.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace dotnet_lab5_src.ViewModels;

public class LibraryViewModel
{
    private BookLibrary _library;
    public NotificationsLogger NotificationsLogger { get; }

    public ObservableCollection<BookShelf> BookShelves
    {
        get => _library.BooksShelves;
    }

    public ObservableCollection<Order> Orders
    { 
        get => _library.Orders; 
    }

    public string Notifications
    {
        get => NotificationsLogger.Notifications;
    }

    public LibraryViewModel()
    {
        NotificationsLogger = new NotificationsLogger();

        var books = new List<BookShelf>
        {
            new BookShelf(1,"Misery","Stephen King",5),
            new BookShelf(2,"Game of Thrones","George R.R. Martin",5),
            new BookShelf(3,"Last Wish","Andjei Sapkovski",5),
            new BookShelf(4,"Brave New World","Aldous Huxley",5),
            new BookShelf(5,"1984","George Orwell",5),
        };

        _library = new BookLibrary(NotificationsLogger, new LibraryJsonLogger(),books);
        Task.Run(() => _library.StartLogging());
        Task.Run(() => _library.StartGeneratingClients());
    }
}
