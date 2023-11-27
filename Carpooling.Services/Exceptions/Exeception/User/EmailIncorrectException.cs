using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class EmailIncorrectException : ApplicationException
    {
        public EmailIncorrectException(string message)
            : base(message)
        {

        }
    }
}