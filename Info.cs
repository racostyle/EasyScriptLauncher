using System;
using System.Diagnostics.CodeAnalysis;

namespace EasyScriptLauncher
{
    internal static class Info
    {
        internal static string ScriptLoadingFailed(string script, string message)
        {
            return $"Error occured while starting script: {script}{Environment.NewLine}Error: {message}{Environment.NewLine}";
        }
        internal static string ScriptLoadingFailed()
        {
            return $"Error occured while starting script!{Environment.NewLine}";
        }

        internal static string FillTheSettings(string path)
        {
            return $"Please state the valid path to ps scripts!!{Environment.NewLine}Settings location: {path}{Environment.NewLine}";
        }

        internal static string ScriptStartedSuccessfully(string script)
        {
            return $"Script {script} started successfully{Environment.NewLine}";
        }

        internal static string GenericError(string message)
        {
            return $"An Error occured {message}{Environment.NewLine}";
        }

        internal static string Done()
        {
            return "Completed";
        }
    }
}
