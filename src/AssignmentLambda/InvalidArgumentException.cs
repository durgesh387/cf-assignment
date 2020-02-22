using System;
using System.Runtime.Serialization;

namespace AssignmentLambda
{
    [Serializable]
    internal class InvalidArgumentException : Exception
    {

        public InvalidArgumentException()
        {
        }

        public InvalidArgumentException(string message) : base(message)
        {
        }

        public InvalidArgumentException(string argumentName, string argumentValue) : base(GetMessage(argumentName, argumentValue))
        {
        }

        private static string GetMessage(string argumentName, string argumentValue)
        {
            return $"Provided argument value : {argumentValue} for argument : {argumentName} is invalid.";
        }
    }
}