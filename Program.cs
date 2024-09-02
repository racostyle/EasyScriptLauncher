using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace EasyScriptLauncher
{
    internal class Program
    {
        static readonly string SETTINGS_FILE = "EasyScriptLauncher_Settings.json";
        private static Info _info;

        static void Main(string[] args)
        {
            _info = new Info(new Logger());

            try
            {
                var config = new Settings().LoadSettings(SETTINGS_FILE);
                if (!Directory.Exists(config.ScriptsFolder))
                {
                    _info.FillTheSettings(Path.Combine(Directory.GetCurrentDirectory(), SETTINGS_FILE));
                    Environment.Exit(1);
                }

                string[] scripts;
                if (config.SearchForScriptsRecursively)
                    scripts = Directory.GetFiles(config.ScriptsFolder, "*.ps1", SearchOption.AllDirectories);
                else
                    scripts = Directory.GetFiles(config.ScriptsFolder, "*.ps1", SearchOption.TopDirectoryOnly);

                if (scripts.Length == 0)
                {
                    _info.NoScriptsFound(config.ScriptsFolder, config.SearchForScriptsRecursively);
                    Environment.Exit(1);
                }

                foreach (var script in scripts)
                {
                    _info.StartingScript(script);
                    StartProcess(script, config);
                }
            }
            catch (Exception ex)
            {
                _info.GenericError(ex.Message);
                Environment.Exit(1);
            }
            _info.Done();
        }

        private static void StartProcess(string pathToScript, Config config)
        {
            bool useShellToExecute = !config.RunInSameWindow;

            string argument;
            if (config.TestBehaviour)
                argument = "-ExecutionPolicy Bypass -Command \"exit 0\"";
            else
                argument = $"-ExecutionPolicy Bypass -File \"{pathToScript}\"";
            
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = argument,
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
                _info.ScriptStartedSuccessfully(pathToScript);
            else
            {
                if (process.ExitCode == 0)
                    _info.ScriptCompleted();
                else
                    _info.ScriptLoadingFailed();
            }
                
        }
    }
}
