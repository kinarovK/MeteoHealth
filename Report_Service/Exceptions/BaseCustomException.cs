using System;
using System.Collections.Generic;
using System.Text;

namespace Report_Service.Exceptions
{
    public class BaseCustomException : Exception
    {
        public BaseCustomException(string message) : base(message) { }
    }
}
