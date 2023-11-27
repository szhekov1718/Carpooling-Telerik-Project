using System;

namespace Carpooling.Services.Exceptions.Exeception.User

{
    public class EntityNotFound : ApplicationException
    {
        public EntityNotFound(string message)
            : base(message)
        {

        }
    }
}