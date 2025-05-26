using System;
using Npgsql;

namespace GameHubConsole
{
    class Program
    {
        private static readonly string _connectionString = "Host=localhost;Username=postgres;Password=228;Database=Game-Home-Ha";

        static void Main()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\n=== Game Hub ===");
                    Console.WriteLine("1. Добавить пользователя");
                    Console.WriteLine("2. Просмотреть все игры");
                    Console.WriteLine("3. Добавить игровую сессию");
                    Console.WriteLine("4. Просмотреть друзей пользователя");
                    Console.WriteLine("5. Выход");
                    Console.Write("> ");

                    var input = Console.ReadLine();
                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Ошибка: введите число от 1 до 5");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            AddUser();
                            break;
                        case 2:
                            ViewGames();
                            break;
                        case 3:
                            AddGameSession();
                            break;
                        case 4:
                            ViewFriends();
                            break;
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Неизвестная команда");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        static void AddUser()
        {
            Console.Write("Введите username: ");
            var username = Console.ReadLine()!;
            Console.Write("Введите email: ");
            var email = Console.ReadLine()!;
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine()!;

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO Account (username, email) VALUES (@u, @e);" +
                "INSERT INTO \"User\" (account_id, password) VALUES (currval(pg_get_serial_sequence('account', 'id')), @p)",
                conn);

            cmd.Parameters.AddWithValue("u", username);
            cmd.Parameters.AddWithValue("e", email);
            cmd.Parameters.AddWithValue("p", password);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Пользователь создан!");
        }

        static void ViewGames()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT id, title, genre FROM Game", conn);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\nСписок игр:");
            while (reader.Read())
            {
                Console.WriteLine($"[{reader.GetInt32(0)}] {reader.GetString(1)} ({reader.GetString(2)})");
            }
        }

        static void AddGameSession()
        {
            Console.Write("ID пользователя: ");
            var userId = int.Parse(Console.ReadLine()!);
            Console.Write("ID игры: ");
            var gameId = int.Parse(Console.ReadLine()!);
            Console.Write("Дата начала (ГГГГ-ММ-ДД ЧЧ:ММ): ");
            var startTime = DateTime.Parse(Console.ReadLine()!);
            Console.Write("Длительность (минуты): ");
            var duration = int.Parse(Console.ReadLine()!);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO GameSession (userid, gameid, StartTime, durationMinutes) " +
                "VALUES (@u, @g, @s, @d)", conn);

            cmd.Parameters.AddWithValue("u", userId);
            cmd.Parameters.AddWithValue("g", gameId);
            cmd.Parameters.AddWithValue("s", startTime);
            cmd.Parameters.AddWithValue("d", duration);
            cmd.ExecuteNonQuery();

            Console.WriteLine("Игровая сессия добавлена!");
        }

        static void ViewFriends()
        {
            Console.Write("ID пользователя: ");
            var userId = int.Parse(Console.ReadLine()!);

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT u.id, a.username FROM FriendLink f " +
                "JOIN \"User\" u ON f.friendid = u.id " +
                "JOIN Account a ON u.account_id = a.id " +
                "WHERE f.Userid = @id", conn);

            cmd.Parameters.AddWithValue("id", userId);
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\nДрузья пользователя:");
            while (reader.Read())
            {
                Console.WriteLine($"[{reader.GetInt32(0)}] {reader.GetString(1)}");
            }
        }
    }
}