using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal.ParserModels.TabCommas
{
    internal class HeaderTabComma : BaseCalendar
    {
        public HeaderTabComma(string source) : base (source)
        {
            this.ProcessEvents();
        }

        private void ProcessEvents()
        {
        }
    }
}
