using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Client;

namespace WikiDescription
{
    internal class WikiPage
    {
        private string topicName;
        private Site site;

        public WikiPage(Site site, string topicName)
        {
            this.site = site;
            this.topicName = topicName;
        }

        public async Task<string> GetDescription()
        {
            var page = new Page(this.site, this.topicName);
            await page.RefreshAsync(PageQueryOptions.FetchContent);

            if (string.IsNullOrWhiteSpace(page.Content) || page.Content.StartsWith("#REDIRECT"))
            {
                return string.Empty;
            }

            var paragraphs = page.Content.Split('\n');
            StringBuilder titleText = new StringBuilder();

            var paragraphCount = 0;
            foreach (var paragraph in paragraphs)
            {
                if (string.IsNullOrEmpty(paragraph) || paragraph.StartsWith("{{") || paragraph.StartsWith("[[") || paragraph.StartsWith("=="))
                {
                    continue;
                }

                var refStart = paragraph.IndexOf("<ref>");
                var paragraphClean = string.Empty;
                if (refStart > 0)
                {
                    var refEnd = paragraph.IndexOf("</ref>");
                    paragraphClean = paragraph.Replace(paragraph.Substring(refStart, (refEnd - refStart + 6)), "");
                }
                else
                {
                    paragraphClean = paragraph;
                }

                string regularExpressionPattern = @"\[\[(.*?)\]\]";
                System.Text.RegularExpressions.Regex re = new Regex(regularExpressionPattern);

                string key = string.Empty;
                string keyValue = string.Empty;
                Dictionary<string, string> stringKeyPairs = new Dictionary<string, string>();
                MatchCollection mc = re.Matches(paragraphClean);
                foreach (Match m in mc)
                {
                    if (m.Value.Contains("|"))
                    {
                        string[] splitStrings = m.Value.Split('|');
                        keyValue = splitStrings[1].TrimEnd(']');
                        stringKeyPairs.Add(m.Value, keyValue);
                    }
                }

                foreach (var stringKey in stringKeyPairs.Keys)
                {
                    paragraphClean = paragraphClean.Replace(stringKey, stringKeyPairs[stringKey]);
                }

                while (paragraphClean.Contains("  "))
                {
                    paragraphClean = paragraphClean.Replace("  ", " ");
                }

                paragraphClean = paragraphClean.Replace("'''", "");
                paragraphClean = paragraphClean.Replace("[[", "");
                paragraphClean = paragraphClean.Replace("]]", "");

                titleText.Append(paragraphClean);
                paragraphCount++;
                if (paragraphCount == 2)
                {
                    break;
                }

                titleText.Append(" ");
            }

            return titleText.ToString();
        }
    }
}
