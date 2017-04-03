using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    internal class TabCommaEvent : BaseEvent
    {
        public TabCommaEvent() : base()
        {
        }

        public void PopulateEvents(string[] propertyNames, string[] propertyValues)
        {
            for (int index= 0; index < propertyNames.Length; index++)
            {
                if (string.IsNullOrEmpty(Mappings.CalParam(propertyNames[index])))
                {
                    continue;
                }

                LineItem li = new LineItem();
                li.Name = propertyNames[index];
                li.Value = index < propertyValues.Length ? propertyValues[index] : string.Empty;

                if (li.Name == "DTSTART")
                {
                    if (DateTime.TryParse(li.Value, out DateTime parsedDate))
                    {
                        li.Value = parsedDate.ToString("yyyyMMdd");
                    }

                }

                this.ContentLines[li.Name] = li;
            }
        }
    }
}
