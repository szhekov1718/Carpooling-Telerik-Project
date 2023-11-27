using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class PasswordIncorrectException : ApplicationException
    {
        public PasswordIncorrectException(string message)
            : base(message)
        {

        }
    }
}