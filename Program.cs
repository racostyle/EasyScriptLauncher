using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace EasyScriptLauncher
{
    internal class Program
    {
        static readonly string SETTINGS_FILE = "Easy_Script_Launcher_Settings.json";

        static void Main(string[] args)
        {
            try
            {
                var config = new Settings().LoadSettings(SETTINGS_FILE);
                if (!Directory.Exists(config.ScriptsFolder))
                {
                    Info.FillTheSettings(Path.Combine(Directory.GetCurrentDirectory(), SETTINGS_FILE));
                    Environment.Exit(1);
                }

                var files = Directory.GetFiles(config.ScriptsFolder, "*.ps1", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    Console.WriteLine($"Starting {file}");
                    StartProcess(file, config);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Info.GenericError(ex.Message));
                Environment.Exit(1);
            }
            Console.WriteLine(Info.Done());
        }

        private static void StartProcess(string pathToScript, Config config)
        {
            bool useShellToExecute = !config.RunInSameWindow;

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{pathToScript}\"",
                Verb = "RunAs",
                UseShellExecute = useShellToExecute,
                CreateNoWindow = config.HideWindow,
                RedirectStandardError = !useShellToExecute,
                RedirectStandardOutput = !useShellToExecute
            };

            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Thread.Sleep(2000);
            if (!process.HasExited)
                Console.WriteLine(Info.ScriptStartedSuccessfully(pathToScript));
            else      
                Console.WriteLine(Info.ScriptLoadingFailed());
        }
    }
}
