using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Application;

public interface IProcessExecutionReporter
{
    void ReportOutcome(ProcessExecutionOutcome executionOutcome);
}
