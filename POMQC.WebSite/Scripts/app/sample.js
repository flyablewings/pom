/// <reference path="../_references.js" />
function NewSample() {
    blockUI();

    $('#isUpdate' + Func.FitSample).val('0');
    $('#isUpdate' + Func.PPSample).val('0');
    $('#isUpdate' + Func.TopSample).val('0');

    $('#comment' + Func.FitSample).val('');
    $('#comment' + Func.PPSample).val('');
    $('#comment' + Func.TopSample).val('');

    $('input[name="img' + Func.FitSample + '"]').val('');
    $('input[name="img' + Func.PPSample + '"]').val('');
    $('input[name="img' + Func.TopSample + '"]').val('');

    try {
        var dropzone1 = Dropzone.forElement('#dropzoneForm' + Func.FitSample);
        var dropzone2 = Dropzone.forElement('#dropzoneForm' + Func.PPSample);
        var dropzone3 = Dropzone.forElement('#dropzoneForm' + Func.TopSample);

        dropzone1.removeAllFiles();
        dropzone2.removeAllFiles();
        dropzone3.removeAllFiles();
    } catch (e) { }

    $('#dropzoneForm' + Func.FitSample).empty();
    $('#dropzoneForm' + Func.PPSample).empty();
    $('#dropzoneForm' + Func.TopSample).empty();

    setTimeout(function () {
        unblockUI();
    }, 200);
}

function viewSample(custId, custPO, aiglPO, from, to, status, factoryId) {
    NewSample();
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
        loadSampleTable(result.POs);

        filterChecklist(Checklist.FitSample, result.FitSample);
        filterChecklist(Checklist.PPSample, result.PPSample);
        filterChecklist(Checklist.TopSample, result.TopSample);

        unblockUI();
    });
}

function sampleDetail(owner, custId, custPO, aiglPO, custName, factoryId) {
    var table = $(owner).parent();
    for (var i = 0; i < table[0].rows.length; i++) {
        $(table[0].rows[i]).removeClass('selected-row');
    }

    $(owner).addClass('selected-row');

    var from = $('.datepicker')[0].value;
    var to = $('.datepicker')[1].value;

    NewSample();
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
        filterChecklist(Checklist.FitSample, result.FitSample);
        filterChecklist(Checklist.PPSample, result.PPSample);
        filterChecklist(Checklist.TopSample, result.TopSample);

        unblockUI();
    });
}

function loadSampleTable(POs) {
    $('#main tbody tr').remove();
    if (POs.length == 0) {
        $('#main tbody').append('<tr><td colspan="9" class="c">There is no information</td></tr>');
    }
    else {
        for (var i = 0; i < POs.length; i++) {
            var po = POs[i];
            $('#main').append('<tr class="' + (i == 0 ? 'selected-row' : '') + '" onclick="sampleDetail(this, ' + po.CustId + ', \'' + po.CustPO + '\', \'' + po.AIGLPO + '\', \'' + po.Customer + '\', ' + po.FactoryId + ')"><td>' + po.AIGLPO + '</td><td>' + po.CustPO + '</td><td>' + po.Customer + '</td><td>' + po.Factory + '</td><td>' + po.Division + '</td><td>' + po.InlineDate + '</td><td>' + po.Status + '</td><td class="r">' + po.POQty + '</td><td class="r">' + po.Defect + '</td></tr>');
        }
    }
}