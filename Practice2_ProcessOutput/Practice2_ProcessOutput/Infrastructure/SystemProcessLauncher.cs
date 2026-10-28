using System.ComponentModel;
using System.Diagnostics;
using Practice2_ProcessOutput.Application;
using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Infrastructure;

public sealed class SystemProcessLauncher : IProcessLauncher
{
    private const string ProcessWasNotStartedReason = "Операционная система не запустила новый процесс.";

    private readonly ProcessStartInfoFactory processStartInfoFactory;

    public SystemProcessLauncher(ProcessStartInfoFactory processStartInfoFactory)
    {
        ArgumentNullException.ThrowIfNull(processStartInfoFactory);

        this.processStartInfoFactory = processStartInfoFactory;
    }

    public async Task<ProcessExecutionOutcome> LaunchAndCaptureOutputAsync(
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

        Stopwatch stopwatch = Stopwatch.StartNew();
        int processIdentifier = 0;
        Task<string> standardOutputReadingTask = Task.FromResult(string.Empty);
        Task<string> standardErrorReadingTask = Task.FromResult(string.Empty);

        try
        {
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

            standardOutputReadingTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
            standardErrorReadingTask = process.StandardError.ReadToEndAsync(cancellationToken);

            await process.WaitForExitAsync(cancellationToken);

            CapturedProcessOutput capturedOutput = await CollectCapturedOutputAsync(
                standardOutputReadingTask,
                standardErrorReadingTask);

            stopwatch.Stop();

            return new CompletedProcessExecution(
                processIdentifier,
                stopwatch.Elapsed,
                process.ExitCode,
                capturedOutput);
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

            await AwaitReadingTasksWithoutThrowingAsync(standardOutputReadingTask, standardErrorReadingTask);

            stopwatch.Stop();

            return new CancelledProcessExecution(processIdentifier, stopwatch.Elapsed);
        }
    }

    private static async Task<CapturedProcessOutput> CollectCapturedOutputAsync(
        Task<string> standardOutputReadingTask,
        Task<string> standardErrorReadingTask)
    {
        await Task.WhenAll(standardOutputReadingTask, standardErrorReadingTask);

        string standardOutput = await standardOutputReadingTask;
        string standardError = await standardErrorReadingTask;

        return new CapturedProcessOutput(standardOutput, standardError);
    }

    private static async Task AwaitReadingTasksWithoutThrowingAsync(
        Task<string> standardOutputReadingTask,
        Task<string> standardErrorReadingTask)
    {
        Task[] readingTasks = [standardOutputReadingTask, standardErrorReadingTask];

        await Task
            .WhenAll(readingTasks)
            .ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
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
