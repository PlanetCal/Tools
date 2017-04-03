using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSToPlanetCal
{
    class Program
    {
        private const string Holiday = "Holiday";

        static void Main(string[] args)
        {
            if (args.Length <=0)
            {
                Console.WriteLine("Please specify a directory.", args[0]);
                Console.WriteLine("Example: ICSToPlanetCal C:\\ICSFilesDirectory.");
                return;
            }

            if (Directory.Exists(args[0]))
            {
                List<CalendarBase> calenders = new List<CalendarBase>();

                DirectoryInfo di = new DirectoryInfo(args[0]);

                // Process Header Tab, Comma separated files
                foreach (FileInfo fi in di.GetFiles("*.txt"))
                {
                    string[] countrySplits = fi.Name.Split(new char[] { ' ', '.' });
                    var country = string.Empty;
                    foreach (string countrySplit in countrySplits)
                    {
                        try
                        {
                            if (ISO3166.FromName(countrySplit) != null)
                            {
                                country = ISO3166.FromName(countrySplit).Name;
                                break;
                            }
                        }
                        catch (ArgumentException)
                        {
                            country = string.Empty;
                        }
                    }

                    HeaderTabCommaCalendar HTCC = new HeaderTabCommaCalendar(fi.FullName);
                    HTCC.Location = country;
                    calenders.Add(HTCC);
                }

                // Process ICS Files
                foreach (FileInfo fi in di.GetFiles("*.ics"))
                {
                    OutlookCalendar oc = new OutlookCalendar(File.ReadAllText(fi.FullName));
                    calenders.Add(oc);
                }

                List<EventJson> eventsList = new List<EventJson>();
                foreach(CalendarBase cb in calenders)
                {
                    foreach(BaseEvent eb in cb.Events)
                    {
                        EventJson ej = new EventJson();
                        ej.Name = eb.ContentLines["SUMMARY"].Value;
                        ej.Location = cb.Location;
                        ej.Id = Guid.NewGuid().ToString();
                        ej.Type = eb.ContentLines.ContainsKey("CLASS") ? eb.ContentLines["CLASS"].Value : string.Empty;
                        if (string.IsNullOrWhiteSpace(ej.Type))
                        {
                            if (ej.Name.Contains(Program.Holiday) || (ej.Description.Contains(Program.Holiday)))
                            {
                                ej.Type = Program.Holiday;
                            }
                        }

                        ej.StartDateTime = eb.ContentLines["DTSTART"].Value;
                        ej.EndDateTime = eb.ContentLines.ContainsKey("DTEND") ? eb.ContentLines["DTEND"].Value : eb.ContentLines["DTSTART"].Value;
                        ej.Details = eb.ContentLines.ContainsKey("Details") ? eb.ContentLines["Details"].Value : string.Empty;
                        ej.Description = eb.ContentLines.ContainsKey("DESCRIPTION") ? eb.ContentLines["DESCRIPTION"].Value : string.Empty;

                        eventsList.Add(ej);
                    }
                }

                string output = JsonConvert.SerializeObject(eventsList, Formatting.Indented);
            }
            else
            {
                Console.WriteLine("The specified directory {0} doesn't exist", args[0]);
                Console.WriteLine("Example: ICSToPlanetCal C:\\ICSFilesDirectory.");
            }
        }
    }
}
