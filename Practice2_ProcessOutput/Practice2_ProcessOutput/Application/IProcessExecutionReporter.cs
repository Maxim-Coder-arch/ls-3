using Practice2_ProcessOutput.Domain;

namespace Practice2_ProcessOutput.Application;

public interface IProcessExecutionReporter
{
    void ReportOutcome(ProcessExecutionOutcome executionOutcome);
}
