// Copyright (c) 2005-2020, Coveo Solutions Inc.

using System;

namespace Nure.RepositoryManager.Exceptions
{
    public class InvalidRepositoryException : Exception
    {
        public InvalidRepositoryException(string p_Message) : base(p_Message)
        {
        }
    }
}
