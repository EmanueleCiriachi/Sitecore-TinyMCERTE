<%@ Page Language="c#" Inherits="Sitecore.Shell.Controls.RADEditor.RADEditor" AutoEventWireup="true" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html>
<html style="width:100%;height:100%;margin:0px;padding:0px;overflow:hidden">
  <head>
    <title>Sitecore</title>
    
    <style type="text/css">
      .scRadEditor .InsertSitecoreLink 
      {
        background-position: -3697px center;
      }
      
      .scRadEditor .InsertSitecoreMedia
      {
      	background-position: -1866px center;
      }
      
    </style>
    
    <script src="/sitecore/shell/controls/lib/prototype/prototype.js" type="text/javascript"></script>
  
    <script type="text/javascript" language="javascript">
     var scClientID = "<%=Editor.ClientID%>"
     var scItemID = "<asp:placeholder id="ItemID" runat="server"/>"
     var scLanguage = "<asp:placeholder id="Language" runat="server"/>"
     var scSpecificLanguage = "<asp:placeholder id="SpecificLanguage" runat="server"/>"
    
      function scGetEditor() {
        if (typeof($find)=="function") {
          return $find(scClientID);
        }
        
        return null;
      }
      
      function scGetMode() {
        return '<%=Sitecore.Web.WebUtil.GetQueryString("mo")%>';
      }

      function HideToolbars(editor) {
        var holder = document.getElementById("Top" + editor.Id);
        holder.style.display = "none";
        OnClientLoad(editor);
      }
      
      function scFitEditor() {
        var editor = scGetEditor();
        if (!editor) {
          alert("no editor");
          return;
        }
        
        var container = $("mainForm");
        
        editor.setSize(container.getWidth(), container.getHeight());

        var designIframe = document.getElementById('Editor_contentIframe');
        var designMode = designIframe.style.height != '0px'; 
        if (designMode) {
          designIframe.style.height = "0";
        }

        var htmlIframe = document.querySelector('#Editor_contentIframe + iframe');
        htmlIframe && (htmlIframe.style.height = "0");
        setTimeout(function () {
          var clientHeight = document.getElementById('EditorCenter').clientHeight  + 'px';

          if (designMode) {
            designIframe.style.height = clientHeight;
          }

          htmlIframe && (htmlIframe.style.height = clientHeight);

        }, 0);
      }
      
      Event.observe(window, "resize", scFitEditor);
    </script>
    
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/Rich Text Editor/RichText.js"></script>
    <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/Rich Text Editor/RTEfixes.js"></script>
  </head>
  <body style="width:100%; height:100%; padding:0px; overflow:hidden; margin:0px">
    <form runat="server" id="mainForm" method="post" style="width:100%;height:100%;margin:0px;padding:0px">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
      <telerik:RadEditor ID="Editor" Runat="server" 
        Width="100%"
        Height="400px"
        CssClass="scRadEditor"
        
        Editable="True"
        AllowCustomColors="true"
        AllowThumbGeneration="false"
        ContentFilters="DefaultFilters"
        ConvertToXHtml="true"      
        ConvertFontToSpan="true"
        ConvertTagsToLower="true"
        EnableClientSerialize="true"
        EnableContextMenus="true"
        EnableDocking="true"
        EnableEnhancedEdit="true"
        EnableHtmlIndentation="true"
        EnableServerSideRendering="true"
        EnableTab="true"
        FocusOnLoad="false"
        ShowSubmitCancelButtons="false"
        ShowHtmlMode="false"
        ShowPreviewMode="false"
        StripAbsoluteAnchorPaths="true"
        StripAbsoluteImagesPaths="true"
        StripFormattingOptions="NoneSupressCleanMessage" 
        LocalizationPath="~/sitecore/shell/controls/rich text editor/Localization/"

        Skin="Default"
        ToolsFile="~/sitecore/shell/Controls/Rich Text Editor/ToolsFile.xml"

        ImageManager-UploadPaths="/media library"
        ImageManager-DeletePaths="/media library"
        ImageManager-ViewPaths="/media library"

        FlashManager-UploadPaths="/media library"
        FlashManager-DeletePaths="/media library"
        FlashManager-ViewPaths="/media library"
        
        MediaManager-UploadPaths="/media library"
        MediaManager-DeletePaths="/media library"
        MediaManager-ViewPaths="/media library"

        DocumentManager-DeletePaths="/media library"
        DocumentManager-ViewPaths="/media library"

        TemplateManager-UploadPaths="/media library"
        TemplateManager-DeletePaths="/media library"
        TemplateManager-ViewPaths="/media library"

        ThumbSuffix="thumb"
        
        OnClientLoad="OnClientLoad"
        OnClientModeChange="OnClientModeChange"
        OnClientCommandExecuted="OnClientCommandExecuted"
        OnClientSelectionChange = "OnClientSelectionChange"
        OnClientPasteHtml="OnClientPasteHtml" />
        
      <script type="text/javascript" language="javascript" src="/sitecore/shell/Controls/Rich Text Editor/RichText Commands.js"></script>
      
      <asp:placeholder id="EditorClientScripts" runat="server"/>
    </form>
  </body>
  <script type="text/javascript">    
    if (Prototype.Browser.IE) {
      var version = parseInt(navigator.userAgent.substring(navigator.userAgent.indexOf("MSIE") + 5));
      if (version == 7) {
        document.body.style.overflow = "";
      }
    }
  </script>
</html>