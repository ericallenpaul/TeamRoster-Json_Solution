using System;
using System.Collections.Generic;
using System.Text;

namespace TeamRoster.App
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ConsolePromptAttribute :System.Attribute
    {
        public string Prompt { get; set; }

        public bool Required { get; set; }

        public bool Ignore { get; set; }

        public ConsolePromptAttribute(string prompt, bool required = false, bool ignore = false)
        {
            Prompt = prompt;
            Required = required;
            Ignore = ignore;
        }

    }
}
