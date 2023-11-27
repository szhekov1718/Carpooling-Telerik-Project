using System;

namespace Carpooling.Services.Exceptions.Exeception.User
{
    public class TravelPassengerDoesNotExistException : ApplicationException
    {
        public TravelPassengerDoesNotExistException(string message)
        : base(message)
        {

        }
    }
}