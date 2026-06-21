using System;

namespace SIBANTUAN
{
    // Simple static session holder for currently logged-in user.
    // Properties are nullable to allow clearing on logout.
    public static class Session
    {
        public static int? UserId { get; set; }
        public static string Username { get; set; }
        public static string Nama { get; set; }
        public static string Role { get; set; }

        public static void Clear()
        {
            UserId = null;
            Username = null;
            Nama = null;
            Role = null;
        }
    }
}
