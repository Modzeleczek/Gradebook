$(function () {
    // const tables = document.querySelectorAll('.list-table');
    const tables = $('.list-table');
    tables.each((index, htmlElement) => {
        const $table = $(htmlElement);
        $table.addClass('table table-bordered table-striped');
        $table.each((tableIndex, table) => {
            const theads = $(table).children('thead');
            if (theads.length != 1) throw new DOMException("table does not have exactly 1 thead");
            theads.addClass('thead-dark');
            const trs = $(theads[0]).children('tr');
            if (trs.length != 1) throw new DOMException("thead does not have exactly 1 tr");
            const ths = $(trs[0]).children('th');
            ths.each((thIndex, th) => {
                if (th.innerText.length == 0)
                    $(th).addClass("sorter-false filter-false");
            });
        });
        if ($table.hasClass("tablesorter_no_headers")) return;
        $table.tablesorter({
            theme: "blue",
            widthFixed: true,
            // widget code contained in the jquery.tablesorter.widgets.js file
            // use the zebra stripe widget if you plan on hiding any rows (filter widget)
            // the uitheme widget is NOT REQUIRED!
            widgets: ["filter", "columns"],
            widgetOptions: {
                // class names added to columns when sorted
                columns: ["primary", "secondary", "tertiary"],
                // reset filters button
                filter_reset: ".reset",
                // extra css class name (string or array) added to the filter element (input or select)
                filter_cssFilter: [
                    'form-control',
                    'form-control',
                    'form-control custom-select', // select needs custom class names :(
                    'form-control',
                    'form-control',
                    'form-control',
                    'form-control'
                ]
            }
        })
        .tablesorterPager({
            // target the pager markup - see the HTML block below
            container: $(".ts-pager"),
            // target the pager page select dropdown - choose a page
            cssGoto: ".pagenum",
            // remove rows from the table to speed up the sort of large tables.
            // setting this to false, only hides the non-visible rows; needed if you plan to add/remove rows with the pager enabled.
            removeRows: false,
            // output string - default is '{page}/{totalPages}';
            // possible variables: {page}, {totalPages}, {filteredPages}, {startRow}, {endRow}, {filteredRows} and {totalRows}
            output: '{startRow} - {endRow} / {filteredRows} ({totalRows})'
        });
    })
});
