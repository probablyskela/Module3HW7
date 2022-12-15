using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module3HW7.Logger;

namespace Module3HW7;

public class Starter
{
    public Starter()
    {
        Logger.Logger.BackUpEvent += NotifyBackUp;
    }

    public void Run(int taskCount, int iterations)
    {
        List<Task> tasks = new();
        for (var i = 0; i < taskCount; i++)
        {
            tasks.Add(Task.Run(() => { Runner(iterations); }));
        }

        Task.WaitAll(tasks.ToArray());
    }

    private static void Runner(int iterations)
    {
        var rand = new Random();
        for (var i = 0; i < iterations; i++)
        {
            var log = (rand.Next() % 3) switch
            {
                0 => new Log("Action completed successfully.", LogType.Info),
                1 => new Log("Action completed with errors.", LogType.Warning),
                2 => new Log("Action wasn't completed successfully.", LogType.Error),
                _ => throw new ArgumentOutOfRangeException()
            };

            Logger.Logger.Log(log);
        }
    }

    private static void NotifyBackUp()
    {
        Console.WriteLine("Back up process has been started");
    }
}