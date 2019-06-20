using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAttributeClasses
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConsolePromptAttribute :System.Attribute
    {
        public string Prompt { get; set; }

        public bool Required { get; set; }

        public bool Ignore { get; set; }

        public int MinValue { get; set; }

        //constructor with mandatory and default parameters
        public ConsolePromptAttribute(string prompt, bool required = false, bool ignore = false, int minValue = 0)
        {
            Prompt = prompt;
            Required = required;
            Ignore = ignore;
            MinValue = minValue;
        }
    }
}
