using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations;
using CustomAttributeClasses;

namespace TeamRoster.App
{
    public class ConsoleInput
    {
        public static T GetUserInput<T>() where T : new()
        {
            //set up the return value
            //in this case generic returned value meaning we don't know the type
            T returnvalue = new T();

            //get the type of object
            Type objectType = typeof(T);

            //get the objects property list
            PropertyInfo[] properties = objectType.GetProperties();

            foreach (var property in properties)
            {
                bool isRequired = false;
                bool isIgnored = false;
                bool isValid = false;
                int minValue = 0;

                //figure out the prompt based on the display name attribute
                //set the prompt to the property name
                string prompt = property.Name;

                var promptFromDisplayName = returnvalue.GetAttributeFrom<ConsolePromptAttribute>(property.Name);
                if (promptFromDisplayName != null)
                {
                    prompt = promptFromDisplayName.Prompt;
                    isIgnored = promptFromDisplayName.Ignore;
                    isRequired = promptFromDisplayName.Required;
                    minValue = promptFromDisplayName.MinValue;
                }

                //skip this one if it's ignored
                if (isIgnored)
                {
                    //go to the next property
                    continue;
                }

                //write the prompt to the screen
                Console.Write($"{prompt}: ");

                //get the input from the user
                object result;
                CallTryParseInput(property, isRequired, minValue, out isValid, out result);

                //make sure we have valid input
                while (!isValid)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Invalid input!");
                    Console.WriteLine($"Please try a value that is {property.PropertyType.GetFriendlyName()} as {property.Name}");
                    Console.WriteLine();
                    Console.Write($"{property.Name}: ");
                    CallTryParseInput(property, isRequired, minValue, out isValid, out result);
                }

                //set the properties value based on the user input
                property.SetValue(returnvalue, result);

            }

            return returnvalue;
        }

       

        private static void CallTryParseInput(PropertyInfo property, bool isRequired, int minValue, out bool isValid, out object result)
        {
            Type t = property.PropertyType;
            Type ex = typeof(ConsoleInput);
            MethodInfo mi = ex.GetMethod("TryParseUserInput");
            MethodInfo miConstructed = mi.MakeGenericMethod(t);
            object[] args = { Console.ReadLine(), isRequired, minValue, null };
            result = miConstructed.Invoke(null, args);
            isValid = (bool)args[3];
        }

        public static T TryParseUserInput<T>(string valueToParse, bool isRequired, int minValue, out bool isValid)
        {
            //assign the default "out" value
            isValid = false;

            try
            {
                //if it's required but empty return the default with isValid as false
                if (string.IsNullOrEmpty(valueToParse) && isRequired)
                {
                    return default(T);
                }

                //if it's empty and not required return with isValid as true
                //with the default value
                if (string.IsNullOrEmpty(valueToParse))
                {
                    isValid = true;
                    return default(T);
                }

                //convert the value to the specified type (T)
                var returnValue = (T)Convert.ChangeType(valueToParse, typeof(T), CultureInfo.InvariantCulture);

                //if the type is int, not null and minValue is not null, check to see if the number is greater than minValue
                if (typeof(T) == typeof(int) && returnValue != null)
                {
                    int returnValueCompare = Convert.ToInt32(returnValue);

                    if (returnValueCompare < minValue)
                    {
                        isValid = false;
                        return returnValue;
                    }
                }

                isValid = true;
                return returnValue;
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        

    }
}
