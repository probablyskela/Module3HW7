using System;

namespace Module3HW7.Logger;

public class Log
{
    private readonly DateTime _dateTime;
    private readonly LogType _logType;
    private readonly string _message;

    public Log(string message, LogType logType)
    {
        _dateTime = DateTime.Now;
        _logType = logType;
        _message = message;
    }

    public new string ToString()
    {
        return $"{_dateTime}: {_logType}: {_message}";
    }
}