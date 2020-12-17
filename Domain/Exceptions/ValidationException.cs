using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message, object errorData): base(message, errorData)
        {

        }
    }
}
