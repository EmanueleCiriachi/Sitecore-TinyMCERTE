<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditorPage.aspx.cs" Inherits="TinyMCERTE.TinyEditorPage" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.HtmlControls" TagPrefix="sc" %>

<!DOCTYPE html>
<html style="overflow: hidden; width: 100%; height: 100%" lang="en">
<head runat="server">
  <title>Sitecore</title>
  <link href="/sitecore/shell/Themes/Standard/Default/Content Manager.css" rel="stylesheet" type="text/css" />
  <link href="/sitecore/shell/Themes/Standard/Default/Dialogs.css" rel="stylesheet" type="text/css" />
  <link href="/sitecore/shell/Controls/TinyMCE Editor/style/tinymceeditor.css" rel="stylesheet" type="text/css" />
  <style type="text/css">
    textarea {
      outline: none;
    }
  </style>
  
  <script src="/sitecore/shell/controls/lib/jquery/jquery-1.10.2.min.js" type="text/javascript"></script>
  <script type="text/javascript">
    if (typeof(window.$sc) == "undefined") window.$sc = jQuery.noConflict();
  </script>
  <script src="/sitecore/shell/controls/lib/prototype/prototype.js" type="text/javascript"></script>
  <script src="/sitecore/shell/Controls/TinyMCE Editor/EditorPage.js" type="text/javascript"></script>
  <script src="/sitecore/shell/controls/lib/node_modules/tinymce/tinymce.js" type="text/javascript"></script>
  <script src="/sitecore/shell/Controls/TinyMCE Editor/SitecoreTinyMCEButtons.js" type="text/javascript"></script>
  <script src="/sitecore/shell/Controls/TinyMCE Editor/jquery.modal.min.js" type="text/javascript"></script>

  <script type="text/javascript">
    <asp:Placeholder runat="server" ID="ScriptConstants" />
  </script>
</head>

<body class="scStretch">
  <form runat="server" id="mainForm" class="scStretch">
    <sc:AjaxScriptManager runat="server" />
    <sc:ContinuationManager runat="server" />
    <asp:HiddenField ID="EditorToolbar" runat="server" />
    <asp:HiddenField ID="EditorPlugins" runat="server" />
    <asp:HiddenField ID="EditorInitCallback" runat="server" />
    <asp:HiddenField ID="EditorMenubar" runat="server" />
    <asp:HiddenField ID="EditorBranding" runat="server" />
    <asp:HiddenField ID="EditorStyleFormats" runat="server" />
    <asp:HiddenField ID="CSSPath" runat="server" />

    <input type="hidden" id="EditorValue" />
 
    <div class="scStretch scFlexColumnContainer">

      <div class="scFlexContent">
        <div class="scStretchAbsolute scDialogContentContainer scRadEditor">
          <textarea id="FieldText" runat="server"/>
        </div>
      </div>
      <script type="text/javascript" src="/sitecore/shell/Controls/TinyMCE Editor/BrowserDetection.js"></script>
      <script type="text/javascript" src="/sitecore/shell/Controls/TinyMCE Editor/RichText Commands.js"></script>
      <script type="text/javascript" src="/sitecore/shell/Controls/TinyMCE Editor/RTEfixes.js"></script>


      <div id="scButtons" class="scFormDialogFooter">
        <div class="footerOkCancel">
          <sc:Button Class="scButton scButtonPrimary" Click="javascript: if (Prototype.Browser.IE) { var designModeBtn = $$(\'.reMode_design\')[0]; designModeBtn && designModeBtn.click(); } EditorPage.scSendRequest(event, \'editorpage:accept\');" ID="OkButton" KeyCode="13" runat="server">
            <sc:Literal runat="server" Text="Accept" />
          </sc:Button>

          <sc:Button Click="javascript:EditorPage.scSendRequest(event, \'editorpage:reject\')" ID="CancelButton" KeyCode="27" runat="server">
            <sc:Literal runat="server" Text="Reject" />
          </sc:Button>
        </div>
      </div>
      </div>
    </form>
  </body>
</html>
