if (typeof (EditorPage) == "undefined") EditorPage = new Object();

EditorPage = function () {
  var currentKey = null;
  var tinyeditorref;

  function RemoveInlineScripts() {
    if (scRemoveScripts === "true") {
      removeInlineScriptsInRTE();
    }
  }

  // Initialisation method for TinyMCE
  function InitTinyMCE() {
    // Retrieve the configuration from the hidden fields
    var EditorToolbar = $j("#EditorToolbar").val().split("+");
    var EditorPlugins = $j("#EditorPlugins").val();
    var EditorInitCallback = $j("#EditorInitCallback").val();
    var EditorMenubar = $j("#EditorMenubar").val();
    var EditorBranding = $j("#EditorBranding").val();
    var CSSPath = $j("#CSSPath").val();

    tinyeditorref = tinymce.init({
        height: 500,
        selector: '#FieldText',
        menubar: EditorMenubar,
        image_advtab: true,
        branding: EditorBranding,
        toolbar: EditorToolbar,
        plugins: EditorPlugins,
        autosave_ask_before_unload: false,
        content_css: CSSPath,
        init_instance_callback: function (editor) {
          eval(EditorInitCallback);
        }
    });

    window.tinyeditorref2 = tinymce;
    window.tinyeditorref = tinyeditorref;
  }

  function OnClientLoad(editor) {
    editor.attachEventHandler("mouseup", function() {
      var element = editor.getSelection().getParentElement();
      if (element !== undefined && element.tagName.toUpperCase() === "IMG") {
        fixImageParameters(element, prefixes.split("|"));
      }
    });

    var filter = new WebControlFilter();
    editor.get_filtersManager().add(filter);

    var protoFilter = new PrototypeAwayFilter();
    editor.get_filtersManager().add(protoFilter);

    var imageFilter = new ImageSourceFilter();
    editor.get_filtersManager().add(imageFilter);

    setTimeout(function() {
      var filterManager = editor.get_filtersManager();
      var myFilter = filterManager.getFilterByName("Sitecore WebControl filter");
      var myImageFilter = filterManager.getFilterByName("Sitecore ImageSourceFilter filter");

      myFilter.getDesignContent(editor.get_contentArea());
      myImageFilter.getDesignContent(editor.get_contentArea());

      editor.fire("ToggleTableBorder");
      editor.fire("ToggleTableBorder");

      editor.setFocus();
    }, 0);
  }

  function OnClientModeChange(editor, args) {
    scFitEditor();
  }

  function scFitEditor() {
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

  return {

    Init: function() {
      $j(".reMode_design").click(function(){
        RemoveInlineScripts();
      });

      Event.observe(window, "resize", scFitEditor);
      InitTinyMCE();
    },

    scSendRequest: function(evt, command) {
      RemoveInlineScripts();
      if (tinyMCE != null) {
        var newValue = $j(tinyMCE.activeEditor.getDoc()).find("body").html();
        $("EditorValue").value = newValue;
        $j("#FieldText").html(newValue);
      }
        
      RemoveInlineScripts();
      scForm.browser.clearEvent(evt);
      scForm.postRequest("", "", "", command);

      return false;
    },

    scCloseEditor: function() {
      var doc = window.top.document;

      // Field editor
      var w = doc.getElementById('feRTEContainer');

      if (w) {        
        $(w).hide();
      } else {
      // Page editor
        if (top._scDialogs.length != 0) {
          top.dialogClose();
        } else {
          scCloseRadWindow();
        }
      }
    }
  }
}();

var $j = jQuery.noConflict();
$j(document).ready(function() {      
    EditorPage.Init();
});