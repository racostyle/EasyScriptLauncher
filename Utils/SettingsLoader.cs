﻿using System;
using System.IO;
using System.Text.Json;

namespace EasyScriptLauncher.Utils
{
    internal class SettingsLoader
    {
        internal Config LoadSettings(string settingsFile)
        {
            Config config;
            if (File.Exists(settingsFile))
            {
                var content = File.ReadAllText(settingsFile);

                try
                {
                    config = JsonSerializer.Deserialize<Config>(content);
                    return config;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while trying to read a file {settingsFile}. Error: {ex.Message}");
                }
            }

            config = new Config
            {
                ScriptsFolder = "A:\\path\\to\\script",
                RunInSameWindow = false,
                HideWindow = false,
                SearchForScriptsRecursively = false,
                TestBehaviour = false,
                LoadProfile = false,
            };
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText(settingsFile, json);

            return config;
        }
    }
}
