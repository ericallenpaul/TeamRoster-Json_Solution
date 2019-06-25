using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TeamRoster.Models;

namespace TeamRoster.Services
{
    public class TeamService
    {
        private string _DataDirectory;
        private string _DataFile = "";

        public TeamService(string DataDirectory)
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

        public List<Team> GetAll()
        {
            List<Team> returnValue = new List<Team>();

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
                        returnValue = JsonConvert.DeserializeObject<List<Team>>(jsonData);
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
                    $"An error occurred while trying to get teams.");
                throw;
            }

            return returnValue;
        }

        public Team Add(Team team, List<Team> teams)
        {
            //get the next team id
            int newTeamId = GetNextId(teams);

            //assign the playe an id
            team.Team_Id = newTeamId;

            //add the team to the list
            teams.Add(team);

            //save the list
            Save(teams);

            //return the team with the new ID
            return team;
        }

        public List<Team> Delete(Team team, List<Team> teams)
        {
            try
            {
                teams.Remove(team);
                Save(teams);
            }
            catch (Exception ex)
            {
                ex.Data.Add("DeleteError",
                    $"An error occurred while trying to delete a team. (Team ID: {team.Team_Id}");
                throw;
            }

            return teams;
        }

        private void Save(List<Team> teams)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(teams);

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

        private int GetNextId(List<Team> teams)
        {
            int returnValue = 1;

            try
            {
                if (teams.Any())
                {
                    //get the team with the highest ID
                    var team = teams.OrderByDescending(u => u.Team_Id).FirstOrDefault();

                    //get that teams ID and add 1
                    int id = team.Team_Id;
                    id++;
                    returnValue = id;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetNextIdError",
                    "An error occurred while trying to get the next team Id.");
                throw;
            }

            return returnValue;
        }
    }
}
