using System;

namespace dotnet_lab5_src.Models;

public class Order : IEquatable<Order>
{
    public int Id { get; set; }
    public string Client { get; set; }
    public Book Book { get; set; }

    public Order(int id, string client, Book book)
    {
        Id = id;
        Client = client;
        Book = book;
    }

    public bool Equals(Order? other)
    {
        if (ReferenceEquals(null, other)) return false;

        if (ReferenceEquals(this, other)) return true;

        return this.Id == other.Id;
    }

    public override string ToString()
    {
        return $"{Client}, \"{Book.Name}\"";
    }
}