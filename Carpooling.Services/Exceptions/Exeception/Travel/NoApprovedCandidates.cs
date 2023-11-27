using System;

namespace Carpooling.Services.Exceptions.Exeception.User
{
    public class NoApprovedCandidates : ApplicationException
    {
        public NoApprovedCandidates(string message)
        : base(message)
        {

        }
    }
}