namespace Practice1_ProcessManager.Domain;

public sealed record StartedProcessInformation(
    int ProcessIdentifier,
    string ExecutableFileName);
