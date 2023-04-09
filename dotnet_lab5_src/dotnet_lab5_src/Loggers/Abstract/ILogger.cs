using dotnet_lab5_src.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet_lab5_src.Loggers.Abstract;

public interface ILogger<T>
{
    public void Log(T library);
}
