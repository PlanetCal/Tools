using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class Calendar
    {
        private const string Pattern = "BEGIN:VCALENDAR\\r\\n(.+?)\\r\\nBEGIN:VEVENT";
        private const string PatternNewLine = "BEGIN:VCALENDAR\\n(.+?)\\nBEGIN:VEVENT";
        private const string SummaryLanguage = "SUMMARY;LANGUAGE=";
        public const string EventPattern = "(BEGIN:VEVENT.+?END:VEVENT)";

        public string Source { get; set; }
        public CalendarParameters Parameters { get; set; }

        public string Location { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public Calendar(string source)
        {
            this.Source = source;

            this.ProcessEvents();
        }

        private void ProcessEvents()
        {
            Match parameterMatch = Regex.Match(this.Source, Calendar.Pattern, RegexOptions.Singleline);
            string parameterString = parameterMatch.Groups[1].ToString();
            if (string.IsNullOrEmpty(parameterString))
            {
                parameterMatch = Regex.Match(this.Source, Calendar.PatternNewLine, RegexOptions.Singleline);
                parameterString = parameterMatch.Groups[1].ToString();
            }

            Parameters = new CalendarParameters(parameterString);

            this.ExtractCountryFromParameters();

            foreach (Match EventMatch in Regex.Matches(this.Source, EventPattern, RegexOptions.Singleline))
            {
                string eventString = EventMatch.Groups[1].ToString();
                if (eventString.Contains(Calendar.SummaryLanguage))
                {
                    this.ExtractCountryFromEvent(eventString);
                    continue;
                }

                Events.Add(new Event(eventString));
            }
        }

        private void ExtractCountryFromEvent(string eventString)
        {
            try
            {
                int languageIndex = eventString.IndexOf(Calendar.SummaryLanguage);
                string languageCode = eventString.Substring(languageIndex + Calendar.SummaryLanguage.Length, 5);

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
                    }
                    break;
                }
                catch (ArgumentException)
                {
                    this.Location = string.Empty;
                }
            }
        }
    }
}
