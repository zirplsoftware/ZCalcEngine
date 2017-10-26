using System;
using System.Collections.Generic;

namespace Zirpl.CalcEngine
{
    public class CalcEngineBindingException : ArgumentException
    {
        private readonly List<BindingInfo> _bindingPath;
        public string FullPath { get; }
        public Type Type { get; }

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

        internal CalcEngineBindingException(string message, object previousObj, object initialObj, string fullPath, Type type, List<BindingInfo> bindingPath) : this(message, previousObj, initialObj)
        {
            _bindingPath = bindingPath;
            FullPath = fullPath;
            Type = type;
        }

        public object Initial { get; set; }

        public object Previous { get; set; }
    }
}