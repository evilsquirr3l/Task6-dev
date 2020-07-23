using System;

namespace Business.Validation
{
    public class LibraryException : Exception
    {
        private static readonly string DefaultMessage = "Library exception was thrown.";

        public LibraryException() : base(DefaultMessage)
        {
            
        }

        public LibraryException(string message) : base(message)
        {
            
        }
    }
}