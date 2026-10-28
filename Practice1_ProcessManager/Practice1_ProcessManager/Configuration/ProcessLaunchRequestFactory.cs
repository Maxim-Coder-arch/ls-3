using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Configuration;

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

        string executableFileName = commandLineArguments[0];
        IReadOnlyList<string> executableArguments = commandLineArguments.Skip(1).ToArray();

        return new ProcessLaunchRequest(executableFileName, executableArguments, workingDirectory);
    }

    private static ProcessLaunchRequest CreateDefaultRequest(string workingDirectory)
    {
        return new ProcessLaunchRequest(
            DefaultLaunchSettings.ExecutableFileName,
            [DefaultLaunchSettings.InformationArgument],
            workingDirectory);
    }
}
