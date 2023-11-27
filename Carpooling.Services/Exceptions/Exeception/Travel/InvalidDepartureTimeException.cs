using System;

namespace Carpooling.Services.Exceptions.Exeception.User
{
    public class InvalidDepartureTimeException : ApplicationException
    {
        public InvalidDepartureTimeException(string message)
        : base(message)
        {

        }
    }
}