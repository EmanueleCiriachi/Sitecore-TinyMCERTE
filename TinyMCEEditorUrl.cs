using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Shell;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Web;

namespace TinyMCERTE {
    public class TinyMCEEditorUrl {
        /// <summary>Gets or sets the conversion.</summary>
        /// <value>The conversion.</value>
        public TinyMCEEditorUrl.HtmlConversion Conversion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Sitecore.Shell.Applications.ContentEditor.RichTextEditor.TinyMCEEditorUrl" /> is disabled.
        /// </summary>
        /// <value><c>true</c> if disabled; otherwise, <c>false</c>.</value>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Rich text editor should be started from page editor.
        /// </summary>
        /// <value><c>true</c> if page editor; otherwise, <c>false</c>.</value>
        public bool IsPageEdit { get; set; }

        /// <summary>Gets or sets the field ID.</summary>
        /// <value>The field ID.</value>
        public string FieldID { get; set; }

        /// <summary>Gets or sets the handle.</summary>
        /// <value>The handle.</value>
        public string Handle { get; set; }

        /// <summary>Gets or sets the ID.</summary>
        /// <value>The ID.</value>
        public string ID { get; set; }

        /// <summary>Gets or sets the item ID.</summary>
        /// <value>The item ID.</value>
        public string ItemID { get; set; }

        /// <summary>Gets or sets the language.</summary>
        /// <value>The language.</value>
        public string Language { get; set; }

        /// <summary>Gets or sets the mode.</summary>
        /// <value>The mode.</value>
        public string Mode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Rich text editor should be shown in a frame based modal dialog.
        /// </summary>
        /// <value><c>true</c> if Rich text editor should be shown in a frame based modal dialog; otherwise, <c>false</c>.</value>
        public bool ShowInFrameBasedDialog { get; set; }

        /// <summary>Gets or sets the source.</summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>Gets the URL.</summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>Gets or sets the version.</summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>Parse string</summary>
        /// <param name="urlString">The url String.</param>
        /// <returns>Initialized TinyMCEEditorUrl object</returns>
        public static TinyMCEEditorUrl Parse(string urlString) {
            Assert.ArgumentNotNull((object)urlString, ExtensionMethods.nameof(() => urlString));
            TinyMCEEditorUrl tinyMCEEditorUrl = new TinyMCEEditorUrl();
            UrlString urlString1 = new UrlString(urlString);
            tinyMCEEditorUrl.ItemID = StringUtil.GetString(new string[2]
      {
        urlString1["id"],
        string.Empty
      });
            tinyMCEEditorUrl.ID = StringUtil.GetString(new string[1]
      {
        urlString1["ed"]
      });
            tinyMCEEditorUrl.Version = StringUtil.GetString(new string[1]
      {
        urlString1["vs"]
      });
            tinyMCEEditorUrl.Language = StringUtil.GetString(new string[1]
      {
        urlString1["la"]
      });
            tinyMCEEditorUrl.FieldID = StringUtil.GetString(new string[1]
      {
        urlString1["fld"]
      });
            tinyMCEEditorUrl.Source = StringUtil.GetString(new string[1]
      {
        urlString1["so"]
      });
            tinyMCEEditorUrl.Disabled = urlString1["di"] == "1";
            tinyMCEEditorUrl.Handle = StringUtil.GetString(new string[1]
      {
        urlString1["hdl"]
      });
            tinyMCEEditorUrl.Mode = StringUtil.GetString(new string[1]
      {
        urlString1["mo"]
      });
            tinyMCEEditorUrl.IsPageEdit = urlString1["pe"] == "1";
            tinyMCEEditorUrl.ShowInFrameBasedDialog = urlString1["fbd"] == "1";
            return tinyMCEEditorUrl;
        }

        /// <summary>Gets the URL.</summary>
        /// <returns>The URL.</returns>
        /// <contract>
        ///  <requires name="url" condition="not empty" />
        ///  <requires name="mode" condition="not empty" />
        ///  <ensures condition="not null" />
        /// </contract>
        public UrlString GetUrl() {
            this.Handle = Control.GetUniqueID("H");
            NameValueCollection querystring = new NameValueCollection(1);
            if (string.IsNullOrEmpty(this.Url)) {
                if (this.Disabled || UserOptions.HtmlEditor.ContentEditorMode == UserOptions.HtmlEditor.Mode.Preview) {
                    this.Url = "/sitecore/shell/Controls/TinyMCE Editor/Preview.aspx";
                    querystring["sc_disableproperties"] = "1";
                } else
                    this.Url = "/sitecore/shell/Controls/TinyMCE Editor/Default.aspx";
            }
            UrlString url = new UrlString(this.Url);
            url["da"] = Context.Database.Name;
            url["id"] = StringUtil.GetString(new string[1]
      {
        this.ItemID
      });
            url["ed"] = StringUtil.GetString(new string[1]
      {
        this.ID
      });
            url["vs"] = StringUtil.GetString(new string[1]
      {
        this.Version
      });
            url["la"] = StringUtil.GetString(new string[1]
      {
        this.Language
      });
            url["fld"] = StringUtil.GetString(new string[1]
      {
        this.FieldID
      });
            url["so"] = StringUtil.GetString(new string[1]
      {
        this.Source
      });
            url["di"] = this.Disabled ? "1" : "0";
            url["hdl"] = this.Handle;
            url["mo"] = this.Mode;
            url["pe"] = this.IsPageEdit ? "1" : "0";
            url["fbd"] = this.ShowInFrameBasedDialog ? "1" : "0";
            UIUtil.AddContentDatabaseParameter(url);
            string body = this.Value;
            if (body == "__#!$No value$!#__")
                body = string.Empty;
            if (this.Conversion == TinyMCEEditorUrl.HtmlConversion.DesignTime) {
                querystring["sc_live"] = "0";
                string controlName = string.Empty;
                if (Settings.HtmlEditor.SupportWebControls)
                    controlName = "control:IDEHtmlEditorControl";
                body = DesignTimeHtml.Convert(body, controlName, querystring);
            } else if (this.Conversion == TinyMCEEditorUrl.HtmlConversion.Runtime)
                body = RuntimeHtml.Convert(body, Settings.HtmlEditor.SupportWebControls);
            HttpContext.Current.Session[this.Handle] = (object)body;
            return url;
        }

        /// <summary>The html conversion.</summary>
        public enum HtmlConversion {
            DoNotConvert,
            DesignTime,
            Runtime,
        }
    }
}
