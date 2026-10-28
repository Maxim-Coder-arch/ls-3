# П2

В первой работе мы отслеживали процесс. Теперь расширим диспетчер: он должен собирать обычный вывод и сообщения об ошибках отдельно.

Не будем повторно создавать процесс с нуля — изменим стартовый проект первой практической работы.

### Стартовый код

Замените содержимое `Program.cs` в проекте `Practice2_ProcessOutput` следующим кодом.

```

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
```

### Как проверить stderr

Команда `dotnet --info` обычно пишет информацию в `stdout`, поэтому `stderr` может оказаться пустым. Это нормальное поведение, а не ошибка программы.

Чтобы проверить оба потока, можно временно заменить команду на `cmd.exe` с аргументами:

```
startInfo.FileName = "cmd.exe";
startInfo.ArgumentList.Add("/c");
startInfo.ArgumentList.Add(
    "echo Сообщение в stdout & echo Ошибка в stderr 1>&2"
);
```

Этот вариант предназначен для Windows. Он использует `cmd.exe` только в учебном тесте с фиксированной строкой, без пользовательского ввода.

Ожидаемый вывод:

```
=== STDOUT ===
Сообщение в stdout

=== STDERR ===
Ошибка в stderr
```

Код завершения обычно будет `0`, если команда выполнена успешно.
## Решение

### Структура проекта

| Папка | Назначение |
| --- | --- |
| `Domain` | Запрос на запуск, захваченный вывод и варианты результата |
| `Application` | `ProcessDispatcher` и абстракции запуска, наблюдения и вывода результата |
| `Infrastructure` | `SystemProcessLauncher` с асинхронным чтением stdout и stderr |
| `Presentation` | Консольный вывод потоков и обработка Ctrl+C |
| `Configuration` | Сценарии запуска: `dotnet --info`, демонстрация потоков, произвольная утилита |

### Запуск

Основной сценарий (`dotnet --info`, stderr ожидаемо пуст):

```
dotnet run --project Practice2_ProcessOutput
```

Проверка обоих потоков без правки кода (на Windows используется `cmd.exe`, на Linux и macOS — `/bin/sh`):

```
dotnet run --project Practice2_ProcessOutput -- --streams-demo
```

Ошибка запуска:

```
dotnet run --project Practice2_ProcessOutput -- missing-tool-12345.exe
```

### Почему нет deadlock

Чтение stdout и stderr запускается через `ReadToEndAsync` сразу после старта процесса и до `WaitForExitAsync`. Если ждать завершения до начала чтения, процесс, записавший в канал больше, чем вмещает его буфер, заблокируется на записи, а родитель будет бесконечно ждать его выхода. Оба потока читаются параллельно, поэтому переполнение одного из них не блокирует другой.

Для `cmd.exe` перед командой выполняется `chcp 65001`, чтобы кириллица в потоках передавалась в UTF-8 и корректно декодировалась.
