$(document).on('shown.bs.modal', '.modal', function () {
    SetDefaultComponentFocus(this);
    $("div.table-scrollable:visible", $(this)).each(function () {
        SetScrolltableFixedHeader($(this));
    });
});

var closingModal = new Array();
$(document).on('hidden.bs.modal', '.modal', function () {
    if ($(this).attr('ignore-close-action') != 'true') {
        if (typeof $('form', this).attr('cancel-action') != 'undefined' && closingModal.indexOf($(this).attr('id')) < 0) {
            $.ajax({
                type: "POST",
                url: $('form', this).attr('cancel-action'),
                data: { 'objectId': $('#ID', this).val() },
                beforeSend: AjaxBegin,
                complete: AjaxComplete,
                error: AjaxFailure
            });
        }
        else {
            var index = closingModal.indexOf($(this).attr('id'));
            if (index !== -1) closingModal.splice(closingModal, 1);
        }
    }
    else
        $(this).removeAttr('ignore-close-action');
});

function OpenModal(modal) {
    $(modal).modal({ backdrop: 'static' });
    $(modal).css('padding-right', ''); //For some reason, it adds a style padding-right 17px, need to remove

    if ($(modal).is('[class*="modal-level"]')) {
        var classes = $(modal).attr('class').split(" ");
        var i = 0;
        while (i < classes.length) {
            if (classes[i].indexOf('modal-level') >= 0) {
                $('.modal-backdrop:not([class*="modal-level"]').addClass(classes[i]);
                break;
            }
            i++;
        }
    }

    if ($('#' + $(modal).attr('id').replace('_modal', '') + '_Search').length > 0) {
        ClearSearchProperties($('#' + $(modal).attr('id').replace('_modal', '') + '_Search'));
    }

    PopulateDatePickers(modal);
}

function CloseModal(modal) {
    closingModal.push($(modal).attr('id'))
    $(modal).modal('hide');
}


function SetFormCustomAction(form, action) {
    if (typeof form.attr('data-postaction') != 'undefined') {
        form.attr('data-original-action', form.attr('data-postaction'));
        form.attr('data-postaction', action);
    }
    else {
        form.attr('data-original-action', form.attr('action'));
        form.attr('action', action);
    }
}

function ClearFormCustomAction(form) {
    if (typeof form.attr('data-original-action') != 'undefined') {
        if (typeof form.attr('data-postaction') != 'undefined') {
            form.attr('data-postaction', form.attr('data-original-action'));
        }
        else {
            form.attr('action', form.attr('data-original-action'));
        }
        form.removeAttr('data-original-action');
    }
}


function GetPostbackDataLookupModal(form) {
    var modalId = form.attr('id').replace('_Search', '');

    $('#' + modalId + '_Search_PostbackActionName', form).val($('#' + modalId + '_PostbackActionName').val());
    $('#' + modalId + '_Search_PostbackFormId', form).val($('#' + modalId + '_PostbackFormId').val());
}

function PostbackLookupModal(lookupId) {
    var form = $('#' + $('#' + lookupId + '_PostbackFormId').val() + '_modal form');

    var container = $(form.attr('data-ajax-update'));
    $('#' + lookupId + ' input[type="checkbox"]:checked').each(function () {
        container.append($('<input>').attr('type', 'hidden').attr('name', 'LookupSelection').val($(this).val()));
    });

    SetFormCustomAction(form, $('#' + lookupId + '_PostbackActionName').val());
    CloseModal('#' + lookupId + '_modal');
    form.submit();
}