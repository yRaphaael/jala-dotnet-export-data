using System.Diagnostics;

public class ObserverMessage : IObservable
{
    public void UpdateMessage(string message)
    {
        Process process = new();
        process.StartInfo.FileName = "notify-send";
        process.StartInfo.Arguments = $"\"Notification\" \"{message}\"";
        
        process.Start();
        
        process.WaitForExit();
    }
}