namespace Practice2_ProcessOutput.Domain;

public sealed record CompletedProcessExecution(
    int ProcessIdentifier,
    TimeSpan ElapsedTime,
    int ExitCode,
    CapturedProcessOutput CapturedOutput)
    : ProcessExecutionOutcome(ElapsedTime);
