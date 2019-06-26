using CustomAttributeClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamRoster.Models
{
    public class Player
    {
        [Key]
        [ConsolePrompt("Id",true,true)]
        public int Player_Id { get; set; }

        [ConsolePrompt("First Name",true)]
        public string FirstName { get; set; }

        [ConsolePrompt("Last Name", true)]
        public string LastName { get; set; }

        [ConsolePrompt("Team", true)]
        public string Team { get; set; }

        [ConsolePrompt("Age", true, false, 1)]
        public int Age { get; set; }

        [ConsolePrompt("Date Added", true, true)]
        public DateTime DateAdded { get; set; }

    }
}
