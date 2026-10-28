namespace Practice2_ProcessOutput.Domain;

public sealed record CancelledProcessExecution(
    int ProcessIdentifier,
    TimeSpan ElapsedTime)
    : ProcessExecutionOutcome(ElapsedTime);
