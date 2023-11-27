using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class AuthorisationException : ApplicationException
    {
        public AuthorisationException(string message)
            : base(message)
        {

        }
    }
}