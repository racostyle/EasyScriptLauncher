using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EasyScriptLauncher
{
    internal class Program
    {
        static readonly string SETTINGS_FILE = "Easy_Script_Launcher_Settings.json";

        static void Main(string[] args)
        {
            try
            {
                var config = LoadSettings();
                if (!Directory.Exists(config.ScriptsFolder))
                {
                    Console.WriteLine($"Please state the valid path to ps scripts!! {Environment.NewLine}Settings location: {Path.Combine(Directory.GetCurrentDirectory(), SETTINGS_FILE)}");
                    Environment.Exit(1);
                }

                var files = Directory.GetFiles(config.ScriptsFolder).Where(x => x.EndsWith(".ps1"));

                foreach (var file in files)
                {
                    StartProcess(file, config);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while starting scripts: {ex.Message}");
                Environment.Exit(1);
            }
            Console.WriteLine($"OK!");
        }

        private static Config LoadSettings()
        {
            Config config;
            if (File.Exists(SETTINGS_FILE))
            {
                var content = File.ReadAllText(SETTINGS_FILE);

                try
                {
                    config = JsonSerializer.Deserialize<Config>(content);
                    return config;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while trying to read a file {SETTINGS_FILE}. Error: {ex.Message}");
                }
            }

            config = new Config
            {
                ScriptsFolder = "A:\\path\\to\\script",
                RunInSameWindow = false,
                HideWindow = false
            };
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText(SETTINGS_FILE, json);

            return config;
        }

        private static void StartProcess(string pathToScript, Config config)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{pathToScript}\"",
                Verb = "RunAs",
                UseShellExecute = !config.RunInSameWindow,
                CreateNoWindow = config.HideWindow
            };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }

        public class Config
        {
            public string ScriptsFolder { get; set; }
            public bool SearchForScriptsRecursively { get; set; }
            public bool RunInSameWindow { get; set; }
            public bool HideWindow { get; set; }
        }
    }
}
