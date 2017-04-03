using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class OutlookCalendar : CalendarBase
    {
        private const string Pattern = "BEGIN:VCALENDAR\\r\\n(.+?)\\r\\nBEGIN:VEVENT";
        private const string PatternNewLine = "BEGIN:VCALENDAR\\n(.+?)\\nBEGIN:VEVENT";
        private const string SummaryLanguage = "SUMMARY;LANGUAGE=";
        public const string EventPattern = "(BEGIN:VEVENT.+?END:VEVENT)";

        public OutlookCalendar(string source) : base (source)
        {
            this.ProcessEvents();
        }

        private void ProcessEvents()
        {
            Match parameterMatch = Regex.Match(this.Source, OutlookCalendar.Pattern, RegexOptions.Singleline);
            string parameterString = parameterMatch.Groups[1].ToString();
            if (string.IsNullOrEmpty(parameterString))
            {
                parameterMatch = Regex.Match(this.Source, OutlookCalendar.PatternNewLine, RegexOptions.Singleline);
                parameterString = parameterMatch.Groups[1].ToString();
            }

            this.Parameters = new CalendarParameters(parameterString);

            this.ExtractCountryFromParameters();

            foreach (Match EventMatch in Regex.Matches(this.Source, EventPattern, RegexOptions.Singleline))
            {
                string eventString = EventMatch.Groups[1].ToString();
                if (eventString.Contains(OutlookCalendar.SummaryLanguage))
                {
                    this.ExtractCountryFromEvent(eventString);
                    continue;
                }

                OutlookEvent icsEvent = new OutlookEvent(eventString);

                // Add only if the event has valid content of events...
                if (icsEvent.ContentLines.Count > 0)
                {
                    this.Events.Add(icsEvent);
                }
            }
        }

        private void ExtractCountryFromEvent(string eventString)
        {
            try
            {
                int languageIndex = eventString.IndexOf(OutlookCalendar.SummaryLanguage);
                string languageCode = eventString.Substring(languageIndex + OutlookCalendar.SummaryLanguage.Length, 5);

                var ci = new CultureInfo(languageCode);
                var ri = new RegionInfo(ci.LCID);
                this.Location = ri.DisplayName;
            }
            catch(ArgumentException)
            {
                this.Location = string.Empty;
            }
        }

        private void ExtractCountryFromParameters()
        {
            CalendarParameter cp;
            this.Parameters.TryGetValue("X-WR-CALNAME", out cp);
            if (cp == null)
            {
                return;
            }

            string[] countrySplits = cp.Value.Split(new char[] { ' ' });

            foreach (string counrty in countrySplits)
            {
                try
                {
                    if (ISO3166.FromName(counrty) != null)
                    {
                        this.Location = ISO3166.FromName(counrty).Name;
                        break;
                    }
                }
                catch (ArgumentException)
                {
                    this.Location = string.Empty;
                }
            }
        }
    }
}
