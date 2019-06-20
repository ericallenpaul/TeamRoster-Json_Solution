using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TeamRoster.Models;

namespace TeamRoster.Services
{
    public class PlayerService
    {
        private string _DataDirectory;
        private string _DataFile = "";

        public PlayerService(string DataDirectory)
        {
            try
            {
                if (Directory.Exists(DataDirectory))
                {
                    //get the directory containing the data file
                    _DataDirectory = DataDirectory;
                }
                else
                {
                    //we couldn't find the data directory, throw an error
                    throw new Exception($"InstantiationError: Unable to create the service, Can't find the data directory: {DataDirectory}");
                }

                //derive the filename from the class
                string currentJsonFileName = this.GetType().Name.Replace("Service", "") + ".json";

                //establish the path to the data file
                _DataFile = $@"{DataDirectory}{currentJsonFileName}";
            }
            catch (Exception ex)
            {
                ex.Data.Add("InstantiationError",
                    $"An error occurred while trying to create the service.");
                throw;
            }
        }

        public List<Player> GetAll()
        {
            List<Player> returnValue = new List<Player>();

            try
            {
                //always make sure the file exists before attempting to access it
                if (File.Exists(_DataFile))
                {
                    //read the file
                    string jsonData = File.ReadAllText(_DataFile);

                    if (!String.IsNullOrEmpty(jsonData))
                    {
                        //deserialize the file back into a list
                        returnValue = JsonConvert.DeserializeObject<List<Player>>(jsonData);
                    }
                }
                else
                {
                    //we couldn't find the file, throw an error
                    throw new Exception($"GetAllError: Unable to find file: {_DataFile}");
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetAllError",
                    $"An error occurred while trying to get players.");
                throw;
            }

            return returnValue;
        }

        public Player Add(Player player, List<Player> players)
        {
            //get the next player id
            int newPlayerId = GetNextId(players);

            //assign the playe an id
            player.Player_Id = newPlayerId;

            //add the player to the list
            players.Add(player);

            //save the list
            Save(players);

            //return the player with the new ID
            return player;
        }

        public List<Player> Delete(Player player, List<Player> players)
        {
            try
            {
                players.Remove(player);
                Save(players);
            }
            catch (Exception ex)
            {
                ex.Data.Add("DeleteError", 
                    $"An error occurred while trying to delete a player. (Player ID: {player.Player_Id}");
                throw;
            }

            return players;
        }

        private void Save(List<Player> players)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(players);

                if (!string.IsNullOrEmpty(jsonData))
                {
                    File.WriteAllText(_DataFile, jsonData);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("SaveError",
                    $"An error occurred while trying to save the lsit.");
                throw;
            }
        }

        private int GetNextId(List<Player> players)
        {
            int returnValue = 1;

            try
            {
                if (players.Any())
                {
                    //get the player with the highest ID
                    var player = players.OrderByDescending(u => u.Player_Id).FirstOrDefault();

                    //get that players ID and add 1
                    int id = player.Player_Id;
                    id++;
                    returnValue = id;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetNextIdError",
                    "An error occurred while trying to get the next player Id.");
                throw;
            }

            return returnValue;
        }

    }
}
