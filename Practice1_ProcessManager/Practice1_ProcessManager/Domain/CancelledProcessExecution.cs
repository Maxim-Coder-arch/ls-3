namespace Practice1_ProcessManager.Domain;

public sealed record CancelledProcessExecution(
    int ProcessIdentifier,
    TimeSpan ElapsedTime)
    : ProcessExecutionOutcome(ElapsedTime);
