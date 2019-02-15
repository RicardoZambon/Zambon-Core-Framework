/*
Usage example:

    AutoOpenView=ListView_User
    
    &AutoOpenModal=ModalView_User
    &AutoOpenModalAction=/Employees/Edit
    &AutoOpenModalParameters="objectId":159
*/

$(document).ready(function () {

    var menu = getUrlParameter('AutoOpenViewId');
    if (menu != 'undefined' && menu.length > 0) {
        AutoOpenView(menu);        
    }

});

function AutoOpenView(id) {
    $.ajax({
        type: 'POST',
        url: urlPath + '/View/ListView',
        data: { ViewId: id },
        beforeSend: AjaxBegin,
        complete: function () {
            AjaxComplete();
            //RefreshMenuBadgesCount();

            var modal = getUrlParameter('AutoOpenModalId');
            var view = getUrlParameter('AutoOpenModalViewId');

            var action = getUrlParameter('AutoOpenModalAction');
            
            if ((modal != 'undefined' && modal.length > 0)
                && (action != 'undefined' && action.length > 0)
                && (view != 'undefined' && view.length > 0)) {

                var parameters = getUrlParameter('AutoOpenModalParameters');

                var params = '{ "ParentViewId": "' + id + '", "ModalId": "' + modal + '", "ViewId": "' + view + '"';
                if (parameters != 'undefined' && parameters.length > 0)
                    params = params + ', ' + parameters;
                params = params + ' }';

                AutoOpenModal(modal, action, JSON.parse(params));
            }
        },
        success: function (data, status, xhr) {
            if (xhr.getResponseHeader('content-type').indexOf('text/html') >= 0)
                $('#bodyContent').html(data);
            else
                AjaxSuccess(data, status, xhr);
        },
        error: AjaxFailure
    });

}

//id: ModalView_Requests
//action: /Controller/ActionName
//parameters: { ListViewId: '', ModalViewId: '', ... }

function AutoOpenModal(id, action, parameters) {
    $.ajax({
        type: 'POST',
        url: urlPath + action,
        data: parameters,
        beforeSend: AjaxBegin,
        complete: AjaxComplete,
        success: function (data, status, xhr) {
            if (xhr.getResponseHeader('content-type').indexOf('text/html') >= 0)
            {
                $('#' + id + '_container').html(data);
                OpenModal('#' + id + '_modal');
            } else
                AjaxSuccess(data, status, xhr);
        },
        error: AjaxFailure
    });
}

function getUrlParameter(k) {
    var p = {};
    location.search.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (s, k, v) { p[k] = v })
    return k ? decodeURIComponent(p[k]) : p;
};