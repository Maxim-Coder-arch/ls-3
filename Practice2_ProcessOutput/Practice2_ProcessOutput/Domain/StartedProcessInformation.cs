namespace Practice2_ProcessOutput.Domain;

public sealed record StartedProcessInformation(
    int ProcessIdentifier,
    string ExecutableFileName);
