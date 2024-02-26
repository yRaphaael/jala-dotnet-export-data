using System;
using System.Collections.Generic;

public class StackError
{
    private static readonly Stack<ErrorInfo> _errorStack = new Stack<ErrorInfo>();

    public static void PushError(string errorMessage)
    {
        var errorInfo = new ErrorInfo(errorMessage, DateTime.Now);
        _errorStack.Push(errorInfo);
    }

    public static string PopError()
    {
        if (_errorStack.Count == 0)
        {
            return "No errors.";
        }
        else
        {
            var errorInfo = _errorStack.Pop();
            return $"[{errorInfo.Timestamp}] Error: {errorInfo.ErrorMessage}";
        }
    }

    private class ErrorInfo
    {
        public string ErrorMessage { get; }
        public DateTime Timestamp { get; }

        public ErrorInfo(string errorMessage, DateTime timestamp)
        {
            ErrorMessage = errorMessage;
            Timestamp = timestamp;
        }
    }
}