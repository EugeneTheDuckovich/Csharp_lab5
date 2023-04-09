using dotnet_lab5_src.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_lab5_src.Models;

public class BookShelf: NotifyPropertyChanged
{
    public Book Book { get; set; }

    private int _amount;
    public int Amount 
    { 
        get => _amount; 
        set 
        {
            _amount = value;
            OnPropertyChanged(nameof(Amount));
        }
    }

    public BookShelf(int bookId, string bookName, string authorName, int amount)
    {
        Book = new Book(bookId, bookName, authorName);
        Amount = amount;
    }

    public override string ToString()
    {
        return $"name: {Book.Name}, author: {Book.Author}, amount: {Amount}";
    }
}
