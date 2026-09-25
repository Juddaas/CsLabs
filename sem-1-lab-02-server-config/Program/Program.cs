using System;

{
    public class Program
    {
        const int playersPerGb = 10;
        const int minMemory = 2;
        public static string CheckConfiguration(int players, int memory, bool isPublic, bool hasPassword)
        {
            int status = 0;

            if (players <= 0)
            {
                status = 2;
            }
            if (memory < minMemory)
            {
                status = 2;
            }
            if (players > memory * playersPerGb)
            {
                if (status < 2)
                {
                    status = 1;
                }
            }
            if (isPublic && hasPassword)
            {
                if (status < 2)
                {
                    status = 1;
                }
            }
            if (status == 2)
            {
                if (players <= 0)
                {
                    return "Запуск невозможен: количество игроков должно быть больше нуля.";
                }

                return "Запуск невозможен: серверу недостаточно оперативной памяти.";
            }
            if (status == 1)
            {
                if (isPublic && hasPassword)
                {
                    return "Запуск возможен с предупреждением: публичный сервер защищён паролем.";
                }

                return "Запуск возможен с предупреждением: для такого количества игроков рекомендуется больше оперативной памяти.";
            }
            return "Сервер готов к запуску.";
        }
        static void Main(string[] args)
        {
            Console.WriteLine("=== Проверка конфигурации игрового сервера ===");
            Console.WriteLine();
            Console.Write("Количество игроков: ");
            int players = ReadNumber();
            Console.Write("Оперативная память (ГБ): ");
            int memory = ReadNumber();
            Console.Write("Сервер публичный? (1 - да, 0 - нет): ");
            bool isPublic = Console.ReadLine() == "1";
            Console.Write("Установлен пароль? (1 - да, 0 - нет): ");
            bool hasPassword = Console.ReadLine() == "1";
            Console.WriteLine();
            Console.WriteLine("Результат проверки:");
            Console.WriteLine(CheckConfiguration(players, memory, isPublic, hasPassword));
        }
        static int ReadNumber()
        {
            int number = 0;
            while (!int.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Ошибка. Введите целое число: ");
            }
            return number;
        }
    }
}
