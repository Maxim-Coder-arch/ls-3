namespace Practice1_ProcessManager.Domain;

public sealed record FailedProcessLaunch(
    string ExecutableFileName,
    string FailureReason,
    TimeSpan ElapsedTime)
    : ProcessExecutionOutcome(ElapsedTime);
