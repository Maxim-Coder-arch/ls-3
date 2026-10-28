using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Application;

public interface IProcessLauncher
{
    Task<ProcessExecutionOutcome> LaunchAndCaptureOutputAsync(
        ProcessLaunchRequest launchRequest,
        IProcessLifecycleObserver lifecycleObserver,
        CancellationToken cancellationToken);
}
