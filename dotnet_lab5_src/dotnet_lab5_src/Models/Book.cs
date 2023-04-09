using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dotnet_lab5_src.Models;

public class Book : IEquatable<Book>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }

    [JsonConstructor]
    public Book(int id, string name, string author)
    {
        Id = id;
        Name = name;
        Author = author;
    }

    public bool Equals(Book? other)
    {
        if(ReferenceEquals(null, other)) return false;

        if (ReferenceEquals(this, other)) return true;

        return this.Id == other.Id;
    }
}
