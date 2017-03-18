using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    public class ItemParameters : Dictionary<string, ItemParameter>
    {
        private const string ParameterPattern = "([^;]+)(?=;|$)";

        public ItemParameters(string source)
        {
            MatchCollection matches = Regex.Matches(source, ParameterPattern);
            foreach (Match match in matches)
            {
                ItemParameter contentLineParameter = new ItemParameter(match.Groups[1].ToString());
                this[contentLineParameter.Name] = contentLineParameter;
            }
        }
    }
}
