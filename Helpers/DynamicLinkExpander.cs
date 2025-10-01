using Sitecore.Common;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;
using Sitecore.Sites;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace TinyMCERTE.Helpers {
    public class DynamicLinkExpander {

        public static string ExpandURL(Guid itemId, SiteContext siteContext) {
            using (new SiteContextSwitcher(siteContext)) {
                // Get the context database (usually "master" or "web")  
                Database database = siteContext.Database;

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

        public static string ExpandImageURL(Guid itemId, SiteContext siteContext) {
            // Get the context database (usually "master" or "web")  
            Database database = siteContext.Database;

            // Retrieve the item by ID  
            Item item = database.GetItem(itemId.ToID());

            // Create MediaItem from the item  
            MediaItem mediaItem = new MediaItem(item);

            // Setup MediaUrlBuilderOptions as needed (e.g., absolute URL, etc.)  
            var options = new MediaUrlBuilderOptions {
                AbsolutePath = false,               // set to true if you want absolute URL  
                AlwaysIncludeServerUrl = false,     // true to include scheme and hostname  
                LanguageEmbedding = LanguageEmbedding.Never
            };

            var url = MediaManager.GetMediaUrl(mediaItem, options);

            return url;
        }

        public static string ProcessHtml(string html, SiteContext siteContext) {
            if (string.IsNullOrEmpty(html))
                return html;

            // Regex to find href="..."; non-greedy to handle multiple attributes  
            // Capture the quote type (single or double) for proper replacement  
            string hrefPattern = @"href\s*=\s*([""'])(.*?)\1";

            // Regex to find GUID (with or without braces)  
            Regex guidRegex = new Regex(@"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.IgnoreCase);

            // Regex to match /media/{32 hex chars}.ashx pattern and capture the 32 hex chars  
            Regex mediaGuidRegex = new Regex(@"^[-]?/media/([0-9a-fA-F]{32})\.ashx$", RegexOptions.IgnoreCase);

            string result = Regex.Replace(html, hrefPattern, match =>
            {
                string quote = match.Groups[1].Value;
                string hrefValue = match.Groups[2].Value;

                // Decode HTML entities in href  
                string decodedHref = HttpUtility.HtmlDecode(hrefValue);

                // Try to parse _id parameter from href query string first  
                int queryStart = decodedHref.IndexOf('?');
                if (queryStart >= 0) {
                    string queryString = decodedHref.Substring(queryStart + 1);
                    var queryParams = HttpUtility.ParseQueryString(queryString);

                    string idParam = queryParams["_id"];
                    if (!string.IsNullOrEmpty(idParam)) {
                        // Clean idParam from braces if any  
                        string cleanId = idParam.Trim('{', '}');

                        if (Guid.TryParse(cleanId, out Guid itemId)) {
                            string expandedUrl = ExpandURL(itemId, siteContext);
                            return $"href={quote}{expandedUrl}{quote}";
                        }
                    }
                }

                // If no _id param or invalid, check if href matches /media/{32hex}.ashx pattern  
                var mediaMatch = mediaGuidRegex.Match(decodedHref);
                if (mediaMatch.Success) {
                    string hex32 = mediaMatch.Groups[1].Value;

                    // Convert 32 hex chars into a GUID string with dashes  
                    // GUID format: 8-4-4-4-12  
                    string guidStr = $"{hex32.Substring(0, 8)}-{hex32.Substring(8, 4)}-{hex32.Substring(12, 4)}-{hex32.Substring(16, 4)}-{hex32.Substring(20, 12)}";

                    if (Guid.TryParse(guidStr, out Guid mediaGuid)) {
                        string expandedUrl = ExpandImageURL(mediaGuid, siteContext);
                        return $"href={quote}{expandedUrl}{quote}";
                    }
                }

                // No matching pattern, return original  
                return match.Value;

            }, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return result;
        }
    }
}
