<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preview.aspx.cs" Inherits="Sitecore.Shell.Controls.RADEditor.Preview" %>
<%@ Import Namespace="Sitecore" %>
<%@ Register Assembly="Sitecore.Kernel" Namespace="Sitecore.Web.UI.HtmlControls" TagPrefix="sc" %>
<!DOCTYPE html>

<html>
<head runat="server">
    <title>Sitecore</title>
    <script src="/sitecore/shell/controls/lib/prototype/prototype.js" type="text/javascript"></script>
    <asp:PlaceHolder id="Stylesheets" runat="server" />
    <style type="text/css">
      body { height:100%; width:100%; padding: 0;margin: 0;display: inline-block;vertical-align: top;}
      form { height:100%; width:100% }
      #ContentWrapper {margin-left: 4px;}
    </style>

    <script type="text/javascript" language="javascript">
    
      var scModified = false;
      var scDisabled = "<asp:PlaceHolder id="DisabledFlag" runat="server" />";
      var scValue = <asp:PlaceHolder id="InitialValue" runat="server" />;
      var scOldValue = "";
   
      function scGetForm() {
        return window.parent.scForm;
      }
      
      function scGetFrameValue(value, request) {
        if (scOldValue.indexOf("__#!$No value$!#__") >= 0) {
          return "__#!$No value$!#__";
        }
        
        var html = scValue;
        
        if (scModified) {
          var form = scGetForm()
          
          if (form != null) {
            form.setModified(true);
          }
          
          if (request != null) {
            request.form += "&EditorChanged=" + encodeURIComponent("1");
          }
        }
        
        return html;
      }
      
      function scGetText() {
        return document.getElementById("ContentWrapper").innerHTML;
      }
    
      function scSetText(text) {
        scModified = scValue != text;
        
        scValue = text;
              
        if (scValue.indexOf("__#!$No value$!#__") >= 0) {
          document.getElementById("ContentWrapper").innerHTML = ""; }
        else {
          document.getElementById("ContentWrapper").innerHTML = scValue;
        }
        
        fixIeObjectTagBug();

        scShowWebControls(document.body);
        scAddLanguageToImageSources(document.getElementById("ContentWrapper"));
        scDisableLinks();
      }
      
      function scOnLoad() {
        $('ClickableArea').observe('focus', function(){ this.setStyle({position:'static', height:'0px', width:'0px'}); });
        fixIeObjectTagBug();

        scOldValue = scValue;
        document.body.title = "<asp:PlaceHolder id="HelpText" runat="server" />";
        
        if (typeof(document.addEventListener) != "undefined") {
          document.addEventListener("keydown", scKeyDown, false);
        }
        
        if (scValue.indexOf("__#!$No value$!#__") >= 0) {
          document.getElementById("ContentWrapper").innerHTML = "";
        }
        
        scShowWebControls(document.body);
        scAddLanguageToImageSources(document.getElementById("ContentWrapper"));
        scDisableLinks();
      }
      
      function fixIeObjectTagBug(){
        var objects = $$('object');
        var i;
        for (i = 0; i < objects.length; i++) {
            if (!objects[i].id || objects[i].id.indexOf('IE_NEEDS_AN_ID_') > -1) {
                objects[i].id = 'IE_NEEDS_AN_ID_' + i;
            }
        }
      }

      function scAddLanguageToImageSources(node) {
        var children = node.childNodes;

        for (var i = children.length - 1; i >= 0; i--) {
          var n = children[i];

          if (n.nodeType != 1) {
            continue;
          }

          if (n.tagName.toLowerCase() == "img") {
            var src = scGetImageSource(n.outerHTML);
            if (src) {
              src = scReplaceAmps(src);
              var newSrc = scAddLanguageToImageSource(src, parent.document.getElementById("scLanguage").value);
              if (newSrc != src) {
                n.setAttribute("src", newSrc);
                n.setAttribute("_languageInserted", true);
              }
            }
          } else scAddLanguageToImageSources(n);
        }
      }
      
      function scGetImageSource(text) {
        var sourceStart = text.indexOf("src=\"");
        var sourceEnd;
        var source;

        if (sourceStart > -1) {
          sourceStart += "src=\"".length;
          sourceEnd = text.indexOf("\"", sourceStart);
          if (sourceEnd > -1) {
            source = text.substr(sourceStart, sourceEnd - sourceStart);
          }
        }

        return source.trim();
      }

      function scAddLanguageToImageSource(src, language) {
        if (src) {

            var regex = new RegExp('([\\w\\d]{32})\\.ashx', "m");
            var match = regex.exec(src);
            if (match) {
              var qs = src.indexOf('?');
              if (qs > -1) {
                var lang = src.indexOf('la=');
                if (lang == -1) {
                  src = src.substr(0, qs + 1) + "la=" + language + "&" + src.substr(qs + 1);
                  return src;
                }
              } else {
                src = src + "?la=" + language;
                return src;
              }
          }
        }

        return src;
      }

      function scReplaceAmps(text) {
        var ind;
        while ((ind = text.indexOf("&amp;")) > -1) {
          text = text.substring(0, ind + 1) + text.substring(ind + "&amp;".length);
        }
        return text;
      }

      function scDisableLinks() {
        var form = scGetForm();
        
        var list = document.getElementsByTagName("A");
        for(var n = 0; n < list.length; n++) {
          form.browser.attachEvent(list[n], "onclick", scDoCancel);
        }
      }
      
      function scShowWebControls(node) {
        var children = node.childNodes;

        for (var i = children.length - 1; i >= 0; i--) {
          var n = $(children[i]);

          if (n.nodeType != 1) {
            continue;
          }

          var prefix = n.scopeName != null ? n.scopeName : n.prefix;
          if (prefix == "HTML") {
            prefix = null;
          }

          if (prefix == null || prefix == "") {
            var j = n.tagName.indexOf(':');
            if (j >= 0 && !(n.tagName.substr(0, 1) == "/")) {
              prefix = n.tagName.substr(0, j);
            }
          }

          if (prefix == null || prefix == "") {
            scShowWebControls(n);
            continue;
          }

          var e = new Element("img", { 'width': 32, 'height': 32, 'class': 'scWebControl', 'title': n.outerHTML, 'style': 'background:#F8EED0;margin:4px;border:1px solid #F0CCA5', 'src': '/sitecore/shell/~/icon/Software/32x32/Elements1.png' });

          Element.replace(n, e);
        }
      }

      function scEdit() {
        if (scDisabled == "1") {
          return;
        }
        
        var form = scGetForm();
        
        if (form != null) {
          var id = location.href;
          
          var n = id.indexOf("&ed=");
          if (n >= 0) {
            id = id.substr(n + 4);
            
            n = id.indexOf("&");
            
            if (n >= 0) {
              id = id.substr(0, n);
            }
          
            form.invoke("richtext:edit(id=" + id + ")");
          }
        }
      }

      function scDoCancel() {
        var evt = window.event;

        if (evt != null) {
          evt.returnValue = false;
          evt.cancelBubble = true;
        }
        
        return false;
      }

      function scKeyDown(e) {
        if (typeof(document.addEventListener) != "undefined" && !e) {
          return;
        }
      
        if (scDisabled == "1") {
          return;
        }
        
        var form = scGetForm();
        var evt = window.event || e;
        
        if (form != null) {
          if (evt.ctrlKey && evt.keyCode == 83) {
            form.postEvent(evt.srcElement, evt, "item:save");
          }
          else {
            form.handleKey(document.body, evt, null, null, true);
          }
          
          form.browser.clearEvent(evt, true, false, 0);
        }
      }

      function scContextMenu(e) {
        var evt = window.event || e;

        if (!e.ctrlKey) {
          return false;
        }
      }
      
    </script>
</head>

<body onload="javascript:scOnLoad();" ondblclick="javascript:scEdit();" onkeydown="javascript:scKeyDown();" onblur="javascript:$('ClickableArea').setStyle({position:'absolute', height: '100%', width: '100%'});" oncontextmenu="javascript:return scContextMenu(event)">
  <div id="ClickableArea" style="position: absolute; height: 100%; width: 100%; top: 0; left: 0;" tabindex='0'></div>
  <div id="ContentWrapper">
    <asp:PlaceHolder id="Content" runat="server"/>
  </div>
</body>
</html>
