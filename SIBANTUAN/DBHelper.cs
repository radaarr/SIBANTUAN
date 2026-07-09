using MySql.Data.MySqlClient;
using System;

public class DBHelper
{
    private static string connectionString =
        "Server=localhost;Database=sibantuan;Uid=root;Pwd=;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}