using dotnet_lab5_src.Models;
using System.Collections.Generic;

namespace dotnet_lab5_src.Memento;

public class BookLibraryMemento
{
    public List<BookShelf> BooksInLibrary { get; set; }
    public List<OrderMemento> Orders { get; set; }
}
