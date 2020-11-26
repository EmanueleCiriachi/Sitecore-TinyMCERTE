using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.WordOCX;
using System;
using System.Web;
using Sitecore.Shell.Applications.ContentEditor;

namespace TinyMCERTE
{
    public class RTETinyEditor : RichText {
        /// <summary>The handle.</summary>
        private string handle;
        /// <summary>The item version.</summary>
        private string itemVersion;
        /// <summary>The set value on pre render.</summary>
        private bool setValueOnPreRender;

        public RTETinyEditor() {
            this.Class = "scContentControlHtml2";
            this.Activation = true;
            this.Attributes["tabindex"] = "-1";
        }

        /// <summary>Gets or sets the field ID.</summary>
        /// <value>The field ID.</value>
        public string FieldID {
            get {
                return this.GetViewStateString(ExtensionMethods.nameof(() => FieldID));
            }
            set {
                Assert.ArgumentNotNull((object)value, ExtensionMethods.nameof(() => value));
                this.SetViewStateString(ExtensionMethods.nameof(() => FieldID), value);
            }
        }

        /// <summary>Gets or sets the item ID.</summary>
        /// <value>The item ID.</value>
        public string ItemID {
            get {
                return this.GetViewStateString(ExtensionMethods.nameof(() => ItemID));
            }
            set {
                Assert.ArgumentNotNull((object)value, ExtensionMethods.nameof(() => value));
                this.SetViewStateString(ExtensionMethods.nameof(() => ItemID), value);
            }
        }

        /// <summary>Gets or sets the item language.</summary>
        /// <value>The item language.</value>
        public string ItemLanguage {
            get {
                return this.GetViewStateString(ExtensionMethods.nameof(() => ItemLanguage));
            }
            set {
                Assert.ArgumentNotNull((object)value, ExtensionMethods.nameof(() => value));
                this.SetViewStateString(ExtensionMethods.nameof(() => ItemLanguage), value);
            }
        }

        /// <summary>Gets or sets the item version.</summary>
        /// <value>The item version.</value>
        public string ItemVersion {
            get {
                return this.itemVersion;
            }
            set {
                Assert.ArgumentNotNull((object)value, ExtensionMethods.nameof(() => value));
                this.itemVersion = value;
            }
        }

