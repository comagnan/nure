using System;

namespace Nure.Repository.Exceptions
{
    public class InvalidRepositoryException : Exception
    {
        public InvalidRepositoryException(string p_Message) : base(p_Message)
        {
        }
    }
}
