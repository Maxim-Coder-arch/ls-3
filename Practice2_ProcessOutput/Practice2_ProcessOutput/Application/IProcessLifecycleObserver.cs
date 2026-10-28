using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Application;

public interface IProcessLifecycleObserver
{
    void OnProcessStarted(StartedProcessInformation startedProcessInformation);
}
