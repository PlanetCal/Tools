using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class CalendarParameters : Dictionary<string, CalendarParameter>
    {
        private const string Pattern = "(.+?):(.+?)(?=\\r\\n[A-Z]|$)";
        private const string PatternNewLine = "(.+?):(.+?)(?=\\n[A-Z]|$)";
        private const RegexOptions ParameteRegexOptions = RegexOptions.Singleline;

        public CalendarParameters(string source)
        {
            MatchCollection parameterMatches = Regex.Matches(source, CalendarParameters.Pattern, ParameteRegexOptions);
            if (parameterMatches.Count <= 1)
            {
                parameterMatches = Regex.Matches(source, CalendarParameters.PatternNewLine, ParameteRegexOptions);
            }

            foreach (Match parametereMatch in parameterMatches)
            {
                string parameterString = parametereMatch.Groups[0].ToString();
                CalendarParameter calendarParameter = new CalendarParameter(parameterString);
                this[calendarParameter.Name] = calendarParameter;
            }
        }
    }
}
