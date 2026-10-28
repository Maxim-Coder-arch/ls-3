using Practice1_ProcessManager.Application;
using Practice1_ProcessManager.Domain;

namespace Practice1_ProcessManager.Presentation;

public sealed class ConsoleProcessExecutionReporter : IProcessLifecycleObserver, IProcessExecutionReporter
{
    private readonly TextWriter outputWriter;

    public ConsoleProcessExecutionReporter(TextWriter outputWriter)
    {
        ArgumentNullException.ThrowIfNull(outputWriter);

        this.outputWriter = outputWriter;
    }

    public void OnProcessStarted(StartedProcessInformation startedProcessInformation)
    {
        ArgumentNullException.ThrowIfNull(startedProcessInformation);

        outputWriter.WriteLine($"PID: {startedProcessInformation.ProcessIdentifier}");
    }

    public void ReportOutcome(ProcessExecutionOutcome executionOutcome)
    {
        ArgumentNullException.ThrowIfNull(executionOutcome);

        switch (executionOutcome)
        {
            case CompletedProcessExecution completedExecution:
                ReportCompletedExecution(completedExecution);
                break;

            case FailedProcessLaunch failedLaunch:
                ReportFailedLaunch(failedLaunch);
                break;

            case CancelledProcessExecution cancelledExecution:
                ReportCancelledExecution(cancelledExecution);
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(executionOutcome),
                    executionOutcome,
                    "Неизвестный результат выполнения процесса.");
        }
    }

    private void ReportCompletedExecution(CompletedProcessExecution completedExecution)
    {
        outputWriter.WriteLine($"Длительность: {completedExecution.ElapsedTime}");
        outputWriter.WriteLine($"Exit code: {completedExecution.ExitCode}");
    }

    private void ReportFailedLaunch(FailedProcessLaunch failedLaunch)
    {
        outputWriter.WriteLine($"Ошибка запуска: {failedLaunch.FailureReason}");
        outputWriter.WriteLine($"Утилита: {failedLaunch.ExecutableFileName}");
    }

    private void ReportCancelledExecution(CancelledProcessExecution cancelledExecution)
    {
        outputWriter.WriteLine($"Выполнение процесса {cancelledExecution.ProcessIdentifier} отменено.");
        outputWriter.WriteLine($"Длительность: {cancelledExecution.ElapsedTime}");
    }
}
