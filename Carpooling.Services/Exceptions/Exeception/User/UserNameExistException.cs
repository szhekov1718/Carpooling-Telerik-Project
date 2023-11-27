using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class UserNameExistException : ApplicationException
    {
        public UserNameExistException(string message)
            : base(message)
        {

        }
    }
}