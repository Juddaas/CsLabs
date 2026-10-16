
using System;
using System.IO;

public struct LogEntry
{
    public DateTime Timestamp;
    public string Level;
    public string Category;
    public string Message;
}

public class Program
{
    public static void Main()
    {
        string[] lines = File.ReadAllLines("event_server.log");
        LogEntry[] entries = ParseLog(lines);

        Console.WriteLine("Всего записей: " + entries.Length);
        Console.WriteLine("Ошибок: " + CountByLevel(entries, "Error"));
        Console.WriteLine("Предупреждений: " + CountByLevel(entries, "Warning"));
        Console.WriteLine("Статус: " + GetServerStatus(entries));

        LogEntry[] errors = FilterByLevel(entries, "Error");
        Console.WriteLine("Записей Error после фильтрации: " + errors.Length);

        LogEntry[] serverEntries = FilterByCategory(entries, "Server");
        Console.WriteLine("Записей категории Server: " + serverEntries.Length);

        LogEntry[] searchResults = Search(entries, "database");
        Console.WriteLine("Записей по поиску database: " + searchResults.Length);

        LogEntry[] dateEntries = FilterByDate(entries, new DateTime(2026, 9, 2));
        Console.WriteLine("Записей за 2 сентября: " + dateEntries.Length);
    }

    public static LogEntry[] ParseLog(string[] lines)
    {
        LogEntry[] entries = new LogEntry[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            LogEntry entry = new LogEntry();

            entry.Timestamp = DateTime.Parse(line.Substring(0, 23));

            int firstOpenBracket = line.IndexOf('[');
            int firstCloseBracket = line.IndexOf(']', firstOpenBracket);
            int secondOpenBracket = line.IndexOf('[', firstCloseBracket);
            int secondCloseBracket = line.IndexOf(']', secondOpenBracket);

            entry.Level = line.Substring(firstOpenBracket + 1,
                firstCloseBracket - firstOpenBracket - 1);
            entry.Category = line.Substring(secondOpenBracket + 1,
                secondCloseBracket - secondOpenBracket - 1);
            entry.Message = line.Substring(secondCloseBracket + 2);

            entries[i] = entry;
        }

        return entries;
    }

    public static LogEntry[] FilterByDate(LogEntry[] entries, DateTime date)
    {
        LogEntry[] temporaryResult = new LogEntry[entries.Length];
        int resultCount = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Timestamp.Date == date.Date)
            {
                temporaryResult[resultCount] = entries[i];
                resultCount++;
            }
        }

        return CopyResult(temporaryResult, resultCount);
    }

    public static LogEntry[] FilterByLevel(LogEntry[] entries, string level)
    {
        LogEntry[] temporaryResult = new LogEntry[entries.Length];
        int resultCount = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Level == level)
            {
                temporaryResult[resultCount] = entries[i];
                resultCount++;
            }
        }

        return CopyResult(temporaryResult, resultCount);
    }

    public static LogEntry[] FilterByCategory(LogEntry[] entries, string category)
    {
        LogEntry[] temporaryResult = new LogEntry[entries.Length];
        int resultCount = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Category == category)
            {
                temporaryResult[resultCount] = entries[i];
                resultCount++;
            }
        }

        return CopyResult(temporaryResult, resultCount);
    }

    public static LogEntry[] Search(LogEntry[] entries, string text)
    {
        LogEntry[] temporaryResult = new LogEntry[entries.Length];
        int resultCount = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Message.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                temporaryResult[resultCount] = entries[i];
                resultCount++;
            }
        }

        return CopyResult(temporaryResult, resultCount);
    }

    public static int CountByLevel(LogEntry[] entries, string level)
    {
        int count = 0;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Level == level)
            {
                count++;
            }
        }

        return count;
    }

    public static string GetServerStatus(LogEntry[] entries)
    {
        bool hasError = false;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i].Level == "Fatal" && entries[i].Category == "Server")
            {
                return "КРИТИЧЕСКАЯ ОШИБКА: сервер остановлен";
            }

            if (entries[i].Level == "Error")
            {
                hasError = true;
            }
        }

        if (hasError)
        {
            return "Есть ошибки: требуется проверка";
        }

        return "Сервер работает штатно";
    }

    public static LogEntry[] CopyResult(LogEntry[] temporaryResult, int resultCount)
    {
        LogEntry[] result = new LogEntry[resultCount];
        Array.Copy(temporaryResult, result, resultCount);
        return result;
    }
}
