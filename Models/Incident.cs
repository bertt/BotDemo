using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotDemo.Models
{
    public class Incident
    {
        public IncidentTypes Type { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return $"Type: {Type}, Location: {Location}";
        }
    }
}