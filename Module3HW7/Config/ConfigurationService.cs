using System;
using System.IO;
using Newtonsoft.Json;

namespace Module3HW7.Config;

public static class ConfigurationService
{
    private static readonly string ConfigurationPath = "../../../appsettings.json";

    public static Configuration Configuration
    {
        get
        {
            var json = File.ReadAllText(ConfigurationPath);
            var configuration = JsonConvert.DeserializeObject<Configuration>(json);
            if (configuration is null)
            {
                throw new NullReferenceException();
            }

            return FixConfiguration(configuration);
        }
    }

    private static Configuration FixConfiguration(Configuration configuration)
    {
        var fix = false;
        if (configuration.BackUpInterval < 1)
        {
            configuration.BackUpInterval = 10;
            fix = true;
        }

        if (configuration.BackUpPath is not null && configuration.BackUpPath[^1] != '/')
        {
            configuration.BackUpPath += '/';
            fix = true;
        }

        if (configuration.BackUpPath is null ||
            !Directory.Exists(GetParentDirectory(configuration.BackUpPath)))
        {
            configuration.BackUpPath = "../../../Logs/";
            fix = true;
        }

        if (fix)
        {
            var output = JsonConvert.SerializeObject(configuration);
            File.WriteAllText(ConfigurationPath, output);
        }

        return configuration;
    }

    private static string GetParentDirectory(string? path)
    {
        if (path is not null && path[^1] == '/')
        {
            path = Directory.GetParent(path)?.FullName;
        }

        if (path is not null)
        {
            path = Directory.GetParent(path)?.FullName;
        }

        path ??= "../../../";
        return path;
    }
}