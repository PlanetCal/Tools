using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    internal class EventJson
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string StartDateTime { get; set; }

        public string EndDateTime { get; set; }

        public string Type { get; set; }

        public string Location { get; set; }

        public Address Address { get;set;}

        public string Details { get; set; }

        public Group[] Groups { get; set; }
    }

    internal class Address
    {
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    internal class Group
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string Icon { get; set; }
        public string Privacy { get; set; }
        public string Owner { get; set; }
        public string[] Administrators { get; set; }
        public string[] Members { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
    }
}
