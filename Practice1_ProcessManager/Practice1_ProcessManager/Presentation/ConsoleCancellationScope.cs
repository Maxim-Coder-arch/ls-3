namespace Practice1_ProcessManager.Presentation;

public sealed class ConsoleCancellationScope : IDisposable
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private bool isDisposed;

    public ConsoleCancellationScope()
    {
        Console.CancelKeyPress += HandleCancelKeyPress;
    }

    public CancellationToken Token => cancellationTokenSource.Token;

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        Console.CancelKeyPress -= HandleCancelKeyPress;
        cancellationTokenSource.Dispose();
        isDisposed = true;
    }

    private void HandleCancelKeyPress(object? sender, ConsoleCancelEventArgs eventArguments)
    {
        eventArguments.Cancel = true;
        cancellationTokenSource.Cancel();
    }
}
