using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    internal static class Mappings
    {
        public static string CalParam(string icsParam)
        {
            Dictionary<string, string> paramMaps = BuildParamMappings();

            paramMaps.TryGetValue(icsParam, out string planetCalParamName);

            return planetCalParamName;
        }

        private static Dictionary<string, string> BuildParamMappings()
        {
            Dictionary<string, string> calParams = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            calParams.Add("DTSTART", "StartDateTime");
            calParams.Add("DTEND", "EndDateTime");
            calParams.Add("SUMMARY", "Name");
            calParams.Add("LOCATION", "Location");
            calParams.Add("DESCRIPTION", "Description");
            calParams.Add("DETAILS", "Details");
            calParams.Add("CLASS", "Type");
            
            return calParams;
        }
    }
}
