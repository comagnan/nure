using System;

namespace Nure.Repository.Exceptions
{
    public class InvalidDirectoryException : Exception
    {
        public InvalidDirectoryException(string p_Message) : base(p_Message)
        {
        }
    }
}
