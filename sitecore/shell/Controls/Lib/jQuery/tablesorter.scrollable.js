/*

@requires $/$.js
@requires $/plugins/$.tablesorter.js

Tablesorter widget to enable table content scrolling with fixed header.

*/
(function ($) {
  $.tablesorter.addWidget({
    /*
    
    Property: id
    The id of this widget.
   
    */
    id: 'scrollable',

    /* 

    Create header as separate table

    */
    createHeaderTable: function (table) {
      $table = $(table);
      $tableHeader = $('<table class="' + table.className + '" style="width:100%"></table>');

      $tableHeader.append($('thead', $table));
      $tableHeader.attr('cellpadding', $table.attr('cellpadding'));
      $tableHeader.attr('cellspacing', $table.attr('cellspacing'));
      return $tableHeader;
    },

    /*
    
    Method: format
    Will be called on init.
    
    */

    format: function (table) {

      $table = $(table);

      // Check if plugin already initialized.
      if ($table.data('header')) {
        return;
      }

      $table.bind('tableresize.scrollable.tablesorter', function (e) {
        $table = $(e.target);
        $header = $table.data('header');
        $header.css('width', $table.outerWidth());
      });

      availableHeight = $table.parent().outerHeight();
      tableWidth = $table.outerWidth();
      $header = this.createHeaderTable(table);
      $table.data('header', $header);
      bodyContainer = $('<div class="tablesorterBodyContainer" style="overflow:auto">');
      $table.before(bodyContainer);
      bodyContainer.append(table);
      bodyContainer.before($header);

      $('.tablesorterBodyContainer').css('height', availableHeight - $header.outerHeight() + 'px');
      $header.css('width', $table.outerWidth());
    }
  });
}
)(jQuery);