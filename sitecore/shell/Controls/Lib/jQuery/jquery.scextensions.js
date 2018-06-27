if (typeof(window.$sc) != "undefined") {
  $sc.Event.prototype.stop = function() {    
    this.stopPropagation();
    this.preventDefault();
  };

  $sc.Event.prototype.isLeftButton = function() {
    var button = this.originalEvent.button;
    if (typeof(button) != "undefined") {
      return $sc.browser.msie ? button === 1 : button === 0;
    }
    else {
      return this.originalEvent.which === 1;
    }
  };

  $sc.event.special.elementresize = {
    setup:function() {
      // There's no native 'elementresize' event. Don't allow jQuery to attach it.
      return true;
    },

    add: function(handleObj) {
      if (this && this.attachEvent && handleObj.handler) {      
        this.attachEvent("onresize", handleObj.handler);                         
      }
    },

    remove: function(handleObj) {
      if (this && this.detachEvent && handleObj.handler) {       
        this.detachEvent("onresize", handleObj.handler);        
      }
    }
  };

  $sc.fn.update = function(html) {
    this.get(0).innerHTML = html;
    return this;
  };

  $sc.fn.outerHtml = function(value) {
    if(value) {
      $sc(value).insertBefore(this);
      $sc(this).remove();
    }
    else { 
      // IE has problems with cloning <SCRIPT> nodes. 
      if ($sc.browser.msie) {
        var html = "";
        this.each(function() { html += this.outerHTML; });
        return html;
      }

      return $sc("<div>").append($sc(this).clone()).html(); 
    }
  };

  $sc.extend({
    util: function() {
      return Sitecore.PageModes.Utility;
    },
       
    areEqualStrings: function (string1, string2, ignoreCase) {
      if ($sc.type(string1) !== "string") {
        throw "First parameter must be of a String type";
      }
      
      if ($sc.type(string2) !== "string") {
        throw "Second parameter must be of a String type";
      }
              
      if (ignoreCase) {
        return string1.toLowerCase() === string2.toLowerCase();                    
      }
      
      return string1 === string2; 
    },

    evalJSON: function(json) {
      return eval("("+json+")");
    },
      
    exists: function(_array, callback) {
      var length = _array.length;
      for (var i = 0; i < length; i++) {
        if (callback.call(_array[i], i)) {
          return true;
        }
      }

      return false;
    },

    findIndex: function(_array, callback) {
      var length = _array.length;
      for (var i = 0; i < length; i++) {
        if (callback.call(_array[i], i)) {
          return i;
        }
      }

      return -1;
    },

    first: function(_array, callback) {
      var length = _array.length;
      for (var i = 0; i < length; i++) {
        if (callback.call(_array[i], i)) {
          return _array[i];
        }
      }

      return null;
    },

    formatString: function(str) {
      if (!str) {
        return str;
      }

      for (var i = 1; i < arguments.length; i++) {
        str = str.replace(new RegExp('\\{' + (i - 1) + '\\}', 'gi'), arguments[i]);
      }

      return str;
    },

    last: function(_array, callback) {
      var length = _array.length;
      for (var i = length - 1; i >= 0; i--) {
        if (callback.call(_array[i], i)) {
          return _array[i];
        }
      }

      return null;
    },

    isHtml: function(content) {
      return this.removeTags(content) !== content;
    },

    parseQuery:function(query) {
      var result = {};
      query.replace(/([^?=&]+)(=([^&]*))?/g,
	      function($0, $1, $2, $3) { 
          result[$1] = $3;        
        }
	    );

      return result;
    },

    removeTags:function(html) {
      return html.replace(/<\w+(\s+("[^"]*"|'[^']*'|[^>])+)?>|<\/\w+>/gi, "");
    },

    toShortId:function(id) {
      if (!id) {
        return id;
      }

      return id.replace(/-|{|}/g ,"");
    },

    toId: function(shortId) {
      if (!shortId || shortId.length != 32) return shortId;
      return "{" +shortId.substr(0, 8) + "-" + shortId.substr(8, 4) + "-" + shortId.substr(12, 4) + "-" + shortId.substr(16, 4) + "-" + shortId.substr(20, 12) + "}";
    },

    truncate: function(string, length) {
      if (string.length > length) {
        return string.substr(0, length) + "...";
      }

      return string;
    }
  });
}