var keepLoadingVisible = false;

function AjaxBegin() {
    ShowLoader(this);
}

function AjaxFailure(xhr, status, error) {
    var err = xhr.responseText;
    if (err == '')
        err = error;

    ShowMessage(err, 'alert-danger', 2000);
    console.log(err);
}

function AjaxSuccess(data, status, xhr) {
    if (typeof data.value != 'undefined') {
        if (typeof data.value.submitViewId != 'undefined' && data.value.submitViewId != '') {

            if (GetLoaderBlurContainer(this) == GetLoaderBlurContainer($(data.value.submitViewId)))
                keepLoadingVisible = true;

            var view = $(data.value.submitViewId);

            var viewContainer = $(view.attr('data-ajax-update'));
            if (typeof data.value.submitViewParameters != 'undefined' && data.value.submitViewParameters.length > 0) {
                $.each(data.value.submitViewParameters, function (key, value) {
                    viewContainer.append($('<input type="hidden" name="' + key + '" value="' + value + '" />'));
                });
            }

            if (typeof data.value.submitViewAction != 'undefined' && data.value.submitViewAction != '') {
                SetFormCustomAction(view, data.value.submitViewAction);
            }

            if (typeof data.value.message != 'undefined' && data.value.message != '') {
                view.attr('message-content', data.value.message);
                view.attr('message-class', data.value.messageClass);
            }

            view.submit();
        }
        else {
            keepLoadingVisible = false;

            if (typeof data.value.message != 'undefined' && data.value.message != '')
                ShowMessage(data.value.message, data.value.messageClass, 2000);
        }

        if (typeof data.value.closeModalId != 'undefined' && data.value.closeModalId != '') {
            $(data.value.closeModalId).attr('ignore-close-action', 'true');
            CloseModal($(data.value.closeModalId));
        }
    }
    else
        keepLoadingVisible = false;

    if (!keepLoadingVisible)
        UpdateMenuBagdesCount();

    if (typeof $(this).attr('data-ajax-open-modal') != 'undefined')
        OpenModal($($(this).attr('data-ajax-open-modal') + '_modal'));
}

function AjaxComplete() {
    if (!keepLoadingVisible) {
        HideLoader(this);

        if (typeof $(this).attr('message-content') != 'undefined' && $(this).attr('message-content') != '') {
            ShowMessage($(this).attr('message-content'), $(this).attr('message-class'), 2000);
            $(this).removeAttr('message-content');
            $(this).removeAttr('message-class');
        }

        if (typeof SetDefaultComponentFocus == 'function')
            SetDefaultComponentFocus(this);

        if (typeof ClearFormCustomAction == 'function')
            ClearFormCustomAction($(this));

        if (typeof $(this).attr('data-ajax-update') != 'undefined' && typeof SetScrolltableFixedHeader == 'function')
            $('div.table-scrollable:visible', $(this).attr('data-ajax-update')).each(function () {
                SetScrolltableFixedHeader($(this));
            });
    }
    if (typeof PopulateDatePickers == 'function')
        PopulateDatePickers(this);
}


function ShowLoader(form) {
    var element = $(GetLoaderContainer(form));
    var blur = $(GetLoaderBlurContainer(form));

    if (!blur.hasClass('content-blur'))
        blur.addClass('content-blur');

    if ($('> .loading', element).length == 0)
        element.append('<div class="loading"></div>');
}

function HideLoader(form) {
    var element = $(GetLoaderContainer(form));
    var blur = $(GetLoaderBlurContainer(form));

    blur.removeClass('content-blur');
    $('> .loading', element).remove();
}


function GetLoaderContainer(form) {
    if (typeof $(form).attr('data-ajax-update-loading') != 'undefined' && $(form).attr('data-ajax-update-loading') != 'body')
        return $(form).attr('data-ajax-update-loading');
    return 'body';
}

function GetLoaderBlurContainer(form) {
    if (GetLoaderContainer(form) != 'body' && $($(form).attr('data-ajax-update-loading') + ' .blur ').length > 0)
        return $(form).attr('data-ajax-update-loading') + ' .blur';
    return '#MainContent';
}


function ShowMessage(message, classes, delay) {
    var currentDate = new Date();
    var datetime = '<b>' + ('0' + currentDate.getHours()).slice(-2) + ':' + ('0' + currentDate.getMinutes()).slice(-2) + ':' + ('0' + currentDate.getSeconds()).slice(-2) + '</b> - ';

    var messageBox =
        $('<div class="alert ' + classes + ' alert-dismissible p-2 pl-3 pr-5" role="alert">' +
            '<button type="button" class="close p-2 pr-3" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' + datetime + message +
            '</div>');

    messageBox.appendTo($('.alert-container'));
    if (delay > 0)
        setTimeout(function () {
            messageBox.fadeOut(500);
            messageBox.detach();
        }, delay);
}


$(document).on('click', 'a[data-form-post]', function (event) {
    var postForm = true;
    if (typeof $(this).attr('data-ajax-confirm') != 'undefined') {
        postForm = confirm($(this).attr('data-ajax-confirm'));
    }

    if (postForm) {
        var form = $(this).closest('form');
        if (typeof $(this).attr('href') != 'undefined') {
            SetFormCustomAction(form, $(this).attr('href'));
        }
        form.submit();
    }
    return false;
});

var prev_selectValue = null;
$(document).on('focus', 'select[data-form-post]', function (event) {
    prev_selectValue = $(this).val();
});
$(document).on('change', 'select[data-form-post]', function (event) {
    var postForm = true;
    if (typeof $(this).attr('data-ajax-confirm') != 'undefined') {
        postForm = confirm($(this).attr('data-ajax-confirm'));
    }

    if (postForm) {
        prev_selectValue = true;

        var form = $(this).closest('form');
        if (typeof $(this).attr('data-ajax-action') != 'undefined') {
            SetFormCustomAction(form, $(this).attr('data-ajax-action'));
        }
        form.submit();
    }
    else
        $(this).val(prev_selectValue);
});


function ajax_update_parent(action, button) {
    var form = $(button).closest('form');

    return $.ajax({
        async: false,
        type: "POST",
        url: action,
        processData: false,
        data: form.serialize(),
        beforeSend: function () {
            ShowLoader(form);
        },
        complete: function () {
            HideLoader(form);
            AjaxComplete();
        },
        success: function (data, status, xhr) {
            return true;
        },
        error: AjaxFailure
    });
}