        /// <summary>Gets or sets the source.</summary>
        /// <value>The source.</value>
        public string Source {
            get {
                return this.GetViewStateString(ExtensionMethods.nameof(() => Source));
            }
            set {
                Assert.ArgumentNotNull((object)value, ExtensionMethods.nameof(() => value));
                this.SetViewStateString(ExtensionMethods.nameof(() => Source), value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is tracking modified.
        /// </summary>
        /// <value><c>true</c> if  this instance is tracking modified; otherwise, <c>false</c>.</value>
        public bool TrackModified {
            get {
                return this.GetViewStateBool(ExtensionMethods.nameof(() => TrackModified), true);
            }
            set {
                this.SetViewStateBool(ExtensionMethods.nameof(() => TrackModified), value, true);
            }
        }

        /// <summary>Handles the message.</summary>
        /// <param name="message">The message.</param>
        public override void HandleMessage(Message message) {
            Assert.ArgumentNotNull((object)message, ExtensionMethods.nameof(() => message));
            base.HandleMessage(message);
            if (!(message["id"] == this.ID))
                return;
            switch (message.Name) {
                case "tinyrte:edit":
                    Sitecore.Context.ClientPage.Start((object)this, "EditHtmlTinyMCE");
                    break;
            }
        }

        /// <summary>Edits the text.</summary>
        /// <param name="args">The args.</param>
        protected void EditHtmlTinyMCE(ClientPipelineArgs args) {
            Assert.ArgumentNotNull((object)args, ExtensionMethods.nameof(() => args));
            if (this.Disabled)
                return;
            if (args.IsPostBack) {
                if (args.Result == null || !(args.Result != "undefined"))
                    return;
                this.UpdateHtml(args);
            } else {
                TinyMCEEditorUrl richTextEditorUrl = new TinyMCEEditorUrl() {
                    Conversion = TinyMCEEditorUrl.HtmlConversion.DoNotConvert,
                    Disabled = this.Disabled,
                    FieldID = this.FieldID,
                    ID = this.ID,
                    ItemID = this.ItemID,
                    Language = this.ItemLanguage,
                    Mode = string.Empty,
                    ShowInFrameBasedDialog = true,
                    Source = this.Source,
                    Url = "/sitecore/shell/Controls/TinyMCE Editor/EditorPage.aspx",
                    Value = this.Value,
                    Version = this.ItemVersion
                };
                UrlString url = richTextEditorUrl.GetUrl();
                this.handle = richTextEditorUrl.Handle;
                SheerResponse.ShowModalDialog(new ModalDialogOptions(url.ToString()) {
                    Width = "1200",
                    Height = "730px",
                    Response = true,
                    Header = Translate.Text("Rich Text Editor")
                });
                args.WaitForPostBack();
            }
        }

        /// <summary>Loads the post data.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The load post data.</returns>
        protected override bool LoadPostData(string value)
        {
            base.LoadPostData(value);
            Assert.ArgumentNotNull((object)value, ExtensionMethods.nameof(() => value));
            if (!(value != this.Value))
                return false;
            this.Value = value;
            return true;
        }

        /// <summary>Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.</summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e) {
            Assert.ArgumentNotNull((object)e, ExtensionMethods.nameof(() => e));
            base.OnLoad(e);
            if (Sitecore.Context.ClientPage.IsEvent)
                return;
            TinyMCEEditorUrl tinyMCEEditorUrl = new TinyMCEEditorUrl() {
                Conversion = TinyMCEEditorUrl.HtmlConversion.DoNotConvert,
                Disabled = this.Disabled,
                FieldID = this.FieldID,
                ID = this.ID,
                ItemID = this.ItemID,
                Language = this.ItemLanguage,
                Mode = "ContentEditor",
                ShowInFrameBasedDialog = true,
                Source = this.Source,
                Url = string.Empty,
                Value = this.Value,
                Version = this.ItemVersion
            };
            UrlString url = tinyMCEEditorUrl.GetUrl();
            this.handle = tinyMCEEditorUrl.Handle;
            this.setValueOnPreRender = true;
            this.SourceUri = url.ToString();
        }

        protected override void OnPreRender(EventArgs e) {
            Assert.ArgumentNotNull((object)e, ExtensionMethods.nameof(() => e));
            base.OnPreRender(e);
            if (this.setValueOnPreRender)
                HttpContext.Current.Session[this.handle] = (object)this.Value;
            this.ServerProperties["ItemLanguage"] = this.ServerProperties["ItemLanguage"];
            this.ServerProperties["Source"] = this.ServerProperties["Source"];
            this.ServerProperties["FieldID"] = this.ServerProperties["FieldID"];
        }

        /// <summary>Sets the modified.</summary>
        protected virtual void SetModified() {
            if (!this.TrackModified)
                return;
            Sitecore.Context.ClientPage.Modified = true;
            SheerResponse.Eval("scContent.startValidators()");
        }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public override string Value {
            get {
                WordFieldValue wordFieldValue = WordFieldValue.Parse(base.Value);
                if (wordFieldValue.BlobId != Sitecore.Data.ID.Null)
                    return wordFieldValue.GetHtmlWithStyles();
                return base.Value;
            }
            set {
                base.Value = value;
            }
        }

        /// <summary>Updates the HTML.</summary>
        /// <param name="args">The arguments.</param>
        protected virtual void UpdateHtml(ClientPipelineArgs args) {
            Assert.ArgumentNotNull((object)args, ExtensionMethods.nameof(() => args));
            string str = args.Result;
            if (str == "__#!$No value$!#__")
                str = string.Empty;
            string text = this.ProcessValidateScripts(str);
            if (text != this.Value)
                this.SetModified();
            SheerResponse.Eval("scForm.browser.getControl('" + this.ID + "').contentWindow.scSetText(" + StringUtil.EscapeJavascriptString(text) + ")");
            SheerResponse.Eval("scContent.startValidators()");
        }

        /// <summary>Processes the validate scripts.</summary>
        /// <param name="value">The value.</param>
        /// <returns>Result of the value.</returns>
        protected string ProcessValidateScripts(string value) {
            if (Settings.HtmlEditor.RemoveScripts) {
                value = WebUtil.RemoveInlineScripts(value);
            }
            return value;
        }

    }
}
