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

            string planetCalParamName;
            paramMaps.TryGetValue(icsParam, out planetCalParamName);

            return planetCalParamName;
        }

        private static Dictionary<string, string> BuildParamMappings()
        {
            Dictionary<string, string> calParams = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            calParams.Add("DTSTART", "DateTime");
            calParams.Add("DTEND", "Duration");
            calParams.Add("SUMMARY", "Name");
            calParams.Add("LOCATION", "Location");
            calParams.Add("DESCRIPTION", "Description");
            calParams.Add("TYPE", "Type");
            calParams.Add("DETAILS", "Details");
            calParams.Add("CLASS", "Type");
            
            return calParams;
        }
    }
}
