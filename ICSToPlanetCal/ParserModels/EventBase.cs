using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    class EventBase
    {
        public Dictionary<string, LineItem> ContentLines { get; set; }

        public EventBase()
        {
            this.ContentLines = new Dictionary<string, LineItem>();
        }
    }
}
