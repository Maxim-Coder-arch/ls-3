using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Configuration;

public static class ProcessLaunchRequestFactory
{
    public static ProcessLaunchRequest CreateFromCommandLineArguments(
        IReadOnlyList<string> commandLineArguments,
        string workingDirectory)
    {
        ArgumentNullException.ThrowIfNull(commandLineArguments);
        ArgumentException.ThrowIfNullOrWhiteSpace(workingDirectory);

        if (commandLineArguments.Count == 0)
        {
            return CreateDefaultRequest(workingDirectory);
        }

        if (IsStandardStreamsDemonstrationRequested(commandLineArguments))
        {
            return CreateStandardStreamsDemonstrationRequest(workingDirectory);
        }

        return CreateCustomRequest(commandLineArguments, workingDirectory);
    }

    private static bool IsStandardStreamsDemonstrationRequested(IReadOnlyList<string> commandLineArguments)
    {
        return commandLineArguments.Count == 1
            && string.Equals(
                commandLineArguments[0],
                LaunchScenarioNames.StandardStreamsDemonstration,
                StringComparison.OrdinalIgnoreCase);
    }

    private static ProcessLaunchRequest CreateDefaultRequest(string workingDirectory)
    {
        return new ProcessLaunchRequest(
            DefaultLaunchSettings.ExecutableFileName,
            [DefaultLaunchSettings.InformationArgument],
            workingDirectory);
    }

    private static ProcessLaunchRequest CreateStandardStreamsDemonstrationRequest(string workingDirectory)
    {
        if (OperatingSystem.IsWindows())
        {
            return new ProcessLaunchRequest(
                StandardStreamsDemonstrationSettings.WindowsCommandInterpreter,
                [
                    StandardStreamsDemonstrationSettings.WindowsExecuteAndTerminateSwitch,
                    StandardStreamsDemonstrationSettings.WindowsCommandText
                ],
                workingDirectory);
        }

        return new ProcessLaunchRequest(
            StandardStreamsDemonstrationSettings.UnixShell,
            [
                StandardStreamsDemonstrationSettings.UnixExecuteCommandSwitch,
                StandardStreamsDemonstrationSettings.UnixCommandText
            ],
            workingDirectory);
    }

    private static ProcessLaunchRequest CreateCustomRequest(
        IReadOnlyList<string> commandLineArguments,
        string workingDirectory)
    {
        string executableFileName = commandLineArguments[0];
        IReadOnlyList<string> executableArguments = commandLineArguments.Skip(1).ToArray();

        return new ProcessLaunchRequest(executableFileName, executableArguments, workingDirectory);
    }
}
