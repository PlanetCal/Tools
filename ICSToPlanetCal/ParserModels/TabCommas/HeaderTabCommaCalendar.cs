using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    /// <summary>
    /// This class will process files content that is separated by tabs or commans with the header
    /// of properties for PlanetCal entity mapping.
    /// </summary>
    internal class HeaderTabCommaCalendar : CalendarBase
    {
        public HeaderTabCommaCalendar(string source) : base (source)
        {
            this.ProcessEvents();
        }

        private void ProcessEvents()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(this.Source);
                string line;
                bool header = true;
                string[] headerNames = null;
                while ((line = sr.ReadLine()) != null)
                {
                    // Process the header
                    string[] properties = line.Split(new char[] { '\t' });
                    if (header)
                    {
                        headerNames = new string[properties.Length];
                        properties.CopyTo(headerNames, 0);
                        header = false;
                        continue;
                    }

                    TabCommaEvent etc = new TabCommaEvent();
                    etc.PopulateEvents(headerNames, properties);

                    this.Events.Add(etc);
                }
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
    }
}
