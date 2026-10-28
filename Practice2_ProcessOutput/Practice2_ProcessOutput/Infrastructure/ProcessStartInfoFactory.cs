using System.Diagnostics;
using System.Text;
using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Infrastructure;

public sealed class ProcessStartInfoFactory
{
    public ProcessStartInfo Create(ProcessLaunchRequest launchRequest)
    {
        ArgumentNullException.ThrowIfNull(launchRequest);

        ProcessStartInfo startInfo = new()
        {
            FileName = launchRequest.ExecutableFileName,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        foreach (string argument in launchRequest.Arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        startInfo.WorkingDirectory = launchRequest.WorkingDirectory;

        return startInfo;
    }
}
