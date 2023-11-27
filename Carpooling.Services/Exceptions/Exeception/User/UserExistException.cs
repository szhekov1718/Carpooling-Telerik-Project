using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class UserExistException : ApplicationException
    {
        public UserExistException(string message)
            : base(message)
        {

        }
    }
}