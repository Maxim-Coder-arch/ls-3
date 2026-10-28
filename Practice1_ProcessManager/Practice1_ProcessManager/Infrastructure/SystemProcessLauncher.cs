using System.ComponentModel;
using System.Diagnostics;
using Practice1_ProcessManager.Application;
using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Infrastructure;

public sealed class SystemProcessLauncher : IProcessLauncher
{
    private const string ProcessWasNotStartedReason = "Операционная система не запустила новый процесс.";

    private readonly ProcessStartInfoFactory processStartInfoFactory;

    public SystemProcessLauncher(ProcessStartInfoFactory processStartInfoFactory)
    {
        ArgumentNullException.ThrowIfNull(processStartInfoFactory);

        this.processStartInfoFactory = processStartInfoFactory;
    }

    public async Task<ProcessExecutionOutcome> LaunchAndAwaitCompletionAsync(
        ProcessLaunchRequest launchRequest,
        IProcessLifecycleObserver lifecycleObserver,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(launchRequest);
        ArgumentNullException.ThrowIfNull(lifecycleObserver);

        ProcessStartInfo startInfo = processStartInfoFactory.Create(launchRequest);

        using Process process = new()
        {
            StartInfo = startInfo
        };

        Stopwatch stopwatch = new();
        int processIdentifier = 0;

        try
        {
            stopwatch.Start();

            bool processWasStarted = process.Start();

            if (!processWasStarted)
            {
                stopwatch.Stop();

                return new FailedProcessLaunch(
                    launchRequest.ExecutableFileName,
                    ProcessWasNotStartedReason,
                    stopwatch.Elapsed);
            }

            processIdentifier = process.Id;

            lifecycleObserver.OnProcessStarted(
                new StartedProcessInformation(processIdentifier, launchRequest.ExecutableFileName));

            await process.WaitForExitAsync(cancellationToken);

            stopwatch.Stop();

            return new CompletedProcessExecution(
                processIdentifier,
                stopwatch.Elapsed,
                process.ExitCode);
        }
        catch (Win32Exception launchException)
        {
            stopwatch.Stop();

            return new FailedProcessLaunch(
                launchRequest.ExecutableFileName,
                launchException.Message,
                stopwatch.Elapsed);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            TerminateProcessTree(process);
            stopwatch.Stop();

            return new CancelledProcessExecution(processIdentifier, stopwatch.Elapsed);
        }
    }

    private static void TerminateProcessTree(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch (InvalidOperationException)
        {
            return;
        }
        catch (Win32Exception)
        {
            return;
        }
    }
}
