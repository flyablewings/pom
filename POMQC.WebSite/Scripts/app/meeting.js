/// <reference path="../_references.js" />
function NewMeeting() {
    blockUI();

    $('#isUpdate' + Func.Meeting).val('0');
    $('#comment' + Func.Meeting).val('');
    $('input[name="img' + Func.Meeting + '"]').val('');
    var dropzone = Dropzone.forElement('#dropzoneForm' + Func.Meeting);
    try {
        dropzone.removeAllFiles();
    } catch (e) { }

    $('#dropzoneForm' + Func.Meeting + ' div.dz-preview').remove();

    setTimeout(function () {
        unblockUI();
    }, 200);
}

function viewMeeting(custId, custPO, aiglPO, from, to, status, factoryId) {
    NewMeeting();
    blockUI();

    var custId = $('#ddlCUST').val();
    var custPO = $('#ddlCUSTPO').val();
    var aiglPO = $('#ddlAIGLPO').val();
    var from = $('.datepicker')[0].value;
    var to = $('.datepicker')[1].value;

    $.ajax({
        url: resolveClientUrl('dashboard/view'),
        type: 'post',
        data: {
            factoryId: factoryId,
            custId: custId,
            custPO: (custPO == 0 ? '' : custPO),
            aiglPO: (aiglPO == 0 ? '' : aiglPO),
            from: from,
            to: to,
            custName: $('#ddlCUST option:selected').text(),
            status: status
        }
    }).success(function (result) {
        loadMeetingTable(result.POs);

        filterChecklist(Checklist.PPMeeting, result.PPMeeting);

        unblockUI();
    });
}

function meetingDetail(owner, custId, custPO, aiglPO, custName, factoryId) {
    var table = $(owner).parent();
    for (var i = 0; i < table[0].rows.length; i++) {
        $(table[0].rows[i]).removeClass('selected-row');
    }

    $(owner).addClass('selected-row');

    var from = $('.datepicker')[0].value;
    var to = $('.datepicker')[1].value;

    NewMeeting();
    blockUI();

    $.ajax({
        url: resolveClientUrl('dashboard/viewdetail'),
        type: 'post',
        data: {
            factoryId: factoryId,
            custId: custId,
            custPO: (custPO == 0 ? '' : custPO),
            aiglPO: (aiglPO == 0 ? '' : aiglPO),
            from: from,
            to: to,
            custName: custName
        }
    }).success(function (result) {
        filterChecklist(Checklist.PPMeeting, result.PPMeeting);

        unblockUI();
    });
}

function loadMeetingTable(POs) {
    $('#main tbody tr').remove();
    if (POs.length == 0) {
        $('#main tbody').append('<tr><td colspan="9" class="c">There is no information</td></tr>');
    }
    else {
        for (var i = 0; i < POs.length; i++) {
            var po = POs[i];
            $('#main').append('<tr class="' + (i == 0 ? 'selected-row' : '') + '" onclick="meetingDetail(this, ' + po.CustId + ', \'' + po.CustPO + '\', \'' + po.AIGLPO + '\', \'' + po.Customer + '\', ' + po.FactoryId + ')"><td>' + po.AIGLPO + '</td><td>' + po.CustPO + '</td><td>' + po.Customer + '</td><td>' + po.Factory + '</td><td>' + po.Division + '</td><td>' + po.InlineDate + '</td><td>' + po.Status + '</td><td class="r">' + po.POQty + '</td><td class="r">' + po.Defect + '</td></tr>');
        }
    }
}