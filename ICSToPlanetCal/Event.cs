using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class Event
    {
        private const string Pattern = "BEGIN:VEVENT\\r\\n(.+)\\r\\nEND:VEVENT";
        private const string PatternNewLine = "BEGIN:VEVENT\\n(.+)\\nEND:VEVENT";
        private const string LinePattern = "(.+?):(.+?)(?=\\r\\n[A-Z]|$)";
        private const string LinePatternNewLine = "(.+?):(.+?)(?=\\n[A-Z]|$)";

        public Dictionary<string, LineItem> ContentLines { get; set; }

        public Event(string source)
        {
            Match contentMatch = Regex.Match(source, Event.Pattern, RegexOptions.Singleline);
            string content = contentMatch.Groups[1].ToString();
            if (string.IsNullOrEmpty(content))
            {
                contentMatch = Regex.Match(source, Event.PatternNewLine, RegexOptions.Singleline);
                content = contentMatch.Groups[1].ToString();
            }

            MatchCollection matches = Regex.Matches(content, Event.LinePattern, RegexOptions.Singleline);
            if (matches.Count <=1)
            {
                matches = Regex.Matches(content, Event.LinePatternNewLine, RegexOptions.Singleline);
            }

            ContentLines = new Dictionary<string, LineItem>();
            foreach (Match match in matches)
            {
                string contentLineString = match.Groups[0].ToString();
                LineItem contentLine = new LineItem(contentLineString);

                // If the date of the event is in the past year, we won't add it.
                if (contentLine.Name == "DTSTART")
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(contentLine.Value, @"yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                    {
                        if (DateTime.UtcNow.Year > dt.Year)
                        {
                            this.ContentLines.Clear();
                            break;
                        }
                    }
                }

                // If the parameter isn't a required for PlanetCal, don't add to content lines...
                if (string.IsNullOrEmpty(Mappings.CalParam(contentLine.Name)))
                {
                    continue;
                }

                this.ContentLines[contentLine.Name] = contentLine;
            }
        }
    }
}

