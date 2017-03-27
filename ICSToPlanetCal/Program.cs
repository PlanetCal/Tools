using ICSToPlanetCal.ParserModels.TabCommas;
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
                }

                // Process ICS Files
                foreach (FileInfo fi in di.GetFiles("*.ics"))
                {
                    Calendar c = new Calendar(File.ReadAllText(fi.FullName));
                }
            }
            else
            {
                Console.WriteLine("The specified directory {0} doesn't exist", args[0]);
                Console.WriteLine("Example: ICSToPlanetCal C:\\ICSFilesDirectory.");
            }
        }
    }
}
