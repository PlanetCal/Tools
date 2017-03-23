using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal.ParserModels.TabCommas
{
    /// <summary>
    /// This class will process files content that is separated by tabs or commans with the header
    /// of properties for PlanetCal entity mapping.
    /// </summary>
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
