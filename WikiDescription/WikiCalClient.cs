using System;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Client;

namespace WikiDescription
{
    internal class WikiCalClient
    {
        private WikiClient wikiClient;
        private Site site;
        private const string WikiEndpoint = @"https://en.wikipedia.org/w/api.php";

        public WikiCalClient()
        {
            this.wikiClient = new WikiClient();
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
            this.site = await Site.CreateAsync(wikiClient, WikiCalClient.WikiEndpoint);
        }
    }
}
