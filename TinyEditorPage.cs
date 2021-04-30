using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Resources.Media;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.ContentEditor.RichTextEditor;
using Sitecore.Shell.Controls.RichTextEditor.Pipelines.LoadRichTextContent;
using Sitecore.Shell.Controls.RichTextEditor.Pipelines.SaveRichTextContent;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XamlSharp.Ajax;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TinyMCERTE {
    public class TinyEditorPage : System.Web.UI.Page {
        /// <summary>ScriptConstants control.</summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected PlaceHolder ScriptConstants;
        /// <summary>mainForm control.</summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected HtmlForm mainForm;
        /// <summary>EditorUpdatePanel control.</summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected UpdatePanel EditorUpdatePanel;
        /// <summary>OkButton control.</summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Sitecore.Web.UI.HtmlControls.Button OkButton;
        /// <summary>CancelButton control.</summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Sitecore.Web.UI.HtmlControls.Button CancelButton;
        /// <summary>
        /// The content of the editor, coming directly from the rich-text field
        /// </summary>
        protected System.Web.UI.HtmlControls.HtmlTextArea FieldText;
        /// <summary>The content of the "toolbar" configuration option in TinyMCE.</summary>
        protected HiddenField EditorToolbar;
        /// <summary>The content of the "plugins" configuration option in TinyMCE.</summary>
        protected HiddenField EditorPlugins;
        /// <summary>The javascript callback after TinyMCE is initialised.</summary>
        protected HiddenField EditorInitCallback;
        /// <summary>The content of the "menubar" configuration option in TinyMCE.</summary>
        protected HiddenField EditorMenubar;
        /// <summary>The content of the "branding" configuration option in TinyMCE.</summary>
        protected HiddenField EditorBranding;
        /// <summary>The content of the "style formats" configuration option in TinyMCE.</summary>
        protected HiddenField EditorStyleFormats;
        /// <summary>The CSS file to apply to the editor's content</summary>
        protected HiddenField CSSPath;
        

        /// <summary>Handles the Accept_ click event.</summary>
        protected void OnAccept() {
            SaveRichTextContentArgs richTextContentArgs = new SaveRichTextContentArgs(this.Request.Form["FieldText"]);
            richTextContentArgs.Content = WebEditUtil.RepairLinks(richTextContentArgs.Content);
            using (new LongRunningOperationWatcher(250, "saveRichTextContent", new string[0]))
                CorePipeline.Run("saveRichTextContent", (PipelineArgs)richTextContentArgs);
            if (!RichTextEditorUrl.Parse(this.Context.Request.RawUrl).ShowInFrameBasedDialog)
                SheerResponse.Eval(string.Format("scRichText.saveRichText({0})", (object)StringUtil.EscapeJavascriptString(richTextContentArgs.Content)));
            else
                SheerResponse.SetDialogValue(richTextContentArgs.Content);
            SheerResponse.Eval("EditorPage.scCloseEditor();");
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event to initialize the page.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs" /> that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            Sitecore.Client.AjaxScriptManager.OnExecute += new AjaxScriptManager.ExecuteDelegate(this.AjaxScriptManager_OnExecute);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs" /> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e) {
            User user = Sitecore.Context.User;
            if (!user.IsInRole(Constants.SitecoreClientUsersRole) && !user.IsAdministrator)
                WebUtil.RedirectToLoginPage();
            base.OnLoad(e);
            if (this.IsPostBack || string.IsNullOrEmpty(WebUtil.GetQueryString("hdl")))
                return;

            var configurationResult = Utils.LoadTinyEditorConfiguration();

            this.RegisterMediaPrefixes();
            
            this.EditorToolbar.Value = configurationResult.EditorToolbar;
            this.EditorPlugins.Value = configurationResult.EditorPlugins;
            this.EditorInitCallback.Value = configurationResult.EditorInitCallback;
            this.EditorMenubar.Value = configurationResult.EditorMenubar;
            this.EditorBranding.Value = configurationResult.EditorBranding;
            this.EditorStyleFormats.Value = configurationResult.EditorStyleFormats;
            this.CSSPath.Value = Sitecore.Configuration.Settings.GetSetting("WebStylesheet");

            this.RenderScriptConstants();
            this.LoadHtml();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreRenderComplete" /> event after the <see cref="M:System.Web.UI.Page.OnPreRenderComplete(System.EventArgs)" /> event and before the page is rendered.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnPreRenderComplete(EventArgs e) {
            base.OnPreRenderComplete(e);
            if (this.IsPostBack)
                return;
        }

        /// <summary>Called when [reject].</summary>
        protected void OnReject() {
            SheerResponse.Eval("EditorPage.scCloseEditor();");
        }

        /// <summary>
        /// Handles the OnExecute event of the AjaxScriptManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The arguments.</param>
        private void AjaxScriptManager_OnExecute(object sender, AjaxCommandEventArgs args) {
            if (args.Name == "editorpage:accept") {
                this.OnAccept();
            } else {
                if (args.Name != "editorpage:reject")
                    return;
                this.OnReject();
            }
        }

        /// <summary>Populates the editor with HTML</summary>
        private void LoadHtml() {
            string queryString = WebUtil.GetQueryString("hdl");
            Assert.IsNotNullOrEmpty(queryString, "html value handle");
            string sessionString = WebUtil.GetSessionString(queryString);
            WebUtil.RemoveSessionValue(queryString);
            LoadRichTextContentArgs richTextContentArgs = new LoadRichTextContentArgs(sessionString);
            using (new LongRunningOperationWatcher(250, "loadRichTextContent", new string[0]))
                CorePipeline.Run("loadRichTextContent", (PipelineArgs)richTextContentArgs);
            this.FieldText.InnerHtml = richTextContentArgs.Content;
        }

        /// <summary>Registers the media prefixes.</summary>
        private void RegisterMediaPrefixes() {
            StringBuilder stringBuilder = new StringBuilder();
            var mediaPrefixes = MediaManager.Config.MediaPrefixes;
            foreach (string mediaPrefix in mediaPrefixes)
                stringBuilder.Append("|" + mediaPrefix.Replace("\\", "\\\\").Replace("/", "\\/"));
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "mediaPrefixes", "var prefixes = '" + (object)stringBuilder + "';", true);
        }

        /// <summary>Renders the script constants.</summary>
        private void RenderScriptConstants() {
            this.ScriptConstants.Controls.Add((System.Web.UI.Control)new LiteralControl("\r\n            var scClientID = '{0}';\r\n            var scItemID = '{1}';\r\n            var scLanguage = '{2}';\r\n            var scDatabase = '{3}';\r\n            var scRemoveScripts = '{4}';\r\n         ".FormatWith("Client ID", (object)WebUtil.GetQueryString("id"), (object)WebUtil.GetQueryString("la"), (object)WebUtil.GetQueryString("sc_content"), (object)Settings.HtmlEditor.RemoveScripts.ToString().ToLowerInvariant())));
        }
    }
}
