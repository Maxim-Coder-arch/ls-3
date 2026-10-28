namespace Practice2_ProcessOutput.Domain;

public sealed record FailedProcessLaunch(
    string ExecutableFileName,
    string FailureReason,
    TimeSpan ElapsedTime)
    : ProcessExecutionOutcome(ElapsedTime);
