using System;
using System.Runtime.Serialization;

namespace Business.Validation
{
    [Serializable]
    public class LibraryException : Exception
    {
        private static readonly string DefaultMessage = "Library exception was thrown.";

        public LibraryException() : base(DefaultMessage)
        {
            
        }

        public LibraryException(string message) : base(message)
        {
            
        }
        
        public LibraryException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected LibraryException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}