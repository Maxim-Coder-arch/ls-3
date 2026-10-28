namespace Practice1_ProcessManager.Domain;

public sealed record ProcessLaunchRequest(
    string ExecutableFileName,
    IReadOnlyList<string> Arguments,
    string WorkingDirectory);
