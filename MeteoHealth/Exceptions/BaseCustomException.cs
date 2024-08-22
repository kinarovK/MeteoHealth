using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoHealth.Exceptions
{
    public class BaseCustomException : Exception
    {
        public BaseCustomException(string message) : base() { }
    }
}
