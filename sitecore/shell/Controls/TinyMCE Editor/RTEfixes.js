// Hack described here http://www.telerik.com/community/forums/sharepoint-2007/full-featured-editor/paragraph-style-names-don-t-match-config.aspx
function OnClientSelectionChange(editor, args) {
    var tool = editor.getToolByName("FormatBlock");
    if (tool) {
        setTimeout(function () {
            var defaultBlockSets = [
              ["p", "Normal"],
              ["h1", "Heading 1"],
              ["h2", "Heading 2"],
              ["h3", "Heading 3"],
              ["h4", "Heading 4"],
              ["h5", "Heading 5"],
              ["menu", "Menu list"],
              ["pre", "Formatted"],
              ["address", "Address"]];

            var value = tool.get_value();

            var block = Prototype.Browser.IE
              ? defaultBlockSets.find(function (element) { return element[1] == value; })
              : [value];

            if (block) {
                var tag = block[0];
                var correctBlock = editor._paragraphs.find(function (element) { return element[0].indexOf(tag) > -1; });
                if (correctBlock) {
                    tool.set_value(correctBlock[1]);
                }
            }
        }, 0);
    }
}

function OnClientModeChange(editor) {
    var url = window.location.protocol + '//' + window.location.hostname;
    var html = editor.get_html();
    var originalHtml = html;

    var lastIndexOf = window.location.pathname.lastIndexOf("/");
    var path = "";
    if (lastIndexOf !== -1) {
        path = window.location.pathname.substring(0, lastIndexOf);
    }

    if (path.indexOf("/") !== 0) {
        path = "/" + path;
    }

    var regexpWithPath = new RegExp(url + path, "gi");
    var regexpWithUrl = new RegExp(url, "gi");

    html = html.replace(regexpWithPath, "");
    html = html.replace(regexpWithUrl, "");

    if (html !== originalHtml) {
        editor.set_html(html);
        editor.saveClientState();
    }
}

function OnClientCommandExecuted(sender, args) {
    if (args.get_commandName() == "SetImageProperties") {
        replaceClearImgeDimensionsFunction();
    }
}

function replaceClearImgeDimensionsFunction(count) {
    if (!count) count = 0;
    setTimeout(function () {
        try {
            var selector = 'iframe[src^="Telerik.Web.UI.DialogHandler.aspx?DialogName=ImageProperties"]';
            $$(selector)[0].contentWindow.Telerik.Web.UI.Widgets.ImageProperties.prototype._clearImgeDimensions = function (image) {
                fixImageParameters(image, prefixes.split('|'));
            };
        } catch (e) {
            if (count < 10) {
                count++;
                replaceClearImgeDimensionsFunction(count);
            }
        }
    }, 500);
}

function fixImageParameters(image, mediaPrefixes) {

    var isMediaLink = false;

    for (var i = 0; i < mediaPrefixes.length; i++) {
        if (mediaPrefixes[i] === undefined || mediaPrefixes[i] === "") {
            continue;
        };

        var imageHost = decodeURI(window.location.protocol + "//" + window.location.hostname);

        if (new RegExp("^" + imageHost + "(.)*/" + decodeURI(mediaPrefixes[i]) + "*", "i").test(decodeURI(image.src))) {
            isMediaLink = true;
            break;
        };
    };

    if (!isMediaLink) { return; };

    _toQueryParams = function (href) {
        var result = {};

        var search = href.split("?")[1];

        if (search !== undefined) {
            var params = search.split("&");
            $sc.each(params, function (index, value) {
                var param = value.split("=");
                result[param[0]] = param[1];
            });
        };

        return result;
    };

    // This code corrects inconsistencies between image sizes set in style attribute, width and height attributes, w and h image parameters.
    var src = image.getAttribute("src");

    var params = _toQueryParams(src);

    var n = src.indexOf("?");
    if (n >= 0) {
        src = src.substr(0, n + 1);
    } else {
        src = src + "?";
    }

    for (var param in params) {
        if (params[param] === undefined || params[param] === "") {
            delete params[param];
        }
    }

    // if style
    if (image.style.height !== undefined && image.style.height !== "") {
        var height = image.style.height.replace("px", "");
        image.removeAttribute("height");
        params["h"] = height;
    }
        // else if attribute
    else if (image.attributes !== undefined && image.attributes["height"] !== undefined && image.attributes["height"] !== "") {
        image.style.height = image.attributes["height"].value + "px";
        params["h"] = image.attributes["height"].value;
    }
        // no style, no attribute
    else {
        delete params["h"];
    }

    // if style
    if (image.style.width !== undefined && image.style.width !== "") {
        var width = image.style.width.replace("px", "");
        image.removeAttribute("width");
        params["w"] = width;
    }
        // else if attribute
    else if (image.attributes !== undefined && image.attributes["width"] !== undefined && image.attributes["width"] !== "") {
        image.style.width = image.attributes["width"].value + "px";
        params["w"] = image.attributes["width"].value;
    }
        // no style, no attribute
    else {
        delete params["w"];
    }

    if ($sc.param(params) === "" && src.endsWith("?")) {
        src = src.substr(0, src.length - 1);
    } else {
        src = src + $sc.param(params);
    }

    image.setAttribute("src", src);
}

