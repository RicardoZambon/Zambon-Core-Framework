function ChangeSearchProperty(obj) {
    var selected = $(obj).find(':selected');
    if (typeof selected != 'undefined') {

        var searchType = 'Text';
        if (typeof selected.attr('data-search-type') != 'undefined')
            searchType = selected.attr('data-search-type');

        var form = $(obj).closest('form');
        var defaultValue = selected.attr('data-search-default-value');

        HideSearchProperties(form);
        ClearSearchProperties(form);

        switch (searchType) {
            case 'Number':
                $('.search-number1', form).show();
                $('.search-number1', form).val(defaultValue);
                break;
            case 'NumberRange':
                $('.search-number1', form).show();
                $('.search-number2', form).show();
                break;
            case 'DateTime':
                $('.search-datepicker1', form).show();
                $('.search-datepicker1', form).val(defaultValue);
                break;
            case 'DateTimeRange':
                $('.search-datepicker1', form).show();
                $('.search-datepicker2', form).show();

                if (defaultValue == '[ThisMonth]') {
                    var currentDate = new Date();
                    $('.search-datepicker1').datepicker('setDate', new Date(currentDate.getFullYear(), currentDate.getMonth(), 1));
                    $('.search-datepicker2').datepicker('setDate', new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0));
                }
                break;
            default:
                $('.search-text', form).show();
                $('.search-text', form).val(defaultValue);
                break;
        }
    }
}

function HideSearchProperties(form) {
    $('.search-text', form).hide();
    $('.search-number1', form).hide();
    $('.search-number2', form).hide();
    $('.search-datepicker1', form).hide();
    $('.search-datepicker2', form).hide();
}
function ClearSearchProperties(form) {
    $('.search-text', form).val('');
    $('.search-number1', form).val('');
    $('.search-number2', form).val('');
    $('.search-datepicker1', form).val('');
    $('.search-datepicker2', form).val('');
    $('.search-date1', form).val('');
    $('.search-date2', form).val('');
}

function SetColumnsFitSameWidth(container) {
    $(container).each(function () {
        var table = $(this);
        $('.row:first .col-fit', table).each(function () {
            var width = 0;
            var index = $(this).parent().index() + 1;

            $('.row > div:nth-child(' + index + ')', table).each(function () { if ($(this).width() > width) width = $(this).width(); });
            $('.row > div:nth-child(' + index + ')', table).each(function () { $(this).width(width); });
        });
    });
}

function SetTablesScrollable(form) {
    if ($('.table-scrollable', form).length > 0) {
        $('.table-scrollable', form).each(function () {
            var container = $('<div></div>');
            container.addClass($(this).attr('class'));
            $('div:first', this).detach().appendTo(container);

            $(this).parent().prepend(container);

            $('tr:first th', this).each(function (index) {
                var column = $('th:eq(' + index + ')', table);
                if (column.hasClass('col-fit'))
                    column.removeClass('col-fit');
                column.width($(this).width());
            });
        });
    }
}

$(document).on('click', '.table-clickable-rows tr td', function () {
    var row = $(this).is('tr') ? $(this) : $(this).closest('tr');
    var checkbox = $('input[type=checkbox]', row);
    checkbox.prop('checked', !checkbox.prop('checked'));
});

function SetScrolltableFixedHeader(div) {
    if ($('.table-fixed-header', div).length == 0) {
        hasTableHeaderVisible = true;

        var fixedHeader = $('<div class="table-fixed-header"></div>');
        var table = $('.table', div);

        if (table.hasClass('rounded'))
            fixedHeader.addClass('rounded-top');

        fixedHeader.append('<table></table>');
        $('table', fixedHeader).prop('class', table.prop('class'));
        $('table', fixedHeader).removeClass('table-clickable-rows');
        $('table', fixedHeader).removeClass('position-relative');

        $('table', fixedHeader).append('<thead><tr></tr></thead>');
        fixedHeader.prop('class', fixedHeader.prop('class') + ' ' + $('thead', table).prop('class'));

        $('thead tr th', table).each(function (index) {
            var col = $('<th></th>');
            var width = $(this).outerWidth();

            col.prop('class', $(this).prop('class'));
            if (index > 0) {
                col.removeClass('col-0');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-0');
                col.removeClass('col-1');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-1');
                col.removeClass('col-2');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-2');
                col.removeClass('col-3');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-3');
                col.removeClass('col-4');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-4');
                col.removeClass('col-5');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-5');
                col.removeClass('col-6');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-6');
                col.removeClass('col-7');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-7');
                col.removeClass('col-8');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-8');
                col.removeClass('col-9');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-9');
                col.removeClass('col-10');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-10');
                col.removeClass('col-11');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-11');
                col.removeClass('col-12');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-12');
                col.removeClass('col-auto');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-auto');
                col.removeClass('col-fit');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col-fit');
                col.removeClass('col');
                $('tbody tr > *:nth-child(' + (index + 1) + ')', table).removeClass('col');
            }

            col.html($(this).html());

            if (width > 0) {
                col.outerWidth(width);
                $('tbody tr:first > *:eq(' + index + ')', table).outerWidth(width);
                //$('tbody tr:first > *:eq(' + index + ')', table).css('width', width + 'px !important');
            }

            $('thead tr', fixedHeader).append(col);
            //$('tbody tr:first > *:eq(' + index + ')', table).prop('class', $('tbody tr:first > *:eq(' + index + ')', table).prop('class') + ' ' + $(this).prop('class'));
        });

        div.prepend(fixedHeader);
        var div = $('<div class="table-container"></div>');
        div.css('margin-top', $('thead', fixedHeader).outerHeight());
        table.wrap(div);

        var lastCol = $('<th></th>');
        lastCol.outerWidth($('thead', fixedHeader).outerWidth() - $('thead', table).outerWidth());
        $('thead tr', fixedHeader).append(lastCol);

        $('thead', table).css('display', 'none');
    }
}

function UpdateMenuBagdesCount() {
    var updatedListViews = []
    $('a[menu-has-badge]').each(function () {
        var menuItem = $(this);
        var listView = $(this).attr('menu-has-badge');
        if (updatedListViews.indexOf(listView) < 0) {
            updatedListViews.push(listView);

            $.ajax({
                type: 'POST',
                url: urlPath + '/View/GetBagdeItemsCount',
                data: { ListViewId: listView },
                success: function (data, status, xhr) {
                    var badge = $('a[menu-has-badge='+listView+'] .badge');
                    badge.text(data);
                    if (data > 0) {
                        if (badge.hasClass('badge-light') || badge.hasClass('badge-secondary')) {
                            badge.addClass('badge-warning');
                            badge.removeClass('badge-light');
                            badge.removeClass('badge-secondary');
                        }
                    }
                    else {
                        if (badge.hasClass('badge-warning')) {
                            if (badge.hasClass('nav-link'))
                                badge.addClass('badge-light');
                            else 
                                badge.addClass('badge-secondary');
                            badge.removeClass('badge-warning');
                        }
                    }
                }
            });
        }
    });
}