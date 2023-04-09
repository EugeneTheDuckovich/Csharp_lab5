using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_lab5_src.Memento;

public class OrderMemento
{
    public int Id { get; set; }
    public string ClientName { get; set; }
    public int BookId { get; set; }
}
