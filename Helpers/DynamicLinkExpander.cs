using Sitecore.Common;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Sites;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace TinyMCERTE.Helpers {
    public class DynamicLinkExpander {

        public static string ExpandURL(Guid itemId, SiteContext siteContext) {
            using (new SiteContextSwitcher(siteContext)) {
                // Get the context database (usually "master" or "web")  
                Database database = Sitecore.Context.Database ?? Sitecore.Configuration.Factory.GetDatabase("master");

                // Retrieve the item by ID  
                Item item = database.GetItem(itemId.ToID());

                var opts = LinkManager.GetDefaultUrlBuilderOptions();
                opts.SiteResolving = true;
                opts.LanguageEmbedding = LanguageEmbedding.Never;   // tweak as needed
                opts.AlwaysIncludeServerUrl = false;              // for absolute URLs

                var url = LinkManager.GetItemUrl(item, opts);

                return url;
            }
        }

        public static string ProcessHtml(string html, SiteContext siteContext) {

            if (string.IsNullOrEmpty(html))
                return html;

            // Regex to find href="..."; non-greedy to handle multiple attributes  
            // Capture the quote type (single or double) for proper replacement  
            string hrefPattern = @"href\s*=\s*([""'])(.*?)\1";

            // Regex to find GUID (with or without braces)  
            Regex guidRegex = new Regex(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}");

            // Use Regex.Replace with a MatchEvaluator to process each href attribute  
            string result = Regex.Replace(html, hrefPattern, match =>
            {
                string quote = match.Groups[1].Value;
                string hrefValue = match.Groups[2].Value;

                // Decode HTML entities in href  
                string decodedHref = HttpUtility.HtmlDecode(hrefValue);

                // Try to parse _id parameter from href query string  
                int queryStart = decodedHref.IndexOf('?');
                if (queryStart < 0) {
                    // No query string, skip  
                    return match.Value;
                }

                string queryString = decodedHref.Substring(queryStart + 1);
                var queryParams = HttpUtility.ParseQueryString(queryString);

                string idParam = queryParams["_id"];
                if (string.IsNullOrEmpty(idParam)) {
                    // No _id param, skip  
                    return match.Value;
                }

                // Clean idParam from braces if any  
                string cleanId = idParam.Trim('{', '}');

                if (Guid.TryParse(cleanId, out Guid itemId)) {
                    // Call ExpandURL with the parsed GUID  
                    string expandedUrl = ExpandURL(itemId, siteContext);

                    // Return href attribute with replaced URL, preserving original quote style  
                    return $"href={quote}{expandedUrl}{quote}";
                } else {
                    // _id param is not a valid GUID, skip  
                    return match.Value;
                }
            }, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return result;
        }
    }
}
