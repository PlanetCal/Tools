using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class LineItem
    {
        private const string ContentPattern = "(.+?)((;.+?)*):(.+)";

        public string Name { get; set; }
        public string Value { get; set; }
        public ItemParameters Parameters { get; set; }

        public LineItem()
        {
        }

        public LineItem(string source)
        {
            source = UnfoldAndUnescape(source);
            Match match = Regex.Match(source, LineItem.ContentPattern, RegexOptions.Singleline);
            
            // Add Error Handling
            Name = match.Groups[1].ToString().Trim();
            Parameters = new ItemParameters(match.Groups[2].ToString());
            Value = match.Groups[4].ToString().Trim();
        }

        public static string UnfoldAndUnescape(string s)
        {
            string unfold = Regex.Replace(s, "(\\r\\n )", "");
            unfold = Regex.Replace(s, "(\\n )", "");

            return Regex.Unescape(unfold);
        }
    }
}
