using dotnet_lab5_src.Library;
using dotnet_lab5_src.Loggers.Abstract;
using dotnet_lab5_src.Memento;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace dotnet_lab5_src.Loggers;

public class LibraryJsonLogger : ILogger<BookLibrary>
{
    public void Log(BookLibrary library)
    {
        CreateFileIfDoesNotExist();

        var jsonString = JsonSerializer.Serialize<BookLibraryMemento>(new BookLibraryMemento 
        { 
            BooksInLibrary = library.BooksShelves
            .ToList(),
            Orders = library.Orders
            .Select(o => new OrderMemento { Id = o.Id,ClientName = o.Client, BookId = o.Book.Id })
            .ToList()
        });

        var fileText = File.ReadAllText("log\\log.json");

        var indexOfInsertion = fileText.LastIndexOf(']');

        if (fileText != "[]") 
        { 
            fileText = fileText.Insert(indexOfInsertion,",\n");
            indexOfInsertion = fileText.LastIndexOf(']');
        }

        fileText = fileText.Insert(indexOfInsertion, jsonString);

        File.WriteAllText("log\\log.json",fileText);
    }

    private static void CreateFileIfDoesNotExist()
    {
        if (!Directory.Exists("log\\"))
        {
            Directory.CreateDirectory("log\\");
        }

        if (!File.Exists("log\\log.json"))
        {
            File.WriteAllText("log\\log.json", "[]");
        }
    }
}