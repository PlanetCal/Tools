using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class BaseEvent
    {
        public Dictionary<string, LineItem> ContentLines { get; set; }

        public BaseEvent()
        {
            this.ContentLines = new Dictionary<string, LineItem>();
        }
    }
}
