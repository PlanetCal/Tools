using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Client;

namespace WikiDescription
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var topic = "new year";
                if (args.Length >= 1)
                {
                    topic = args[0];
                }

                Console.WriteLine("{0} :- {1}", topic, GetTopicDescription(topic));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        static string GetTopicDescription(string topicName)
        {
            WikiClient wcc = new WikiClient();
            wcc.Connect().Wait();

            WikiPage wcp = new WikiPage(wcc.Site, topicName);

            Task<string> task = Task.Run(async () => await wcp.GetDescription());
            task.Wait();
            return task.Result;
        }
    }
}
