using System;

namespace Nure.RepositoryManager.Exceptions
{
    public class InvalidDirectoryException : Exception
    {
        public InvalidDirectoryException(string p_Message) : base(p_Message)
        {
        }
    }
}
