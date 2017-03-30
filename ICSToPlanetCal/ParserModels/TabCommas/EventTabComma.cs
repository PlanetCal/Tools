﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal.ParserModels.TabCommas
{
    internal class EventTabComma : EventBase
    {
        public EventTabComma()
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
                    DateTime parsedDate;
                    if (DateTime.TryParse(li.Value, out parsedDate))
                    {
                    }

                }

                this.ContentLines[li.Name] = li;
            }
        }
    }
}
