using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Json;
namespace EventDataGenerator
{
    class Program
    {
        private static int EventDataPerDay = 10;
        static void Main(string[] args)
        {
            Console.WriteLine("events.json will be written as the out put file. In the same directory");

            string outputPath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            outputPath = Path.Combine(outputPath, "events.json");

            // Generate data for 1 year
            int[] monthDays = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            string yeardata = "[";
            for (int mo = 1; mo < 13; mo++)
            {
                if (mo > 1)
                {
                    yeardata += ",";
                }
                string monthdata = "[";
                for (int i = 0; i < monthDays[mo-1]; i++)
                {
                    if (i > 0)
                    {
                        monthdata += ",";
                    }
                    string eventdata = "[";
                    for (int num = 1; num <= EventDataPerDay; num++)
                    {
                        if(num > 1)
                        {
                            eventdata += ",";
                        }
                        eventdata += "{";
                        eventdata += "\"" + "id" + "\"" + ":" + "\"" + num.ToString() + "\",";
                        eventdata += "\"" + "title" + "\"" + ":" + "\"" + "Month " + num + " event name " + num.ToString() + "\",";
                        eventdata += "\"" + "venue" + "\"" + ":" + "\"" + "Sample venue " + num.ToString() + "\",";
                        eventdata += "\"" + "image" + "\"" + ":" + "\"" + "/images/placeholder.png" + "\",";
                        eventdata += "\"" + "time" + "\"" + ":" + "\"" + num.ToString() + " PM" + "\",";
                        eventdata += "\"" + "attendees" + "\"" + ":" + "\"" + new Random(2000).Next().ToString() + "\"";
                        eventdata += "}";

                    }
                    eventdata += "]";
                    monthdata += "{";
                    monthdata += "\"" + (i+1).ToString() + "\"" + ":" + eventdata;
                    monthdata += "}";
                }

                yeardata += "{";
                yeardata += "\"" + mo.ToString() + "\"" + ":" + monthdata;
                yeardata += "}";
            }

            string json = JsonConvert.SerializeObject(yeardata, Formatting.Indented);

            File.WriteAllText(outputPath, yeardata);
        }
    }
}


