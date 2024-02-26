using System;

namespace CustomizedErrors {
    public class InvalidParameterError : Exception
{
    public InvalidParameterError(string parameterName) 
        : base($"Invalid value for parameter '{parameterName}'.")
    {
        ParameterName = parameterName;
    }

    public string ParameterName { get; }
}


    public class InvalidNameError(string message) : Exception(message) {
    }
}