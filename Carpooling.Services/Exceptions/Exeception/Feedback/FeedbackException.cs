using System;

namespace Carpooling.Services.Exceptions.Exeception.User
{
    public class FeedbackException : ApplicationException
    {

        public FeedbackException(string message)
            : base(message)
        {

        }
    }
}