// Fix mentioned here http://www.telerik.com/community/forums/aspnet-ajax/editor/html-entity-characters-are-not-escaped-on-hyperlink-editor-email-subject.aspx
function OnClientPasteHtml(sender, args) {
    var commandName = args.get_commandName();
    var value = args.get_value();
    if (Prototype.Browser.IE && (commandName == "LinkManager" || commandName == "SetLinkProperties")) {
        if (/<a[^>]*href=['|"]mailto:.*subject=/i.test(value)) {
            var hrefMarker = 'href=';

            // quote could be ' or " depending on subject content
            var quote = value.charAt(value.indexOf(hrefMarker) + hrefMarker.length);
            var regex = new RegExp(hrefMarker + quote + 'mailto:.*subject=.*' + quote, 'i');
            var fixedValue = value.replace(regex, function (str) { return str.replace(/</g, "&lt;").replace(/>/g, "&gt;"); });
            args.set_value(fixedValue);
        }
    } else if (commandName == "Paste") {
        // The StripPathsFilter() method receives as a parameter an array of strings (devided by a white space) that will be stripped from the absolute links.
        var relativeUrl = getRelativeUrl(); //returns the relative url.
        var domainUrl = window.location.protocol + '//' + window.location.host;
        if (relativeUrl) {
            var filter = new Telerik.Web.UI.Editor.StripPathsFilter([relativeUrl, domainUrl]); //strip the domain name from the absolute path

            var contentElement = document.createElement("SPAN");
            contentElement.innerHTML = value;
            var newElement = filter.getHtmlContent(contentElement);
            value = newElement.innerHTML;
            if (scForm.browser.isFirefox) {
                value = value.replace(/%7e\//ig, '~/');
            }

            args.set_value(value);  //set the modified pasted content in the editor
        }
    }

    if (Prototype.Browser.IE) {
        var helperIframe = $$("iframe[title^='Paste helper']:first")[0];
        if (helperIframe) {
            Element.setStyle(helperIframe, { width: 0, height: 0 });
        }
    }
}

function getRelativeUrl() {
    var result = window.location.href;
    if (result) {
        var query = window.location.search;
        if (query) {
            result = result.substring(0, result.length - query.length);
        }

        var slashPosition = result.lastIndexOf('/');
        if (slashPosition > -1) {
            result = result.substring(0, slashPosition + 1);
        }
    }

    return result;
}

function fixIeObjectTagBug() {
    var objects = Element.select($('Editor_contentIframe').contentWindow.document, 'object');
    var i;
    for (i = 0; i < objects.length; i++) {
        if (!objects[i].id || objects[i].id.indexOf('IE_NEEDS_AN_ID_') > -1) {
            objects[i].id = 'IE_NEEDS_AN_ID_' + i;
        }
    }
}

function removeInlineScriptsInRTE() {
    if (tinyMCE != null) {
        var content = tinyMCE.activeEditor.getContent();
        var html = $sc.parseHTML(content);

        validateScripts(html);
        tinyMCE.activeEditor.setContent($sc("<p>").append($sc(html).clone()).html());
    }
}

function validateScripts(el) {
  if (el) {
    $sc(el).each(function() {
      $sc(this).removeAttr('srcdoc');
      $sc(this).removeAttr('allowscriptaccess');

      var elem = $sc(this)[0];
      if (elem.attributes) {
        for (var i = 0; i < elem.attributes.length; i++) {
          var attr = elem.attributes[i];
          if (attr.name.startsWith('on')
            || (attr.value.indexOf("javascript") > -1)
            || (attr.value.indexOf("base64") > -1)) {
            elem.removeAttribute(attr.name);
          }
        }
      }

      if (elem.childNodes && elem.childNodes.length > 0) {
        for (var child in elem.childNodes) {
          /* nodeType == 1 is filter element nodes only */
          if (elem.childNodes[child].nodeType == 1)
            validateScripts(elem.childNodes[child]);
        }
      }
    });
  }
}