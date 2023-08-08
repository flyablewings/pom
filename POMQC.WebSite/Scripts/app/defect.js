/// <reference path="../_references.js" />
function addDefectCategory(id) {
    blockUI();
    var popup = $('<div></div>');
    $.ajax({
        url: resolveClientUrl('defect/category?id=' + id),
        type: 'post'
    }).success(function (result) {
        popup.html(result);
        popup.dialog({ modal: true, width: 400, height: 234, title: 'Defect Category', close: function () { popup.remove(); } });
        unblockUI();
    }).error(function (result) {
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function addDefectCode(id) {
    blockUI();
    var popup = $('<div></div>');
    $.ajax({
        url: resolveClientUrl('defect/code?id=' + id),
        type: 'post'
    }).success(function (result) {
        popup.html(result);
        popup.dialog({ modal: true, width: 400, height: 350, title: 'Defect Description', close: function () { popup.remove(); } });
        unblockUI();
    }).error(function (result) {
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function addDefectLocation(id) {
    blockUI();
    var popup = $('<div></div>');
    $.ajax({
        url: resolveClientUrl('defect/location?id=' + id),
        type: 'post'
    }).success(function (result) {
        popup.html(result);
        popup.dialog({ modal: true, width: 400, height: 234, title: 'Defect Location', close: function () { popup.remove(); } });
        unblockUI();
    }).error(function (result) {
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function addCategory() {
    if ($.trim($('#CatName').val()) == '') {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html('Please input Category Name');
        return;
    }

    blockUI();
    var catId = parseInt($('#catid').val());
    $.ajax({
        url: resolveClientUrl(catId == 0 ? 'defect/addcategory' : 'defect/editcategory'),
        type: 'post',
        data: {
            catId: catId,
            catName: $('#CatName').val(),
            active: $('input[id="Active"]').is(':checked')
        }
    }).success(function (result) {
        $('#errmsg').removeClass('success').removeClass('error').addClass(result.IsSuccess ? 'success' : 'error').show().html(result.Message);
        unblockUI();
    }).error(function (result) {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html(result.statusText);
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function addCode() {
    if ($.trim($('#DefCode').val()) == '') {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html('Please input Defect Code');
        return;
    }

    if ($.trim($('#DefName').val()) == '') {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html('Please input Defect Description');
        return;
    }

    blockUI();
    var catId = parseInt($('#CatId option:selected').val());
    var defId = parseInt($('#defid').val());
    $.ajax({
        url: resolveClientUrl(defId == 0 ? 'defect/addcode' : 'defect/editcode'),
        type: 'post',
        data: {
            catId: catId,
            defId: defId,
            defCode: $('#DefCode').val(),
            defName: $('#DefName').val(),
            defVN: $('#DefVN').val(),
            active: $('input[id="Active"]').is(':checked'),
            type: $('input[id="Type"]').is(':checked') ? 1 : 2
        }
    }).success(function (result) {
        $('#errmsg').removeClass('success').removeClass('error').addClass(result.IsSuccess ? 'success' : 'error').show().html(result.Message);
        unblockUI();
    }).error(function (result) {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html(result.statusText);
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function addLocation() {
    if ($.trim($('#LocName').val()) == '') {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html('Please input Location Name');
        return;
    }

    blockUI();
    var locId = parseInt($('#locid').val());
    $.ajax({
        url: resolveClientUrl(locId == 0 ? 'defect/addlocation' : 'defect/editlocation'),
        type: 'post',
        data: {
            locId: locId,
            locName: $('#LocName').val(),
            active: $('input[id="Active"]').is(':checked')
        }
    }).success(function (result) {
        $('#errmsg').removeClass('success').removeClass('error').addClass(result.IsSuccess ? 'success' : 'error').show().html(result.Message);
        unblockUI();
    }).error(function (result) {
        $('#errmsg').removeClass('success').removeClass('error').addClass('error').show().html(result.statusText);
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}