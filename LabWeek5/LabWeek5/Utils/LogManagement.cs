using System;
using System.Globalization;
using System.IO;

namespace LabWeek5.Utils
{
    public class LogManagement
    {
        private readonly string _logFilePath;

        public LogManagement()
        {
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "log.txt");
            EnsureLogDirectoryExists();
            EnsureLogFileExists();
        }

        private void EnsureLogDirectoryExists()
        {
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        private void EnsureLogFileExists()
        {
            if (!File.Exists(_logFilePath))
            {
                File.Create(_logFilePath).Close();
            }
        }
        
        public void AddLog(string errorMessage)
        {
            using (var writer = File.AppendText(_logFilePath))
            {
                writer.WriteLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] - {errorMessage}");
            }
        }
    }
}