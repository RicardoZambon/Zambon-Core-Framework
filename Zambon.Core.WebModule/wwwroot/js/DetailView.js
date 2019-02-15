$(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
    var activeTab = $(e.target.href.substring(e.target.href.indexOf('#'), e.target.href.length));
    SetDefaultComponentFocus(activeTab);
    $("div.table-scrollable:visible", activeTab).each(function () {
        SetScrolltableFixedHeader($("div.table-scrollable:visible", activeTab));
    });

    var form = $(this).closest('form');
    $('#' + form.attr('id') + '_CurrentTabId', form).val(activeTab.attr('id'));
});

function SetDefaultComponentFocus(wrapper) {
    if ($(wrapper).find('[autofocus]').length > 0) {
        var element = $(wrapper).find('[autofocus]');
        element.focus();

        var tmp = element.val();
        element.val('');
        element.val(tmp);
    }
}

function FormUploadSubmit(form) {
    var data = new FormData($(form)[0]);
    $.each($("input[type=file]", form), function (index, element) {
        var file = $(element).get(0);
        var files = file.files;
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
    });

    $.ajax({
        type: "POST",
        url: $(form).attr("data-postaction"),
        contentType: false,
        processData: false,
        data: data,
        beforeSend: AjaxBegin,
        complete: AjaxComplete,
        success: function (data, status, xhr) {
            if (xhr.getResponseHeader('content-type').indexOf('text/html') >= 0)
                $($(form).attr("data-postupdate")).html(data);
            else
                AjaxSuccess(data, status, xhr)
        },
        error: AjaxFailure
    });
    return false;
}