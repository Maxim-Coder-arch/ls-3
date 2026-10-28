using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Application;

public sealed class ProcessDispatcher
{
    private readonly IProcessLauncher processLauncher;
    private readonly IProcessLifecycleObserver lifecycleObserver;
    private readonly IProcessExecutionReporter executionReporter;

    public ProcessDispatcher(
        IProcessLauncher processLauncher,
        IProcessLifecycleObserver lifecycleObserver,
        IProcessExecutionReporter executionReporter)
    {
        ArgumentNullException.ThrowIfNull(processLauncher);
        ArgumentNullException.ThrowIfNull(lifecycleObserver);
        ArgumentNullException.ThrowIfNull(executionReporter);

        this.processLauncher = processLauncher;
        this.lifecycleObserver = lifecycleObserver;
        this.executionReporter = executionReporter;
    }

    public async Task<int> DispatchAsync(
        ProcessLaunchRequest launchRequest,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(launchRequest);

        ProcessExecutionOutcome executionOutcome = await processLauncher.LaunchAndCaptureOutputAsync(
            launchRequest,
            lifecycleObserver,
            cancellationToken);

        executionReporter.ReportOutcome(executionOutcome);

        return ResolveApplicationExitCode(executionOutcome);
    }

    private static int ResolveApplicationExitCode(ProcessExecutionOutcome executionOutcome)
    {
        return executionOutcome switch
        {
            CompletedProcessExecution completedExecution => completedExecution.ExitCode,
            FailedProcessLaunch => ApplicationExitCodes.ProcessLaunchFailed,
            CancelledProcessExecution => ApplicationExitCodes.ProcessExecutionCancelled,
            _ => throw new ArgumentOutOfRangeException(
                nameof(executionOutcome),
                executionOutcome,
                "Неизвестный результат выполнения процесса.")
        };
    }
}
