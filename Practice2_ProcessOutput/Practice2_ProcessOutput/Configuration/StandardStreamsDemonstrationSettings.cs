namespace Practice2_ProcessOutput.Configuration;

public static class StandardStreamsDemonstrationSettings
{
    public const string WindowsCommandInterpreter = "cmd.exe";

    public const string WindowsExecuteAndTerminateSwitch = "/c";

    public const string WindowsCommandText =
        "chcp 65001 > nul & echo Сообщение в stdout & echo Ошибка в stderr 1>&2";

    public const string UnixShell = "/bin/sh";

    public const string UnixExecuteCommandSwitch = "-c";

    public const string UnixCommandText =
        "echo 'Сообщение в stdout'; echo 'Ошибка в stderr' 1>&2";
}
