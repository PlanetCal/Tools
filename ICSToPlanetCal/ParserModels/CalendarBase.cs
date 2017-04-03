using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class CalendarBase
    {
        public string Source { get; set; }
        public CalendarParameters Parameters { get; set; }

        public string Location { get; set; }

        public List<BaseEvent> Events { get; set; } = new List<BaseEvent>();

        public CalendarBase(string source)
        {
            this.Source = source;
        }
    }
}
