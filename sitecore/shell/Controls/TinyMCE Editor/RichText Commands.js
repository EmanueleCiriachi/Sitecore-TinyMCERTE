/* This file is shared between older developer center rich text editor and the new EditorPage, that is used exclusively by Content Editor */

var scEditor = null;
var scTool = null;
var scDatabase;

function scInsertSitecoreLink(sender, returnValue) {
  if (!returnValue) {
    return;
  }

  var d = scEditor.getSelection().getParentElement();

  if (BrowserDetection.isFirefox && d.tagName == "A") {
    d.parentNode.removeChild(d);
  } else {
    scEditor.fire("Unlink");
  }

  var text = scEditor.getSelectionHtml();

  if (BrowserDetection.isIE) {
    text = scIEFixRTETextRange(scEditor);
  }

  if (text == "" || text == null || ((text != null) && (text.length == 15) && (text.substring(2, 15).toLowerCase() == "<p>&nbsp;</p>"))) {
    text = returnValue.text;
  }
  else {
    // if selected string is a full paragraph, we want to insert the link inside the paragraph, and not the other way around.
    var regex = /^[\s]*<p>(.+)<\/p>[\s]*$/i;
    var match = regex.exec(text);
    if (match && match.length >= 2) {
      scEditor.pasteHtml("<p><a href=\"" + returnValue.url + "\">" + match[1] + "</a></p>", "DocumentManager");
      return;
    }
  }

  scEditor.pasteHtml("<a href=\"" + returnValue.url + "\">" + text + "</a>", "DocumentManager");
}

GetMediaID = function(html)
{
  var id = null;
  var list = prefixes.split('|');
  if(!list)
  {
    id = GetIDByMediaPrefix('~\\/media\\/([\\w\\d]+)\\.ashx', html);
  }
  else
  {
    for(i = 0; i < list.length; i++)
    {
      if(list[i] != '')
      {
        id = GetIDByMediaPrefix(list[i] +'([\\w\\d]+)\\.ashx', html);
        if(id)
        {
          break;
        }
      }
    }
  }
  
  return id;
}

GetIDByMediaPrefix = function(pattern, html)
{
    var regex = new RegExp(pattern, 'm');
    var match = regex.exec(html);
    if (match && match.length >=1 && match[1]) {
      return match[1];
    }
    
    return null;
}

function scInsertSitecoreMedia(sender, returnValue) {
  if (returnValue) {
    scEditor.pasteHtml(returnValue.media);
  }
}

function PrototypeAwayFilter() {
  PrototypeAwayFilter.initializeBase(this);
  this.set_isDom(true);
  this.set_enabled(true);
  this.set_name("Sitecore PrototypeAwayFilter filter");
  this.set_description("Sitecore PrototypeAwayFilter filter removes prototype attributes from DOM");
}

PrototypeAwayFilter.prototype =
{
  getHtmlContent: function (content) {
    this.getHtml(content);
    return content;
  },

  getHtml: function (node) {
    var children = node.childNodes;

    for (var i = children.length - 1; i >= 0; i--) {
      var n = children[i];

      if (n.nodeType != 1) {
        continue;
      }

      if (n.removeAttribute) {
        n._extendedByPrototype = null;
        n.removeAttribute("_extendedByPrototype");
      }

      this.getHtml(n);
    }
  }
}

WebControlFilter = function() {
  WebControlFilter.initializeBase(this);
  this.set_isDom(true);
  this.set_enabled(true);
  this.set_name("Sitecore WebControl filter");
  this.set_description("Sitecore WebControl filter displays ASP.NET web controls");
}

