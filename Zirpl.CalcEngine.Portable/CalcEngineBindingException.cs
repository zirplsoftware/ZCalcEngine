using System;

namespace Zirpl.CalcEngine
{
    public class CalcEngineBindingException : ArgumentException
    {
        public CalcEngineBindingException(string message) : base(message)
        {
        }

        public CalcEngineBindingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CalcEngineBindingException(string message, string paramName) : base(message, paramName)
        {
        }

        public CalcEngineBindingException(string message, object previousObj, object initialObj) : base(message)
        {
            Previous = previousObj;
            Initial = initialObj;
        }

        public object Initial { get; set; }

        public object Previous { get; set; }
    }
}