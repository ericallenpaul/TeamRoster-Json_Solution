using System;
using TeamRoster.App.Menus;

namespace TeamRoster.App
{
    public class Program
    {
        public static string DataDir;

        public static string ProgramDirectory { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        static void Main(string[] args)
        {
            DataDir = $@"{Program.ProgramDirectory}Data\";
            MainMenu.Run();
        }
    }
}
