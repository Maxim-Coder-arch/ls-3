namespace Practice2_ProcessOutput.Domain;

public sealed record ProcessLaunchRequest(
    string ExecutableFileName,
    IReadOnlyList<string> Arguments,
    string WorkingDirectory);
