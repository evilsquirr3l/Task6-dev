using System;

namespace Business.Validation
{
    public class ValidationException : Exception
    {
        private static readonly string DefaultMessage = "Validation exception was thrown.";

        public ValidationException() : base(DefaultMessage)
        {
            
        }

        public ValidationException(string message) : base(message)
        {
            
        }
    }
}