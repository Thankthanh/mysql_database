using MySql.Data.MySqlClient;

namespace ConsoleApp1
{
    public class DatabaseHelper
    {
        private static string connectionString = "server=localhost;database=cosmetics_shop;uid=root;pwd=******;charset=utf8mb4;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}