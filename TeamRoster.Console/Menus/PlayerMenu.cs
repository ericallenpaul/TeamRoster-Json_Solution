using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TeamRoster.Models;
using TeamRoster.Services;

namespace TeamRoster.App.Menus
{
    public class PlayerMenu
    {
        private static string _dataDir;
        private static PlayerService _playerService; 

        public static List<Player> _playerList { get; set; }


        public static int DisplayMenu()
        {
            //get the data directory and json file
            _dataDir = $@"{Program.DataDirectory}Data\";
            _playerService = new PlayerService(_dataDir);

            //print the menu to screen
            Console.Clear();
            Console.WriteLine("Player Manager");
            Console.WriteLine("--------------------");
            Console.WriteLine();
            Console.WriteLine(" 1. List Players");
            Console.WriteLine(" 2. Add Player");
            Console.WriteLine(" 3. Delete Player");
            Console.WriteLine(" 4. Return to Main Menu");
            Console.WriteLine();
            Console.Write("Choice: ");

            //capture the result
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }

        public static void Run()
        {
            //create a var to hold the user's selection
            int userInput = 0;

            //continue to loop until a valid
            //number is chosen
            do
            {
                //get the selection
                userInput = DisplayMenu();

                //perform an action based on a selection
                switch (userInput)
                {
                    case 1:
                        GetAll();
                        break;
                    case 2:
                        Add();
                        break;
                    case 3:
                        Delete();
                        break;
                    case 4:
                        MainMenu.Run();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine(" Error: Invalid Choice");
                        System.Threading.Thread.Sleep(1000);
                        break;
                }

            } while (userInput != 4);
        }

        private static void GetAll()
        {
            //get the list of players from the service
            _playerList = _playerService.GetAll();

            //create a ConsoleTable object for displaying
            //output like a table
            ConsoleTable ct = new ConsoleTable(77);

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Players");
            ct.PrintLine();

            string[] headers = new[] { "Id", "First", "Last", "Team", "Age"};
            ct.PrintRow(headers);
            ct.PrintLine();

            if (_playerList.Any())
            {
                foreach (var player in _playerList)
                {
                    string[] rowData = new[] {player.Player_Id.ToString(), player.FirstName, player.LastName, player.Team, player.Age.ToString() };
                    ct.PrintRow(rowData);
                }
            }
            else
            {
                Console.WriteLine("There are no players to list. Try adding a player.");
            }

            ct.PrintLine();

            Console.WriteLine();
            Console.WriteLine(" Press [enter] to return to the menu.");
            Console.ReadLine();
        }

        private static void Add()
        {
            //get the existing list of players
            _playerList = _playerService.GetAll();

            //instantiate the new player object
            Player player = new Player();

            //for each property get info from the user
            player = ConsoleInput.GetUserInput<Player>();
            
            //get the current date and time for DateAdded
            player.DateAdded = DateTime.Now;

            //call the player service and add the player
            player = _playerService.Add(player, _playerList);

            //give the user feed back--pause for one second on screen
            string message = $"Success: Added a new player ID: {player.Player_Id}";
            ConsoleMessage.ShowMessage(message);
        }

        private static void Delete()
        {
            //collect the id of the player to delete
            Console.Write("ID of player to delete: ");
            int.TryParse(Console.ReadLine(), out var playerId);
 
            //get the list of players
            _playerList = _playerService.GetAll();
            var playerToRemove = _playerList.SingleOrDefault(s => s.Player_Id == playerId);

            //make sure a player with the specified id exists
            //before attemptin to delete it
            if (playerToRemove != null)
            {
                //delete the player
                _playerService.Delete(playerToRemove,_playerList);

                //give feedback to the user and pause for one second
                string message = $"Player ID: {playerId} was deleted.";
                ConsoleMessage.ShowMessage(message);
            }
            else
            {
                //could not find the specified player show and error and pause for a second
                string message = $"ERROR: Could not find player with ID: {playerId}.";
                ConsoleMessage.ShowMessage(message);
            }
        }



    }
}
