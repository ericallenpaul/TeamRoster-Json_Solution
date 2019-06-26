using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CustomAttributeClasses;
using Newtonsoft.Json;
using TeamRoster.Models;

namespace TeamRoster.Services
{
    

    public class Service<T> : IService<T> where T : class
    {
        private string _DataDirectory;
        private string _DataFile = "";

        public Service(string DataDirectory)
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
                T obj = (T) Activator.CreateInstance(typeof(T));
                string currentJsonFileName = obj.GetType().Name + ".json";

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

        public T Add(T objectToAdd)
        {
            List<T> listOfT = GetAll();

            //get the next id
            int newId = GetNextId(listOfT);

            //assign an id
            var idPropertyName = GetObjectKeyPropertyName(objectToAdd);
            TrySetProperty(objectToAdd, idPropertyName, newId);

            //add the player to the list
            listOfT.Add(objectToAdd);

            //save the list
            Save(listOfT);

            //return the player with the new ID
            return objectToAdd;
        }

        public List<T> Delete(T objectToRemove)
        {
            List<T> listOfT = GetAll();
            Type type = objectToRemove.GetType();

            try
            {
                listOfT.Remove(objectToRemove);
                Save(listOfT);
            }
            catch (Exception ex)
            {
                ex.Data.Add("DeleteError",
                    $"An error occurred while trying to delete a {type.Name}");
                throw;
            }

            return listOfT;
        }

        public List<T> GetAll()
        {
            List<T> returnValue = new List<T>();

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
                        returnValue = JsonConvert.DeserializeObject<List<T>>(jsonData);
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

        public int GetNextId(List<T> listOfT)
        {
            int returnValue = 1;

            try
            {
                if (listOfT.Any())
                {
                    var idPropertyName = GetObjectKeyPropertyName(listOfT[0]);

                    //get the player with the highest ID
                    var lastObjectAdded = OrderByGeneric<T>(listOfT, idPropertyName).FirstOrDefault();

                    //get that players ID and add 1
                   
                    int id = TryGetPropertyValue(lastObjectAdded, idPropertyName);
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

        public void Save(List<T> listOfT)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(listOfT);

                if (!string.IsNullOrEmpty(jsonData))
                {
                    File.WriteAllText(_DataFile, jsonData);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("SaveError",
                    $"An error occurred while trying to save the list.");
                throw;
            }
        }

        public static List<T> OrderByGeneric<T>(List<T> entities, string propertyName)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = entities.First().GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return entities.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();
        }


        private void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }

        public int TryGetPropertyValue(T obj, string propertyName) 
        {
            Type type = obj.GetType();
            PropertyInfo[] props = type.GetProperties();

            foreach (var prop in props)
            {
                if (prop.Name == propertyName)
                {
                    return (int)prop.GetValue(obj);
                }
            }

            return 0;
        }
    

        private string GetObjectKeyPropertyName(T objectToSearch)
        {
            string returnValue = "";

            PropertyInfo[] properties = objectToSearch.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;

                if (attribute != null) // This property has a KeyAttribute
                {
                    // Do something, to read from the property:
                    returnValue = property.Name;
                }
            }

            return returnValue;
        }
    }
}
