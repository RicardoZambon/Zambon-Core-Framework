function PopulateDatePickers(element) {
    $('.datepicker', $(element)).each(function () {
        $(this).datepicker({
            altField: $(this).attr('data-alt-field'),
            altFormat: $(this).attr('data-alt-format')
        });

        var val = $(this).val();
        if (typeof $(this).attr('data-date-format') != 'undefined') {
            $(this).datepicker('option', 'dateFormat', $(this).attr('data-date-format'));
        }
        $(this).datepicker('setDate', val);
    });
}