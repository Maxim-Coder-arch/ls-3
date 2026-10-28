using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Application;

public interface IProcessLifecycleObserver
{
    void OnProcessStarted(StartedProcessInformation startedProcessInformation);
}
