(function () {
    "use strict";
    var $ = jQuery;
    $.fn.extend({
        filterTable: function () {
            return this.each(function () {
                $(this).on("keyup", function () {
                    $(".filterTable_no_results").remove();
                    var $this = $(this),
                        search = $this.val().toLowerCase(),
                        target = $this.attr("data-filters"),
                        $target = $(target),
                        $rows = $target.find("tbody tr");

                    if (search === "") {
                        $rows.show();
                    } else {
                        $rows.each(function () {
                            var $this = $(this);
                            $this.text().toLowerCase().indexOf(search) === -1 ? $this.hide() : $this.show();
                        });
                        if ($target.find("tbody tr:visible").size() === 0) {
                            var colCount = $target.find("tr").first().find("td").size();
                            var noResults = $('<tr class="filterTable_no_results"><td colspan="' + colCount + '">No results found</td></tr>');
                            $target.find("tbody").append(noResults);
                        }
                    }
                });
            });
        }
    });
    $('[data-action="filter"]').filterTable();
})(jQuery);

$(function () {
    // attach table filter plugin to inputs
    $('[data-action="filter"]').filterTable();

    $(".container").on("click", ".panel-heading span.filter", function () {
        var $this = $(this),
            $panel = $this.parents(".panel");

        $panel.find(".panel-body").slideToggle();
        if ($this.css("display") !== "none") {
            $panel.find(".panel-body input").focus();
        }
    });
    $('[data-toggle="tooltip"]').tooltip();
});