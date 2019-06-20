using System;
using TeamRoster.App.Menus;

namespace TeamRoster.App
{
    public class Program
    {
        public static string DataDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        static void Main(string[] args)
        {
            MainMenu.Run();
        }
    }
}
