using System.Text;
using Practice2_ProcessOutput.Application;
using Practice2_ProcessOutput.Configuration;
using Practice2_ProcessOutput.Domain;
using Practice2_ProcessOutput.Infrastructure;
using Practice2_ProcessOutput.Presentation;

namespace Practice2_ProcessOutput;

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
