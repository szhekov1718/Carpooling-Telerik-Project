using System;

namespace Carpooling.Services.Exceptions.Exeception.User
{
    public class TravelDoesNotExistException : ApplicationException
    {
        public TravelDoesNotExistException(string message)
        : base(message)
        {

        }
    }
}