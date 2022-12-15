using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Module3HW7.Config;

namespace Module3HW7.Logger;

public static class Logger
{
    private static readonly List<Log> Logs = new();
    private static readonly object Lock = new();
    private static int _backUpId = 1;

    public delegate void BackUpHandler();

    public static event BackUpHandler? BackUpEvent;

    public static void Log(Log log)
    {
        lock (Lock)
        {
            Logs.Add(log);

            if (Logs.Count % (_backUpId * ConfigurationService.Configuration.BackUpInterval) != 0)
            {
                return;
            }

            BackUpEvent?.Invoke();
            var logs = new Log[Logs.Count];
            Logs.CopyTo(logs);
            Logs.Clear();
            BackUpWrapper(logs, _backUpId);
        }
    }

    private static void BackUpWrapper(IEnumerable<Log> logs, int backUpId)
    {
        _backUpId++;

        Thread backUp = new(() => { BackUp(logs, backUpId); })
        {
            IsBackground = false
        };
        backUp.Start();
    }

    private static void BackUp(IEnumerable<Log> logs, int backUpId)
    {
        var backUpPath = ConfigurationService.Configuration.BackUpPath;
        if (backUpPath is null)
        {
            throw new NullReferenceException();
        }

        Directory.CreateDirectory(backUpPath);
        var output = logs.Aggregate(string.Empty, (current, log) => current + $"{log.ToString()}\n");
        var dt = DateTime.Now;
        var fileName = $"{backUpPath}{dt.Day}{dt.Month}{dt.Year}-{dt.Hour}{dt.Minute}{dt.Second}-{backUpId}.txt";
        File.WriteAllText(fileName, output);
    }
}