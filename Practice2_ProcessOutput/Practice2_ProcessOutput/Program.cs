using System;
using System.Diagnostics;
using System.ComponentModel;

public class Program
{
    public static async Task Main()
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            UseShellExecute = false,
            CreateNoWindow = true,

            // TODO 1. Включите перенаправление stdout.
            // TODO 2. Включите перенаправление stderr.
        };

        startInfo.ArgumentList.Add("--info");

        using Process process = new()
        {
            StartInfo = startInfo
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            // TODO 3. Запустите процесс.
            process.Start();

            Console.WriteLine($"PID: {process.Id}");

            // TODO 4. Начните чтение stdout асинхронно.
            // TODO 5. Начните чтение stderr асинхронно.
            // Важно: начать оба чтения до ожидания завершения.

            // TODO 6. Дождитесь завершения процесса.

            // TODO 7. Получите оба результата чтения.
            // TODO 8. Остановите секундомер.

            // TODO 9. Выведите длительность и ExitCode.
            // TODO 10. Выведите stdout и stderr отдельно.
        }
        catch (Win32Exception ex)
        {
            stopwatch.Stop();
            Console.WriteLine($"Ошибка запуска: {ex.Message}");
        }
    }
}
