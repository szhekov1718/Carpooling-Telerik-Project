using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class UserNotExistException : ApplicationException
    {
        public UserNotExistException(string message)
        : base(message)
        {

        }
    }
}