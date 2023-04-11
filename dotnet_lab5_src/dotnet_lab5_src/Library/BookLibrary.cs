using dotnet_lab5_src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_lab5_src.RandomGenerators;
using System.Collections.ObjectModel;
using dotnet_lab5_src.Loggers.Abstract;
using System.Windows.Data;

namespace dotnet_lab5_src.Library;

public class BookLibrary
{
    private const int OrderDurationMiliseconds = 10000;

    private ILogger<string> _notificationsLogger;
    private ILogger<BookLibrary> _libraryLogger;

    private object _lockObject;
    private bool _isAcceptingClients;

    public bool IsAcceptingClients
    {
        get => _isAcceptingClients;
        set
        {
            lock(_lockObject)
            {
                _isAcceptingClients = value;
                if (!_isAcceptingClients) _notificationsLogger.Log("library stopped accepting clients");
                else
                {
                    StartGeneratingClients();
                    _notificationsLogger.Log("library started accepting clients");
                }
            }
        }
    }

    public ObservableCollection<BookShelf> BooksShelves { get; }
    public ObservableCollection<Order> Orders { get; }

    public BookLibrary(ILogger<string> notificationsLogger, ILogger<BookLibrary> libraryLogger)
    {
        _libraryLogger = libraryLogger;
        _notificationsLogger = notificationsLogger;
        _lockObject = new object();
        IsAcceptingClients = true;

        Orders = new ObservableCollection<Order>();
        BooksShelves = new ObservableCollection<BookShelf>();
        EnableCollectionsSyncronisation();
    }

    public BookLibrary(ILogger<string> notificationsLogger, ILogger<BookLibrary> libraryLogger,
        IEnumerable<BookShelf> booksInLibrary)
        : this(notificationsLogger, libraryLogger)
    {
        BooksShelves = new ObservableCollection<BookShelf>(booksInLibrary);
        EnableCollectionsSyncronisation();
    }

    public BookLibrary(ILogger<string> notificationsLogger, ILogger<BookLibrary> libraryLogger,
        IEnumerable<BookShelf> booksInLibrary, IEnumerable<Order> orders) 
        : this(notificationsLogger,libraryLogger,booksInLibrary)
    {
        Orders = new ObservableCollection<Order>(orders);
        EnableCollectionsSyncronisation();
        foreach (var order in Orders)
        {
            Task.Run(() => FinishOrder(order));
        }
    }

    private void EnableCollectionsSyncronisation()
    {
        BindingOperations.EnableCollectionSynchronization(Orders, _lockObject);
        BindingOperations.EnableCollectionSynchronization(BooksShelves, _lockObject);
    }

    public async Task<bool> TryMakeOrder(string clientName,Book book)
    {
        if (BooksShelves.Count == 0) return false;

        if (!BooksShelves.Any(b => b.Book.Equals(book))) return false;

        lock (_lockObject)
        {
            var bookShelf = BooksShelves.First(b => b.Book.Equals(book));
            if (bookShelf.Amount == 0) return false;

            int id = Orders.Count == 0 ? 1 : Orders.MaxBy(o => o.Id)!.Id + 1;
            var order = new Order(id, clientName, bookShelf.Book);

            Orders.Add(order);

            Task.Run(() => FinishOrder(order));
            bookShelf.Amount--;

            _notificationsLogger.Log($"{order.Client} ordered \"{order.Book.Name}\"");
        }

        return true;
    }

    private async Task FinishOrder(Order order)
    {
        await Task.Delay(OrderDurationMiliseconds);
        
        lock(_lockObject)
        {
            if (!BooksShelves.Any(b => b.Book.Equals(order.Book))) return;
            var bookShelf = BooksShelves.First(b => b.Book.Equals(order.Book));
            bookShelf.Amount++;

            var orderToRemove = Orders.First(o => o.Equals(order));
            Orders.Remove(orderToRemove);

            _notificationsLogger.Log($"{order.Client} returned \"{order.Book.Name}\"");
        }
    }

    public async Task StartGeneratingClients()
    {
        while (IsAcceptingClients)
        {
            await Task.Delay(2000);

            var clients = RandomClientGenerator.GetRandomClients();

            foreach (var client in clients)
            {
                if(!IsAcceptingClients)
                {
                    return;
                }
                var availableBooks = GetAvailableBooks();

                if (availableBooks.Any())
                {
                    var book = availableBooks[Random.Shared.Next(availableBooks.Count())].Book;

                    await TryMakeOrder(client, book);
                }                
            }
        }
    }

    private BookShelf[] GetAvailableBooks()
    {
        var availableBooks = BooksShelves.Where(b => b.Amount > 0).ToArray();
        if (!availableBooks.Any() && IsAcceptingClients) 
        {
            IsAcceptingClients = false;
            RequestAddingBooks(); 
        }

        return availableBooks;
    }

    private async Task RequestAddingBooks()
    {
        lock (_lockObject)
        {
            _notificationsLogger.Log("books delivery requested");
        }
        await Task.Delay(10000);

        foreach (var bookShelf in BooksShelves)
        {
            lock (_lockObject)
            {
                bookShelf.Amount += 5;
            }
        }
        lock (_lockObject)
        {
            _notificationsLogger.Log("books delivered");
        }
        IsAcceptingClients = true;
    }

    public async Task StartLogging()
    {
        while (true)
        {
            await Task.Delay(15000);

            _libraryLogger.Log(this);
        }
    }
}