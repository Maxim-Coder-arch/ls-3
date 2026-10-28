using System.Text;
using Practice1_ProcessManager.Application;
using Practice1_ProcessManager.Configuration;
using Practice1_ProcessManager.Domain;
using Practice1_ProcessManager.Infrastructure;
using Practice1_ProcessManager.Presentation;

namespace Practice1_ProcessManager;

public static class Program
{
    public static async Task<int> Main(string[] commandLineArguments)
    {
        Console.OutputEncoding = Encoding.UTF8;

        using ConsoleCancellationScope cancellationScope = new();

        ProcessLaunchRequest launchRequest = ProcessLaunchRequestFactory.CreateFromCommandLineArguments(
            commandLineArguments,
            Environment.CurrentDirectory);

        ConsoleProcessExecutionReporter executionReporter = new(Console.Out);

        SystemProcessLauncher processLauncher = new(new ProcessStartInfoFactory());

        ProcessDispatcher processDispatcher = new(
            processLauncher,
            executionReporter,
            executionReporter);

        return await processDispatcher.DispatchAsync(launchRequest, cancellationScope.Token);
    }
}
