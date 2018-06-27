if (typeof (TinyMCEEditor) == "undefined") {
    TinyMCEEditor = {};
}

(function() {
    "use strict";
    TinyMCEEditor.InsertLink = function () {

      // Return a reference to the TinyMCE editor instance used in the RTE Iframe
      function getTinyMCEReference() {
        var mainIframe = top.document.getElementById("jqueryModalDialogsFrame");

        if (mainIframe) {
          var editorIframe = mainIframe.contentWindow.document.getElementById("scContentIframeId0");

          if (editorIframe) {
            return editorIframe.contentWindow.tinyeditorref2;
          }
        }
          
        return null;
      }

      // Retrieves the modal element where the dialog is shown
      function getModalElement() {
        var mainIframe = top.document.getElementById("jqueryModalDialogsFrame");

        if (mainIframe) {
          var editorIframe = mainIframe.contentWindow.document.getElementById("scContentIframeId0");

          if (editorIframe) {
            return window.$sc(editorIframe.contentWindow.document).find(".editorModal");
          }
        }
          
        return null;
      }

      // Closes the modal where the dialog is shown
      function closeIframeModal() {
        var modalElement = getModalElement();
        if (modalElement != null) {
          modalElement.parent().empty();
        }
      }

      // Adds the chosen link in the editor
      function addLink(returnValue) {
        if (returnValue != null) {
          var RTE = getTinyMCEReference().get("FieldText");

          var node = RTE.selection.getNode();
          var content = RTE.selection.getContent();
          var link = [];

          link.push('<a href="');
          link.push(returnValue.url);
          link.push('">');
          link.push(content != null && content != "" ? content : returnValue.text);
          link.push('</a>');

          if (content != null && content != "") {
            node.innerHTML = node.innerHTML.replace(content, link.join(''));
          } else {
            RTE.execCommand('mceInsertContent', false, link.join(''));
          }
        }
      }

      return {

        scClose: function(url, text) {
          var returnValue = {
            url:url,
            text:text
          };

          addLink(returnValue);
          closeIframeModal();
        },

        scCancel: function() {
          closeIframeModal();
        },

        Init: function() {
          if (window.focus && Prototype.Browser.Gecko) {
            window.focus();
          }
        }
      }
    }();
}());

window.$sc(document).ready(function() {
  TinyMCEEditor.InsertLink.Init();
});