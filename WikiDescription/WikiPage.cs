using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WikiClientLibrary;
using WikiClientLibrary.Client;
using HtmlAgilityPack;
using System.Globalization;

namespace WikiDescription
{
    internal class WikiPage
    {
        private string topicName;
        private Site site;
        private const int fullrefLength = 6;
        private const int literefLength = 2;

        public WikiPage(Site site, string topicName)
        {
            this.site = site;

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            this.topicName = textInfo.ToTitleCase(topicName);
        }

        public async Task<string> GetDescription()
        {
            Page page = null;

            var retryCount = 0;
            while (retryCount < 3)
            {
                page = new Page(this.site, this.topicName);
                await page.RefreshAsync(PageQueryOptions.FetchContent);
                if (((string.IsNullOrWhiteSpace(page.Content) == false) && (page.Content.StartsWith("#REDIRECT"))))
                {
                    var startIndex = page.Content.IndexOf("[[");
                    var endIndex = page.Content.IndexOf("]]");

                    this.topicName = page.Content.Substring(startIndex+2, (endIndex - startIndex-2));
                }
                else
                {
                    break;
                }

                retryCount++;
            }

            if (string.IsNullOrWhiteSpace(page.Content))
            {
                return string.Empty;
            }

            var paragraphs = page.Content.Split('\n');
            StringBuilder titleText = new StringBuilder();

            var paragraphCount = 0;
            foreach (var paragraph in paragraphs)
            {
                if (string.IsNullOrEmpty(paragraph) || 
                    paragraph.StartsWith("|") || 
                    paragraph.StartsWith("{{") || 
                    paragraph.StartsWith("[[") || 
                    paragraph.StartsWith("}}") || 
                    paragraph.StartsWith("==") ||
                    paragraph.StartsWith("-->") ||
                    paragraph.StartsWith("<!---"))
                {
                    continue;
                }

                var paragraphClean = paragraph;
                while (paragraphClean.Contains("<ref"))
                {
                    var refLength = WikiPage.fullrefLength;
                    var refStart = paragraphClean.IndexOf("<ref");
                    var refEnd = paragraphClean.IndexOf("</ref>");
                    var refEnd2 = paragraphClean.IndexOf("/>");
                    if (refEnd2 > 0)
                    {
                        refLength = WikiPage.literefLength;
                        refEnd = refEnd2;
                    }

                    if (refEnd >= 0)
                    {
                        paragraphClean = paragraphClean.Replace(paragraphClean.Substring(refStart, (refEnd - refStart + refLength)), "");
                    }
                }

                paragraphClean = WikiPage.ExtractTextFromHtml(paragraphClean);

                string braketPattern = @"\[\[(.*?)\]\]";
                Regex braketRegex = new Regex(braketPattern);

                string key = string.Empty;
                string keyValue = string.Empty;
                Dictionary<string, string> stringKeyPairs = new Dictionary<string, string>();
                MatchCollection braketMatches = braketRegex.Matches(paragraphClean);
                foreach (Match m in braketMatches)
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
                paragraphClean = paragraphClean.Replace("''", "");
                paragraphClean = paragraphClean.Replace("[[", "");
                paragraphClean = paragraphClean.Replace("]]", "");

                string curlyPattern = @"{{(.*?)}}";
                Regex curlyRegex = new Regex(curlyPattern);

                MatchCollection curlyMatches = curlyRegex.Matches(paragraphClean);
                foreach (Match m in curlyMatches)
                {
                    paragraphClean = paragraphClean.Replace(m.Value, "");
                }

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

        public static string ExtractTextFromHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return string.Empty;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var chunks = new List<string>();

#pragma warning disable CS0618 // Type or member is obsolete
            foreach (var item in doc.DocumentNode.DescendantNodesAndSelf())
#pragma warning restore CS0618 // Type or member is obsolete
            {
                if (item.NodeType == HtmlNodeType.Text)
                {
                    if (item.InnerText.Trim() != "")
                    {
                        chunks.Add(item.InnerText.Trim());
                    }
                }
            }
            return String.Join(" ", chunks);
        }
    }
}
