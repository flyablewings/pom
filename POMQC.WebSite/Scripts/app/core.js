/// <reference path="../_references.js" />
var BlockedUI = false;

$(document).ajaxError(function (e, xhr, settings, exception) {
    try {
        unblockUI();
    } catch (e) {
    }
});

function blockUI () {
    if (!BlockedUI) {
        BlockedUI = true;

        $.blockUI.defaults.css.border = '0px solid #aaa';
        $.blockUI.defaults.css.backgroundColor = "transparent";
        $.blockUI.defaults.overlayCSS.backgroundColor = "transparent";

        $.blockUI({
            message: '<div class="loading" />',
            centerX: true,
            centerY: true,
            css: {
                top: '0',
                left: '0',
                width: '100%',
                height: '100%',
                color: 'transparent',
                backgroundColor: 'transparent',
                zIndex: 2222,
                opacity: 1
            }
        });
    }
}

function unblockUI () {
    if (BlockedUI) {
        BlockedUI = false;
        $.unblockUI(document);
    }
}

function showMessage(func, msg, err) {
    $(document).scrollTop(0);
    if (!err) {
        $('#errmsg' + func).removeClass('error').addClass('success').html(msg).show();
    }
    else {
        $('#errmsg' + func).removeClass('success').addClass('error').html(msg).show();
    }
}

function ProcessViewFunction(func) {
    switch (func) {
        case Func.Dashboard:
            viewDashboard();
            break;
        case Func.Inspection:
            var from = $('.datepicker')[0].value;
            var to = $('.datepicker')[1].value;
            getDHU($('#ddlCUST').val(), $('#ddlCUSTPO').val(), $('#ddlAIGLPO').val(), from, to, $('#ddlStatus option:selected').text(), $('#ddlFACT').val());
            break;        
        case Func.ReportByPO:
            viewReportByCustPO($('#ddlCUST').val(), $('#ddlCUSTPO').val(), $('#ddlAIGLPO').val(), Func.ReportByPO);
            break;
        case Func.ReportByFactory:
            var from = $('.datepicker')[0].value;
            var to = $('.datepicker')[1].value;
            viewReportByFactory($('#ddlFACTORY').val(), from, to, Func.ReportByFactory);
            break;
        case Func.ReportAllFactories:
            var from = $('.datepicker2')[0].value;
            var to = $('.datepicker2')[1].value;
            viewReportAllFactories(from, to, Func.ReportAllFactories);
            break;
        case Func.ReportFinal:
            var from = $('.datepicker3')[0].value;
            var to = $('.datepicker3')[1].value;
            viewReportAllFinal(from, to, Func.ReportFinal);
            break;
        case Func.ReportDhuDetail:
            var from = $('.datepicker4')[0].value;
            var to = $('.datepicker4')[1].value;
            viewReportAllDetail(from, to, Func.ReportDhuDetail);
            break;
        case Func.ReportDhuWeekly:
            var from = $('.datepicker5')[0].value;
            var to = $('.datepicker5')[1].value;
            viewReportDhuWeekly(from, to, Func.ReportDhuWeekly);
            break;
        case Func.ReportDhuMonthly:
            var from = $('.datepicker6')[0].value;
            var to = $('.datepicker6')[1].value;
            viewReportDhuMonthly(from, to, Func.ReportDhuMonthly);
            break;
        case Func.Sample:
            var from = $('.datepicker')[0].value;
            var to = $('.datepicker')[1].value;
            viewSample($('#ddlCUST').val(), $('#ddlCUSTPO').val(), $('#ddlAIGLPO').val(), from, to, $('#ddlStatus option:selected').text(), $('#ddlFACT').val());
            break;
        case Checklist.QAChecklist:
            var from = $('.datepicker')[0].value;
            var to = $('.datepicker')[1].value;
            viewChecklist($('#ddlCUST').val(), $('#ddlCUSTPO').val(), $('#ddlAIGLPO').val(), from, to, $('#ddlStatus option:selected').text(), $('#ddlFACT').val());
            break;
        case Checklist.PPMeeting:
            var from = $('.datepicker')[0].value;
            var to = $('.datepicker')[1].value;
            viewMeeting($('#ddlCUST').val(), $('#ddlCUSTPO').val(), $('#ddlAIGLPO').val(), from, to, $('#ddlStatus option:selected').text(), $('#ddlFACT').val());
            break;
        default:
            break;
    }
}

function ProcessNewFunction(func) {
    switch (func) {
        case Func.Sample:
        case Func.FitSample:
        case Func.PPSample:
        case Func.TopSample:
            NewSample();
            break;
        case Func.Checklist:
            NewChecklist();
            break;
        case Func.Meeting:
            NewMeeting();
            break;
        case Func.Inspection:
        case Func.Fabric:
        case Func.Cutting:
        case Func.Inline:
        case Func.Endline:
        case Func.Finishing:
        case Func.Packing:
        case Func.Prefinal:
        case Func.Final:
            NewInspection();
            break;
        default:
            break;        
    }
};

