using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Application;

public interface IProcessLauncher
{
    Task<ProcessExecutionOutcome> LaunchAndAwaitCompletionAsync(
        ProcessLaunchRequest launchRequest,
        IProcessLifecycleObserver lifecycleObserver,
        CancellationToken cancellationToken);
}
