using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CustomAttributeClasses;

namespace TeamRoster.Models
{
    public class Team
    {
        [Key]
        [ConsolePrompt("Id", true, true)]
        public int Team_Id { get; set; }

        public string TeamName { get; set; }
    }
}
