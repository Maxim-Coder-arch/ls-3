namespace Practice2_ProcessOutput.Domain;

public sealed record CapturedProcessOutput(
    string StandardOutput,
    string StandardError)
{
    public bool HasStandardOutput => !string.IsNullOrWhiteSpace(StandardOutput);

    public bool HasStandardError => !string.IsNullOrWhiteSpace(StandardError);
}
