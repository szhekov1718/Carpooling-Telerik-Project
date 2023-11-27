using System;

namespace Carpooling.Services.Exceptions.Exeception.User
{
    public class EmailExistsException : ApplicationException
    {
        public EmailExistsException(string message)
            : base(message)
        {

        }
    }
}
