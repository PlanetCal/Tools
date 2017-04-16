using System;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Client;

namespace WikiDescription
{
    internal class WikiClient
    {
        private WikiClientLibrary.Client.WikiClient wikiClient;
        private Site site;
        private const string WikiEndpoint = @"https://en.wikipedia.org/w/api.php";

        public WikiClient()
        {
            this.wikiClient = new WikiClientLibrary.Client.WikiClient();
        }

        public Site Site
        {
            get
            {
                return this.site;
            }
        }
        public async Task Connect()
        {
            this.site = await Site.CreateAsync(wikiClient, WikiClient.WikiEndpoint);
        }
    }
}
