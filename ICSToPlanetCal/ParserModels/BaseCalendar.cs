using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal.ParserModels
{
    internal class BaseCalendar
    {
        public string Source { get; set; }
        public CalendarParameters Parameters { get; set; }

        public string Location { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public BaseCalendar(string source)
        {
            this.Source = source;
        }
    }
}
