using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;

public class DBHelper
{
    // Ganti sesuai settingan MySQL masing-masing
    private static string connectionString =
        "Server=localhost;Database=sibantuan;Uid=root;Pwd=password_kamu;";

    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}