WebControlFilter.prototype =
{
  getHtmlContent: function(content) {
    this.getHtml(content);
    return content;
  },

  getHtml: function(node) {
    var children = node.childNodes;

    for (var i = children.length - 1; i >= 0; i--) {
      //Do not use here Prototype. This will cause issues like 329238, when Flash object getting extended by 
      //prototype methods.
      var n = children[i];

      if (n.nodeType != 1) {
        continue;
      }

      if (n.tagName != "IMG" || n.className != "scWebControl") {
        this.getHtml(n);
        continue;
      }

      Element.replace(n, n.title);
    }
  },

  getDesignContent: function(content) {
    this.getDesign(content);
    return content;
  },

  getDesign: function(node) {
    var children = node.childNodes;

    for (var i = children.length - 1; i >= 0; i--) {
      var n = children[i];

      if (n.nodeType != 1) {
        continue;
      }

      var prefix = n.scopeName != null ? n.scopeName : n.prefix;

      if (prefix == null || prefix == "" || prefix == "HTML") {
        this.getDesign(n);
        continue;
      }

      var webcontrol = n.outerHTML;
      var j = webcontrol.indexOf("<?xml:namespace");
      if (j >= 0) {
        var k = webcontrol.substr(j).indexOf(">") + j + 1;
        webcontrol = webcontrol.substr(k);
      }

      var e = new Element("img", { 'width': 32, 'height': 32, 'class': 'scWebControl', 'title': webcontrol, 'style': 'background:#F8EED0;margin:4px;border:1px solid #F0CCA5', 'src': '/sitecore/shell/~/icon/Software/32x32/Elements1.png' });

      Element.replace(n, e);
    }
  }
}

function ImageSourceFilter() {
  ImageSourceFilter.initializeBase(this);
  this.set_isDom(true);
  this.set_enabled(true);
  this.set_name("Sitecore ImageSourceFilter filter");
  this.set_description("Sitecore ImageSourceFilter filter adds la querystring parameter to image sources");
}

ImageSourceFilter.prototype =
{
  getHtmlContent: function (content) {
    this.getHtml(content);
    return content;
  },

  getHtml: function (node) {
    var children = node.childNodes;

    for (var i = children.length - 1; i >= 0; i--) {
      var n = children[i];

      if (n.nodeType != 1) {
        continue;
      }

      if (n.tagName.toLowerCase() == "img") {
        var src = scGetImageSource(n.outerHTML);
        if (src && n.getAttribute("_languageInserted")) {
          src = scReplaceAmps(src);
          n.setAttribute("src", scRemoveLanguageFromImageSource(src, window.scLanguage));
          n.removeAttribute("_languageInserted");
        }
      }
      else this.getHtml(n);
    }
  },

  getDesignContent: function (content) {
    this.getDesign(content);
    return content;
  },

  getDesign: function (node) {
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
          var newSrc = scAddLanguageToImageSource(src, window.scLanguage);
          if (newSrc != src) {
            n.setAttribute("src", newSrc);
            n.setAttribute("_languageInserted", true);
          }
        }
      }
      else this.getDesign(n);
    }
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
    var prefs = window.prefixes.split("|");
    if (!prefs) {
      prefs = new Array();
      prefs[0] = "-/media/";
    }
    for (var j = 0; j < prefs.length; ++j) {
      if (prefs[j] == "") continue;

      var regex = new RegExp(prefs[j] + '([\\w\\d]{32})\\.ashx', "m");
      var match = regex.exec(src);
      if (match) {
        var qs = src.indexOf('?');
        if (qs > -1) {
          var lang = src.indexOf('la=');
          if (lang == -1) {
            src = src.substr(0, qs + 1) + "la=" + language + "&" + src.substr(qs + 1);
            return src;
          }
        }
        else {
          src = src + "?la=" + language;
          return src;
        }
        break;
      }
    }
  }
  
  return src;
}

function scRemoveLanguageFromImageSource(src, language) {
  var lang = src.indexOf("la=" + language);
  
  if (lang > -1) {
    src = src.substr(0, lang-1) + src.substr(lang + ("la=" + language).length);
  }

   var qs = src.indexOf("?&");
   if (qs > -1) {
     src = src.substr(0, qs + 1) + src.substr(qs + 2);
   }

  qs = src.indexOf("?");
  
  if (qs == src.length - 1) {
    src = src.substr(0, qs);
  }

  return src;
}

function scReplaceAmps(text) {
  var ind;
  while((ind=text.indexOf("&amp;"))>-1) {
    text = text.substring(0, ind + 1) + text.substring(ind + "&amp;".length);
  }
  return text;
}

function scIEFixRTETextRange(scEditor) {
  var text = scEditor.getSelectionHtml();
  var regex = /^([\s]*<p.*?>).+(<\/p>[\s]*)$/i;
  var match = regex.exec(text);
  if (match && match.length == 3) {
    var elem = scEditor.getSelectedElement();
    if (elem.parentElement.lastChild != elem) {
      var range = scEditor.getSelection().getRange();
      range.moveEnd('character', -1);
      scEditor.getSelection().selectRange(range);

      text = scEditor.getSelectionHtml();
    }
  }

  return text;
}