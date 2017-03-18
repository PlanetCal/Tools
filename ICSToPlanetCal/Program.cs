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
