using System;
using System.IO;

namespace sem_1_lab_03_event_report
{
    public class Program
    {
        public static void Main()
        {
            string[] lines = File.ReadAllLines("event_server.log");
            BuildReport(lines);
        }

        public static void BuildReport(string[] lines)
        {
            bool isEventRunning = false;
            string date = "";
            string winner = "";
            string winnerPoints = "";
            string item = "";
            string consolationReward = "";
            string pointsPlayer = "";
            string pointsValue = "";
            int warnings = 0;
            int errors = 0;

            foreach (string line in lines)
            {
                if (line.Contains("Событие началось: Восстание Ледяного Пламени"))
                {
                    isEventRunning = true;
                    int dateEnd = line.IndexOf(' ');
                    DateTime eventDate = DateTime.Parse(line.Substring(0, dateEnd));
                    date = eventDate.ToString("dd.MM.yyyy");
                }

                if (!isEventRunning)
                {
                    continue;
                }

                if (line.Contains("[Warning]"))
                {
                    warnings++;
                }

                if (line.Contains("[Error]"))
                {
                    errors++;
                }

                if (line.Contains("[Reward]") && line.Contains(" объявлены победителями события"))
                {
                    int messageStart = line.IndexOf("] ") + 2;
                    int winnerEnd = line.IndexOf(" объявлены победителями");
                    winner = line.Substring(messageStart, winnerEnd - messageStart);
                }

                if (line.Contains("[Loot]") && line.Contains("получили ивентовый предмет:"))
                {
                    int itemStart = line.IndexOf("получили ивентовый предмет:") + "получили ивентовый предмет:".Length;
                    item = line.Substring(itemStart).Trim();
                }

                if (line.Contains("[Reward]") && line.Contains("утешительную награду:"))
                {
                    int rewardStart = line.IndexOf("утешительную награду:") + "утешительную награду:".Length;
                    consolationReward = line.Substring(rewardStart).Trim();
                }

                if (line.Contains("Событие \"Восстание Ледяного Пламени\" закрыто"))
                {
                    isEventRunning = false;
                }
            }

            foreach (string line in lines)
            {
                if (line.Contains("[Reward]") && line.Contains(" получили ") && line.Contains(" очков события"))
                {
                    int messageStart = line.IndexOf("] ") + 2;
                    int pointsStart = line.IndexOf(" получили ", messageStart);
                    int valueStart = pointsStart + " получили ".Length;
                    int pointsEnd = line.IndexOf(" очков события", valueStart);

                    pointsPlayer = line.Substring(messageStart, pointsStart - messageStart).Trim();
                    pointsValue = line.Substring(valueStart, pointsEnd - valueStart).Trim();

                    if (pointsPlayer == winner)
                    {
                        winnerPoints = pointsValue;
                        break;
                    }
                }
            }

            Console.WriteLine("# Итоги события: Восстание Ледяного Пламени");
            Console.WriteLine();
            Console.WriteLine($"Дата: {date}");
            Console.WriteLine($"Победитель: {winner}");
            Console.WriteLine($"Очки победителя: {winnerPoints}");
            Console.WriteLine($"Ивентовый предмет: {item}");
            Console.WriteLine($"Утешительная награда Железных волков: {consolationReward}");
            Console.WriteLine($"Предупреждений во время события: {warnings}");
            Console.WriteLine($"Ошибок во время события: {errors}");
        }
    }
}