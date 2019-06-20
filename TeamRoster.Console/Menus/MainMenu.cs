using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.App.Menus
{
    public class MainMenu
    { 

        public static int DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Team Roster Manager");
            Console.WriteLine("--------------------");
            Console.WriteLine();
            Console.WriteLine(" 1. Manage Teams");
            Console.WriteLine(" 2. Manage Players");
            Console.WriteLine(" 3. Exit");
            Console.WriteLine();
            Console.Write("Choice: ");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }

        public static void Run()
        {
            int userInput = 0;
            do
            {
                try
                {
                    //get the selection
                    userInput = DisplayMenu();

                    switch (userInput)
                    {
                        case 1:
                            TeamMenu.Run();
                            break;
                        case 2:
                            PlayerMenu.Run();
                            break;
                        case 3:
                            Console.WriteLine("Exiting...");
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine(" Error: Invalid Choice");
                            System.Threading.Thread.Sleep(1000);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine(" Unexpected Error:");
                    Console.WriteLine(e);
                    System.Threading.Thread.Sleep(2000);
                }

            } while (userInput != 3);
        }


    }
}
