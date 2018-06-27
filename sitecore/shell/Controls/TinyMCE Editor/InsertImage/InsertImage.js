if (typeof (TinyMCEEditor) == "undefined") {
    TinyMCEEditor = {};
}

(function() {
    "use strict";
    TinyMCEEditor.InsertImage = function () {

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

      // Adds the chosen image in the editor
      function addImage(returnValue) {
        if (returnValue != null) {
          var RTE = getTinyMCEReference().get("FieldText");

          var node = RTE.selection.getNode();
          var content = RTE.selection.getContent();

          if (content != null && content != "") {
            node.innerHTML = node.innerHTML.replace(content, returnValue.media);
          } else {
            RTE.execCommand('mceInsertContent', false, returnValue.media);
          }
        }
      }

      // Closes the modal where the dialog is shown
      function closeIframeModal() {
        var modalElement = getModalElement();
        if (modalElement != null) {
          modalElement.parent().empty();
        }
      }

      return {

        scClose: function(media) {
          var returnValue = {
            media:media
          };

          addImage(returnValue);
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