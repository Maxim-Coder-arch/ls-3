using System.Diagnostics;
using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Infrastructure;

public sealed class ProcessStartInfoFactory
{
    public ProcessStartInfo Create(ProcessLaunchRequest launchRequest)
    {
        ArgumentNullException.ThrowIfNull(launchRequest);

        ProcessStartInfo startInfo = new()
        {
            FileName = launchRequest.ExecutableFileName,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (string argument in launchRequest.Arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        startInfo.WorkingDirectory = launchRequest.WorkingDirectory;

        return startInfo;
    }
}
