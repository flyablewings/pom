/// <reference path="../_references.js" />
function NewChecklist() {
    blockUI();

    $('#isUpdate' + Func.Checklist).val('0');
    $('#comment' + Func.Checklist).val('');
    $('input[name="img' + Func.Checklist + '"]').val('');
    var dropzone = Dropzone.forElement('#dropzoneForm' + Func.Checklist);
    try {
        dropzone.removeAllFiles();
    } catch (e) { }

    $('#dropzoneForm' + Func.Checklist + ' div.dz-preview').remove();

    setTimeout(function () {
        unblockUI();
    }, 200);
}

function viewChecklist(custId, custPO, aiglPO, from, to, status, factoryId) {
    NewChecklist();
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
        loadQualityTable(result.POs);

        filterChecklist(Checklist.QAChecklist, result.QAChecklist);

        unblockUI();
    });
}

function checklistDetail(owner, custId, custPO, aiglPO, custName, factoryId) {
    var table = $(owner).parent();
    for (var i = 0; i < table[0].rows.length; i++) {
        $(table[0].rows[i]).removeClass('selected-row');
    }

    $(owner).addClass('selected-row');

    var from = $('.datepicker')[0].value;
    var to = $('.datepicker')[1].value;

    NewChecklist();
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
        filterChecklist(Checklist.QAChecklist, result.QAChecklist);

        unblockUI();
    });
}

function loadQualityTable(POs) {
    $('#main tbody tr').remove();
    if (POs.length == 0) {
        $('#main tbody').append('<tr><td colspan="9" class="c">There is no information</td></tr>');
    }
    else {
        for (var i = 0; i < POs.length; i++) {
            var po = POs[i];
            $('#main').append('<tr class="' + (i == 0 ? 'selected-row' : '') + '" onclick="checklistDetail(this, ' + po.CustId + ', \'' + po.CustPO + '\', \'' + po.AIGLPO + '\', \'' + po.Customer + '\', ' + po.FactoryId + ')"><td>' + po.AIGLPO + '</td><td>' + po.CustPO + '</td><td>' + po.Customer + '</td><td>' + po.Factory + '</td><td>' + po.Division + '</td><td>' + po.InlineDate + '</td><td>' + po.Status + '</td><td class="r">' + po.POQty + '</td><td class="r">' + po.Defect + '</td></tr>');
        }
    }
}