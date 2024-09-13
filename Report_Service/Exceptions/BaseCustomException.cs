using System;

namespace Report_Service.Exceptions
{
    public class BaseCustomException : Exception
    {
        public BaseCustomException(string message) : base(message) { }
    }
}
