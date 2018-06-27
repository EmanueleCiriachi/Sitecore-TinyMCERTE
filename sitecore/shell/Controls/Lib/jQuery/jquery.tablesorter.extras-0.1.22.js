/*

@requires $/$.js
@requires $/plugins/$.tablesorter.js


Tablesorter widget to enable selcting rows.

*/
(function ($) {
  $.tablesorter.addWidget({
    /*
    
    Property: id
    The id of this widget.
   
    */
    id: 'select',

    /*
    
    Method: init
    Constructor.
   
    */
    init: function (table) {
      table.config.select = $.extend({}, this.defaults, table.config.select);

      var tr = $('tbody tr', table);
      $(tr)
         .unbind('mousedown.tablesorter.select')
         .bind('mousedown.tablesorter.select', {
           table: table
         }, $.proxy(this, 'onClick'));

      $(table)
         .unbind('update.tablesorter.select')
         .bind('update.tablesorter.select', $.proxy(this, 'update'));

      // Disable textselection for IE
      if ($.browser.msie) {
        $(tr)
			 .unbind('selectstart.tablesorter.select')
			 .bind('selectstart.tablesorter.select', function () { return false; });
      }
    },

    /*
    
    Method: onClick
    Will be triggerd on mouseout event. Triggers event on stored callbacks.
    
    Parameters:
    e - {Event Object} The event object.
    
    */
    onClick: function (e) {
      var table = $(e.data.table)[0];
      var elem = $(e.currentTarget)[0];
      var not = false;
      var tr = $('tbody tr', table);

      if (e.button == 0 || navigator.userAgent.indexOf("Trident/4") != -1 && e.button == 1) {     // left mouse button
        if (!e.ctrlKey) {
          $(elem).siblings('.selected').removeClass('selected');

          if (e.shiftKey) {
            not = true;
            var curId = $(tr).index($(elem));
            var start, end;

            if (table.config.select.prevId < curId) {
              start = table.config.select.prevId;
              end = curId + 1;
            } else {
              start = curId;
              end = table.config.select.prevId + 1;
            }

            elem = $(tr).slice(start, end);
          }
        }
        $(elem).toggleClass('selected');
      } else {
        if (!$(elem).hasClass('selected')) {
          $(elem).siblings('.selected').removeClass('selected');
          $(elem).addClass('selected');
        }
      }

      if (!not)
        table.config.select.prevId = $(tr).index($(elem));

      // Trigger tablesorter onselect event.
      $(table).trigger('select.tablesorter.select', [{
        table: $(table),
        selected: $('tbody tr.selected', table),
        elem: elem
      }]);

      // Store slected rows
      table.config.select.selection = $.map($('tbody tr.selected', table), function (elem, i) {
        return $($('input', elem)[0]).val();
      });

    },

    /*
    
    Method: update
    
    */
    update: function (e) {
      var table = e.target;
      var preselect = $.map($('tbody tr', table), function (elem, i) {
        return $.inArray($($('input', elem)[0]).val(), table.config.select.selection) < 0 ? null : elem;
      });
      $(preselect).addClass('selected');
    },

    /**/
    format: function (table) {
      this.init(table);
    }
  });

  /*

  $ filter :icontains. Same as :contains but not casesensitive.

  */
  $.expr[':'].icontains = function (obj, index, meta, stack) {
    return (obj.textContent || obj.innerText || $(obj).text() || '')
     .toLowerCase()
     .indexOf(meta[3].toLowerCase()) >= 0;
  };

  /*

  Tablesorter widget to filter table rows by its content.

  */
  $.tablesorter.addWidget({
    /*
    
    Property: id
    The widget id.
    
    */
    id: 'filter',

    defaults: {
      title: 'Search...',
      container: false
    },

    /*
    
    Method: init
    Constructor.
    
    Parameters:
    table - {HTML Object} The table.
    
    */
    init: function (table) {
      var _this = this;

      // Merge config.
      table.config.filter = $.extend({}, this.defaults, table.config.filter);

      var title = table.config.filter.title;
      var filterName = $(table).attr('id');
      var filterId = 'tsFilter_' + filterName;

      // Make filter input.
      var f = [];
      f.push('<input class="tablesorter tsFilter');
      if (title)
        f.push(' empty');
      f.push('"');
      f.push(' id="' + filterId + '"');
      f.push(' type="text"');
      f.push(' name="' + filterName + '"');
      if (title) {
        f.push(' value="' + title + '"');
        f.push(' title="' + title + '"');
      }
      f.push(' />');

      var filterInput = $('input#' + filterId);

      // Insert filter input if not already added.
      if ($(filterInput).length == 0) {
        if (table.config.filter.container)
          $(table.config.filter.container).html(f.join(''));
        else
          $(table).before(f.join(''));
      }

      var filterInput = $('input#' + filterId);

      // Enable input value on focusin/ out
      if (title) {
        $(filterInput)
             .unbind('focusout.tablesorter.filter')
             .unbind('focusin.tablesorter.filter')
             .bind('focusout.tablesorter.filter', function (e) {
               if ($(this).val() == '') {
                 $(this).addClass('empty');
                 $(this).val(title);
               }
             }).bind('focusin.tablesorter.filter', function (e) {
               if ($(this).hasClass('empty')) {
                 $(this).removeClass('empty');
                 $(this).val('');
               }
             });
      }

      // Filter table on every keyup event.
      $(filterInput)
         .unbind('keyup.tablesorter.filter')
         .bind('keyup.tablesorter.filter', function (e) {
           if (!$(this).hasClass('empty'))
             _this.filter(table, $(this).val());
         });

      // Apply filter on every table update.
      $(table)
         .unbind('update.tablesorter.filter')
         .bind('update.tablesorter.filter', function (e) {
           if (!$(filterInput).hasClass('empty'))
             _this.filter(table, $(filterInput).val());
         });

    },

    /*
    
    Method: filter
    Filter the table contents by string.
    
    Parameters:
    table - {HTML Object} The table.
    str - {String} The string to filter on.
    
    */
    filter: function (table, str) {
      $('tbody tr:hidden', table).show();
      $('tbody tr:not(:icontains("' + str + '"))', table).hide();
    },

    /*
    
    Method: format
    Will be called on init.
    
    */
    format: function (table) {
      this.init(table);
    }

  });
})(jQuery);