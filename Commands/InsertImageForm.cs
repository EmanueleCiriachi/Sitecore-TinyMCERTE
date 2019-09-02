using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Resources.Media;
using Sitecore.Shell.Controls.RichTextEditor.InsertImage;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Specialized;
using System.Web;

namespace TinyMCERTE.Commands {
    public class InsertTinyMCEImageForm : InsertImageForm {
        /// <summary>Handles a click on the OK button.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        /// <remarks>
        /// When the user clicks OK, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
        /// </remarks>
        protected override void OnOK(object sender, EventArgs args) {
            Assert.ArgumentNotNull(sender, ExtensionMethods.nameof(() => sender));
            Assert.ArgumentNotNull((object)args, ExtensionMethods.nameof(() => args));
            string str = this.Filename.Value;
            if (str.Length == 0) {
                SheerResponse.Alert("Select a media item.");
            } else {
                Item root = this.DataContext.GetRoot();
                if (root != null) {
                    Item rootItem = root.Database.GetRootItem();
                    if (rootItem != null && root.ID != rootItem.ID)
                        str = FileUtil.MakePath(root.Paths.Path, str, '/');
                }
                MediaItem mediaItem = (MediaItem)this.DataContext.GetItem(str, this.ContentLanguage, Sitecore.Data.Version.Latest);
                if (mediaItem == null)
                    SheerResponse.Alert("The media item could not be found.");
                else if (!(MediaManager.GetMedia(MediaUri.Parse((Item)mediaItem)) is ImageMedia)) {
                    SheerResponse.Alert("The selected item is not an image. Select an image to continue.");
                } else {
                    MediaUrlOptions shellOptions = MediaUrlOptions.GetShellOptions();
                    shellOptions.Language = this.ContentLanguage;
                    string text = !string.IsNullOrEmpty(HttpContext.Current.Request.Form["AlternateText"]) ? HttpContext.Current.Request.Form["AlternateText"] : mediaItem.Alt;
                    Tag image = new Tag("img");
                    this.SetDimensions(mediaItem, shellOptions, image);
                    image.Add("Src", MediaManager.GetMediaUrl(mediaItem, shellOptions));
                    image.Add("Alt", StringUtil.EscapeQuote(text));
                    image.Add("_languageInserted", "true");
                    if (this.Mode == "webedit") {
                        SheerResponse.SetDialogValue(StringUtil.EscapeJavascriptString(image.ToString()));
                        base.OnOK(sender, args);
                    } else
                        SheerResponse.Eval("TinyMCEEditor.InsertImage.scClose(" + StringUtil.EscapeJavascriptString(image.ToString()) + ")");
                }
            }
        }

        /// <summary>Handles a click on the Cancel button.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        /// <remarks>When the user clicksCancel, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.</remarks>
        protected override void OnCancel(object sender, EventArgs args) {
            Assert.ArgumentNotNull(sender, ExtensionMethods.nameof(() => sender));
            Assert.ArgumentNotNull((object)args, ExtensionMethods.nameof(() => args));
            if (this.Mode == "webedit")
                base.OnCancel(sender, args);
            else
                SheerResponse.Eval("TinyMCEEditor.InsertImage.scCancel()");
        }

        /// <summary>Gets the dimensions.</summary>
        /// <param name="item">The item.</param>
        /// <param name="options">The options.</param>
        /// <param name="image">The image.</param>
        private void SetDimensions(MediaItem item, MediaUrlOptions options, Tag image) {
            Assert.ArgumentNotNull((object)item, ExtensionMethods.nameof(() => item));
            Assert.ArgumentNotNull((object)options, ExtensionMethods.nameof(() => options));
            Assert.ArgumentNotNull((object)image, ExtensionMethods.nameof(() => image));
            NameValueCollection form = HttpContext.Current.Request.Form;
            if (!string.IsNullOrEmpty(form["Width"]) && form["Width"] != item.InnerItem["Width"] && form["Height"] != item.InnerItem["Height"]) {
                int result1;
                if (int.TryParse(form["Width"], out result1)) {
                    options.Width = result1;
                    image.Add("width", result1.ToString());
                }
                int result2;
                if (!int.TryParse(form["Height"], out result2))
                    return;
                options.Height = result2;
                image.Add("height", result2.ToString());
            } else {
                image.Add("width", item.InnerItem["Width"]);
                image.Add("height", item.InnerItem["Height"]);
            }
        }
    }
}
