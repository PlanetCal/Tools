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
                    HeaderTabCommaCalendar HTCC = new HeaderTabCommaCalendar(fi.FullName);
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