function exportExcel(tableId, fn) {
    var title = $('#report-title' + fn).html();
    $("#" + tableId).table2excel({
        name: title,
        filename: title,
        fileext: ".xls",
        exclude_img: true,
        exclude_links: true,
        exclude_inputs: true
    });
};

function exportExcelDHU(custId, custPO, aiglPO, date, type, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('dhu/export'),
        type: 'post',
        data: {
            custId: custId,
            custPO: custPO,
            aiglPO: aiglPO,
            date: date,
            type: type
        },
        dataType: 'text/html'
    }).complete(function (result) {
        unblockUI();
        /*var file = getDHUType(type) + '-' + custPO + '-' + aiglPO + '-' + date;
        $(result.responseText).table2excel({
            name: file,
            filename: file,
            fileext: ".xls",
            exclude_img: true,
            exclude_links: true,
            exclude_inputs: true
        });*/
    });
};

function exportExcelFinal(custId, custPO, aiglPO, date, type, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('final/export'),
        type: 'post',
        data: {
            custId: custId,
            custPO: custPO,
            aiglPO: aiglPO,
            date: date,
            type: type
        },
        dataType: 'text/html'
    }).complete(function (result) {
        unblockUI();
        /*var file = getDHUType(type) + '-' + custPO + '-' + aiglPO + '-' + date;
        $(result.responseText).table2excel({
            name: file,
            filename: file,
            fileext: ".xls",
            exclude_img: true,
            exclude_links: true,
            exclude_inputs: true
        });*/
    });
};

function sendMail(isDHU, custid, custpo, aiglpo, date, type) {
    blockUI();
    var popup = $('<div></div>');
    var param = '?custid=' + custid + '&custpo=' + custpo + '&aiglpo=' + aiglpo + '&date=' + date + '&type=' + type;
    $.ajax({
        url: resolveClientUrl((isDHU ? 'dhu/mailto' : 'final/mailto') + param),
        type: 'get'
    }).success(function (result) {
        popup.html(result);
        popup.dialog({ modal: true, width: 500, height: 400, title: 'Compose Email', close: function () { popup.remove(); } });
        unblockUI();
    }).error(function (result) {
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function doSendMail(url) {
    var to = $('#To').val();
    var from = $('#From').val();

    if ($.trim(from) == '') {
        $('#errmsg').removeClass('success').addClass('error').html('Please configure your email address').show();
        return;
    }

    if ($.trim(to) == '') {
        $('#errmsg').removeClass('success').addClass('error').html('Please input receiver email address').show();
        return;
    }

    blockUI();
    $.ajax({
        url: url + 
            '&to=' + $('#To').val() +
            '&cc=' + $('#Cc').val() +
            '&bcc=' + $('#Bcc').val() +
            '&subject=' + $('#Subject').val() +
            '&body=' + $('#Body').val(),
        type: 'post'
    }).success(function (result) {
        if (result.isSuccess) {
            $('#errmsg').removeClass('error').addClass('success').html('Sent mail successfully').show();
        }
        else {
            $('#errmsg').removeClass('success').addClass('error').html('Cannot send mail, please recheck your input information').show();
        }
        unblockUI();
    }).error(function (result) {
        $('#errmsg').removeClass('success').addClass('error').html(result.responseText).show();
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function configEmail() {
    blockUI();
    var popup = $('<div></div>');
    $.ajax({
        url: resolveClientUrl('account/configemail'),
        type: 'get'
    }).success(function (result) {
        popup.html(result);
        popup.dialog({ modal: true, width: 400, height: 200, title: 'Configure Email', close: function () { popup.remove(); } });
        unblockUI();
    }).error(function (result) {
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

function doConfigEmail() {
    var email = $('#Email').val();
    var password = $('#Password').val();

    if ($.trim(email) == '') {
        $('#errmsgs').removeClass('success').addClass('error').html('Please input your email address').show();
        return;
    }

    if ($.trim(password) == '') {
        $('#errmsgs').removeClass('success').addClass('error').html('Please input your email password').show();
        return;
    }

    blockUI();
    $.ajax({
        url: resolveClientUrl('account/configemailpost'),
        data: { email: email, password: password },
        type: 'post'
    }).success(function (result) {
        if (result.IsSuccess) {
            $('#errmsgs').removeClass('error').addClass('success').html('Configured mail successfully').show();
            $('#From').val(email);
        }
        else {
            $('#errmsgs').removeClass('success').addClass('error').html(result.Message).show();
        }
        unblockUI();
    }).error(function (result) {
        $('#errmsgs').removeClass('success').addClass('error').html(result.responseText).show();
        unblockUI();
    }).complete(function (result) {
        unblockUI();
    });
}

var xhr = null;
function Checkin() {
    if (xhr != null) {
        xhr.abort();
    }

    xhr = $.ajax({
        url: resolveClientUrl('account/checkin'),
        type: 'post'
    }).success(function (isOK) {
        xhr = null;
        if (!isOK) {
            top.location.reload(true);
        }
    });
}

function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
      .toString(16)
      .substring(1);
    }
    return s4() + s4() + '_' + s4() + '_' + s4() + '_' +
    s4() + '_' + s4() + s4() + s4();
}

setInterval(function () {
    Checkin();
}, 15000)