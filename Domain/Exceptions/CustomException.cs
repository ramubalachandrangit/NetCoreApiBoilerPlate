using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class CustomException : Exception
    {
        public Object ErrorData { get; set; }

        public CustomException(string message, object errorData) : base(message)
        {
            this.ErrorData = errorData;
        }
    }
}
