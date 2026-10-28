namespace Practice1_ProcessManager.Domain;

public sealed record CompletedProcessExecution(
    int ProcessIdentifier,
    TimeSpan ElapsedTime,
    int ExitCode)
    : ProcessExecutionOutcome(ElapsedTime);
