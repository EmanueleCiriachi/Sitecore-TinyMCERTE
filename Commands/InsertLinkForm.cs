// Decompiled with JetBrains decompiler
// Type: Sitecore.Shell.Controls.RichTextEditor.InsertLink.InsertLinkForm
// Assembly: Sitecore.Client, Version=8.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D00686D3-8624-4B6A-8F9D-0007A3F21CE2
// Assembly location: C:\Ecosystem\emanuele.ciriachi\BlueRubicon.Web\bin\Sitecore.Client.dll

using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Shell.Controls.RichTextEditor.InsertLink;
using Sitecore.Web.UI.Sheer;
using System;

namespace TinyMCERTE.Commands {
    /// <summary>Represents a InsertLinkForm.</summary>
    public class InsertTinyMCELinkForm : InsertLinkForm {

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
            string displayName;
            string text;
            if (this.Tabs.Active == 0 || this.Tabs.Active == 2) {
                Item selectionItem = this.InternalLinkTreeview.GetSelectionItem();
                if (selectionItem == null) {
                    SheerResponse.Alert("Select an item.");
                    return;
                }
                displayName = selectionItem.DisplayName;
                if (selectionItem.Paths.IsMediaItem) {
                    text = this.GetMediaUrl(selectionItem);
                } else {
                    if (!selectionItem.Paths.IsContentItem) {
                        SheerResponse.Alert("Select either a content item or a media item.");
                        return;
                    }
                    LinkUrlOptions options = new LinkUrlOptions();
                    text = LinkManager.GetDynamicUrl(selectionItem, options);
                }
            } else {
                MediaItem selectionItem = (MediaItem)this.MediaTreeview.GetSelectionItem();
                if (selectionItem == null) {
                    SheerResponse.Alert("Select a media item.");
                    return;
                }
                displayName = selectionItem.DisplayName;
                text = this.GetMediaUrl((Item)selectionItem);
            }
            if (this.Mode == "webedit") {
                SheerResponse.SetDialogValue(StringUtil.EscapeJavascriptString(text));
                base.OnOK(sender, args);
            } else
                SheerResponse.Eval("TinyMCEEditor.InsertLink.scClose(" + StringUtil.EscapeJavascriptString(text) + "," + StringUtil.EscapeJavascriptString(displayName) + ")");
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
                SheerResponse.Eval("TinyMCEEditor.InsertLink.scCancel()");
        }

        /// <summary>Gets the media URL.</summary>
        /// <param name="item">The item.</param>
        /// <returns>The media URL.</returns>
        /// <contract>
        ///   <requires name="item" condition="not null" />
        ///   <ensures condition="not null" />
        /// </contract>
        private string GetMediaUrl(Item item) {
            Assert.ArgumentNotNull((object)item, ExtensionMethods.nameof(() => item));
            return MediaManager.GetMediaUrl((MediaItem)item, MediaUrlOptions.GetShellOptions());
        }

        /// <summary>Called when media tree view clicked.</summary>
        private void OnMediaTreeviewClicked() {
            this.SetUploadButtonAvailability();
        }

        /// <summary>Called when active tab changed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="T:System.EventArgs" /> instance containing the event data.</param>
        private void OnActiveTabChanged(object sender, EventArgs args) {
            this.SetUploadButtonAvailability();
        }

        /// <summary>Sets the upload button availability.</summary>
        private void SetUploadButtonAvailability() {
            if (this.Tabs.Active == 1) {
                SheerResponse.Eval("document.getElementById('BtnUpload').style.display='';");
                Item selectionItem = this.MediaTreeview.GetSelectionItem();
                if (selectionItem != null && selectionItem.Access.CanCreate())
                    SheerResponse.Eval("document.getElementById('BtnUpload').disabled = false;");
                else
                    SheerResponse.Eval("document.getElementById('BtnUpload').disabled = true;");
            } else
                SheerResponse.Eval("document.getElementById('BtnUpload').style.display='none';");
        }
    }
}
