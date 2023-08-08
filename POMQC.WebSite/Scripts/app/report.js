/// <reference path="../_references.js" />
function viewReportByCustPO(custId, custPO, aiglPO, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getbycustpo'),
        type: 'post',
        data: {
            custId: custId,
            custPO: custPO,
            aiglPO: aiglPO,
            custName: $('#ddlCUST option:selected').text()
        }
    }).success(function (result) {
        bindReport(result.Items, fn, result.Title);
    });
}

function viewReportByFactory(factoryId, from, to, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getbyfactory'),
        type: 'post',
        data: {
            factoryId: factoryId,
            from: from,
            to: to,
            factoryName: $('#ddlFACTORY option:selected').text()
        }
    }).success(function (result) {
        bindReport(result.Items, fn, result.Title);
    });
}

function viewReportAllFactories(from, to, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getallfactories'),
        type: 'post',
        data: {
            from: from,
            to: to
        }
    }).success(function (result) {
        bindReportAllFactories(result.Factories, result.DHUs, fn, result.Title);
    });
}

function viewReportAllFinal(from, to, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getallfinal'),
        type: 'post',
        data: {
            from: from,
            to: to
        }
    }).success(function (result) {
        bindReportAllFinal(result.Items, result.Title, fn);
    });
}

function viewReportAllDetail(from, to, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getalldetail'),
        type: 'post',
        data: {
            from: from,
            to: to
        }
    }).success(function (result) {
        bindReportAllDetail(result.Items, result.Title, fn);
    });
}

function viewReportDhuWeekly(from, to, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getdhuweekly'),
        type: 'post',
        data: {
            from: from,
            to: to
        }
    }).success(function (result) {
        bindReportDhuWeekly(result.Items, result.Title, fn);
    });
}

function viewReportDhuMonthly(from, to, fn) {
    blockUI();
    $.ajax({
        url: resolveClientUrl('report/getdhumonthly'),
        type: 'post',
        data: {
            from: from,
            to: to
        }
    }).success(function (result) {
        bindReportDhuMonthly(result.Items, result.Title, fn);
    });
}

function bindReportDhuWeekly(items, title, fn) {
    var tbody = $('#report-dhu' + fn + ' tbody').empty();
    $('#report-title' + fn).html(title);

    if (items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            var row = 
                '<tr class="r">' +
                    '<td style="text-align:left">' + item.FactoryName + '</td>' +
                    '<td>' + item.Week1.InspectionQty + '</td>' +
                    '<td>' + item.Week1.ProductQty + '</td>' +
                    '<td>' + item.Week1.ProductPercent + '</td>' +
                    '<td>' + item.Week1.DefectQty + '</td>' +
                    '<td>' + item.Week1.DefectPercent + '</td>' +

                    '<td>' + item.Week2.InspectionQty + '</td>' +
                    '<td>' + item.Week2.ProductQty + '</td>' +
                    '<td>' + item.Week2.ProductPercent + '</td>' +
                    '<td>' + item.Week2.DefectQty + '</td>' +
                    '<td>' + item.Week2.DefectPercent + '</td>' +

                    '<td>' + item.Week3.InspectionQty + '</td>' +
                    '<td>' + item.Week3.ProductQty + '</td>' +
                    '<td>' + item.Week3.ProductPercent + '</td>' +
                    '<td>' + item.Week3.DefectQty + '</td>' +
                    '<td>' + item.Week3.DefectPercent + '</td>' +

                    '<td>' + item.Week4.InspectionQty + '</td>' +
                    '<td>' + item.Week4.ProductQty + '</td>' +
                    '<td>' + item.Week4.ProductPercent + '</td>' +
                    '<td>' + item.Week4.DefectQty + '</td>' +
                    '<td>' + item.Week4.DefectPercent + '</td>' +
                '</tr>';

            tbody.append(row);
        }
    }
    else {
        tbody.append('<tr><td colspan="21" class="c">There is no information</td></tr>');
    }

    var action = 'getdhuweeklyexcel?from=' + $('.datepicker5')[0].value + '&to=' + $('.datepicker5')[1].value;
    $('#report-dhu').prop('href', '/report/' + action);

    unblockUI();
}

function bindReportDhuMonthly(items, title, fn) {
    var tbody = $('#report-dhu' + fn + ' tbody').empty();
    $('#report-title' + fn).html(title);

    if (items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            var row =
                '<tr class="r">' +
                    '<td style="text-align:left">' + item.FactoryName + '</td>' +
                    '<td>' + item.Year + '</td>' +
                    '<td>' + item.Month + '</td>' +
                    '<td>' + item.OutputQty + '</td>' +
                    '<td>' + item.InspectionQty + '</td>' +
                    '<td>' + item.ProductQty + '</td>' +
                    '<td>' + item.ProductPercent + '</td>' +
                    '<td>' + item.DefectQty + '</td>' +
                    '<td>' + item.DefectPercent + '</td>' +
                '</tr>';

            tbody.append(row);
        }
    }
    else {
        tbody.append('<tr><td colspan="9" class="c">There is no information</td></tr>');
    }

    var action = 'getdhumonthlyexcel?from=' + $('.datepicker6')[0].value + '&to=' + $('.datepicker6')[1].value;
    $('#report-dhu-monthly').prop('href', '/report/' + action);

    unblockUI();
}

function bindReportAllDetail(items, title, fn) {
    var tbody = $('#report-detail' + fn + ' tbody').empty();
    $('#report-title' + fn).html(title);

    if (items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            var final = items[i];
            var type = getDHUType(final[0].DHUType);
            var styles = final.length;
            var index = 0, defect = 0, orderQty = 0, insQty = 0;
            var rowSpan = styles > 1 ? 'rowspan="' + styles + '"' : '';

            tbody.append('<tr><td colspan="12" class="b sup-header-row-bg">' + type + '</td></tr>');
            for (var j = 0; j < final.length; j++) {
                var item = final[j];
                defect += item.Total;
                orderQty += item.OrderQty;
                insQty += item.InspectedQty;
            }

            for (var j = 0; j < final.length; j++) {
                var item = final[j];
                var row = '';

                if (index == 0) {
                    row = '<tr>' +
                        '<td ' + rowSpan + '>' + item.FactoryName + '</td>' +
                        '<td ' + rowSpan + '>' + item.CustName + '</td>' +
                        '<td ' + rowSpan + '>' + item.CustPO + '</td>' +
                        '<td ' + rowSpan + '>' + item.AIGLPO + '</td>' +
                        '<td ' + rowSpan + '>' + orderQty + '</td>' +
                        '<td ' + rowSpan + '>' + insQty + '</td>' +
                        '<td>' + item.DefName + '</td>' +
                        '<td>' + item.LocName + '</td>' +
                        '<td class="c">' + getDefectType(item.Type) + '</td>' +
                        '<td class="r">' + item.Total + '</td>' +
                        '<td ' + rowSpan + '>' + getFormattedDate(new Date(item.CreatedDate)) + '</td>' +
                        '<td ' + rowSpan + '>' + item.CreatedUser + '</td>' +
                    '</tr>';
                    tbody.append(row);
                }
                else {
                    row = '<tr>' +
                        '<td>' + item.DefName + '</td>' +
                        '<td>' + item.LocName + '</td>' +
                        '<td class="c">' + getDefectType(item.Type) + '</td>' +
                        '<td class="r">' + item.Total + '</td>' +
                    '</tr>';
                    tbody.append(row);
                }

                ++index;
            }

            row = '<tr>' +
                    '<td colspan="9" class="b r">TOTAL</td>' +
                    '<td class="b r">' + defect + '</td>' +
                    '<td colspan="2"></td>' +
                '</tr>';
            tbody.append(row);
        }
    }
    else {
        tbody.append('<tr><td colspan="12" class="c">There is no information</td></tr>');
    }

    var action = 'getalldetailexcel?from=' + $('.datepicker4')[0].value + '&to=' + $('.datepicker4')[1].value;
    $('#report-all-detail').prop('href', '/report/' + action);

    unblockUI();
}

function bindReportAllFinal(items, title, fn) {
    var tbody = $('#report-final' + fn + ' tbody').empty();
    $('#report-title' + fn).html(title);

    if (items.length > 0) {
        for (var i = 0; i < items.length; i++) {
            var final = items[i];
            var type = getDHUType(final.Type);
            var styles = final.Styles;
            var index = 0, orderQty = 0, actQty = 0, insQty = 0;
            var rowSpan = styles.length > 1 ? 'rowspan="' + styles.length + '"' : '';
            
            tbody.append('<tr><td colspan="14" class="b sup-header-row-bg">' + type + '</td></tr>');
            for (var j = 0; j < styles.length; j++) {
                var item = styles[j];
                var row = '';

                orderQty += item.OrderQuantity;
                actQty += item.ActualProductQty;
                insQty += item.InspectedQty;

                if (index == 0) {
                    row = '<tr>' +
                        '<td ' + rowSpan + '>' + final.FactoryName + '</td>' +
                        '<td ' + rowSpan + '>' + final.CustName + '</td>' +
                        '<td ' + rowSpan + '>' + final.CustPO + '</td>' +
                        '<td ' + rowSpan + '>' + final.AIGLPO + '</td>' +
                        '<td>' + item.Style + '</td>' +
                        '<td>' + item.Color + '</td>' +
                        '<td class="r">' + item.OrderQuantity + '</td>' +
                        '<td class="r">' + item.ActualProductQty + '</td>' +
                        '<td class="r">' + item.InspectedQty + '</td>' +
                        '<td class="c" ' + rowSpan + '>' + getPassStatus(final.FinalStatus) + '</td>' +
                        '<td class="c" ' + rowSpan + '>' + getOnHoldStatus(final.FinalStatus) + '</td>' +
                        '<td class="c" ' + rowSpan + '>' + getRejectStatus(final.FinalStatus) + '</td>' +
                        '<td ' + rowSpan + '>' + final.CreateDate + '</td>' +
                        '<td ' + rowSpan + '>' + final.CreatedUser + '</td>' +
                    '</tr>';
                    tbody.append(row);
                }
                else {
                    row = '<tr>' +
                        '<td>' + item.Style + '</td>' +
                        '<td>' + item.Color + '</td>' +
                        '<td class="r">' + item.OrderQuantity + '</td>' +
                        '<td class="r">' + item.ActualProductQty + '</td>' +
                        '<td class="r">' + item.InspectedQty + '</td>' +
                    '</tr>';
                    tbody.append(row);
                }
                                
                ++index;
            }

            row = '<tr>' +
                    '<td colspan="6" class="b r">TOTAL</td>' +
                    '<td class="b r">' + orderQty + '</td>' +
                    '<td class="b r">' + actQty + '</td>' +
                    '<td class="b r">' + insQty + '</td>' +
                    '<td colspan="5"></td>' +
                '</tr>';
            tbody.append(row);
        }
    }
    else {
        tbody.append('<tr><td colspan="14" class="c">There is no information</td></tr>');
    }

    var action = 'getallfinalexcel?from=' + $('.datepicker3')[0].value + '&to=' + $('.datepicker3')[1].value;
    $('#report-final').prop('href', '/report/' + action);

    unblockUI();
}

function bindReport(data, fn, title) {
    var tbody = $('#report-po' + fn + ' tbody').empty();
    var grandTotal = 0;
    $('#report-title' + fn).html(title);
    if (data.length > 0) {
        tbody.append('<tr><td colspan="5">&nbsp;</td></tr>');
        for (var i = 0; i < data.length; i++) {
            var items = data[i];
            var index = 1;
            var total = 0;

            for (var k = 0; k < items.length; k++) {
                total += items[k].Total;
                grandTotal += items[k].Total;
            }

            tbody.append('<tr class="b sub-header-row-bg"><td></td><td colspan="3">' + getDHUType(items[0].DHUType) + '</td><td class="b c">' + total + '</td></tr>');

            for (var j = 0; j < items.length; j++) {
                var defect = items[j];
                tbody.append('<tr><td class="c">' + index + '</td><td>' + defect.DefName + '</td><td>' + defect.LocName + '</td><td>' + getDefectType(defect.Type) + '</td><td class="c">' + defect.Total + '</td></tr>');

                ++index;
            }

            tbody.append('<tr><td colspan="5">&nbsp;</td></tr>');
        }

        $('#report-grand-total' + fn).html(grandTotal);
    }
    else {
        tbody.append('<tr><td colspan="5" class="c">There is no information</td></tr>');
        $('#report-grand-total' + fn).html('0');
    }

    var custId = '0';
    var custPO = '0';
    var aiglPO = '0';
    var custName = '';
    var tr = $('#main .selected-row');

    if (tr && tr[0] && tr[0].cells) {
        custName = tr[0].cells[2].innerText;
        var options = model.Customers;
        for (var i = 0; i < options.length; i++) {
            if (options[i].CustName == custName) {
                custId = options[i].CustId;
                break;
            }
        }

        custPO = tr[0].cells[1].innerText;
        aiglPO = tr[0].cells[0].innerText;
    }

    custId = custId != '0' ? custId : $("#ddlCUST").val();
    custPO = custPO != '0' ? custPO : $("#ddlCUSTPO").val();
    aiglPO = aiglPO != '0' ? aiglPO : $("#ddlAIGLPO").val();
    custName = custName != '' && custName != 'All' ? custName : $("#ddlCUST option:selected").text();

    var action = fn == Func.ReportByPO ?
            'getbycustpoexcel?custid=' + custId + '&custpo=' + custPO + '&aiglpo=' + aiglPO + '&custname=' + custName :
            'getbyfactoryexcel?factoryid=' + parseInt($('#ddlFACTORY').val()) + '&from=' + $('.datepicker')[0].value + '&to=' + $('.datepicker')[1].value + '&factoryname=' + $('#ddlFACTORY option:selected').text();
    $('#report-cust-fact-' + fn).prop('href', '/report/' + action);

    unblockUI();
}

function bindReportAllFactories(factories, dhus, fn, title) {
    var tbody = $('#report-po' + fn + ' tbody').empty();
    var grandTotal = 0;
    $('#report-title' + fn).html(title);
    if (factories.length > 0) {
        tbody.append('<tr><td colspan="5">&nbsp;</td></tr>');
        for (var i = 0; i < factories.length; i++) {
            var factory = factories[i][0];
            var totalByFactory = 0;
            var totalByDHU = 0;

            for (var m = 0; m < dhus.length; m++) {
                var defects = dhus[m];

                for (var n = 0; n < defects.length; n++) {
                    if (defects[n].FactoryName === factory.FactoryName) {
                        var defect = defects[n];
                        totalByFactory += defect.Total;
                        grandTotal += defect.Total;
                    }
                }
            }

            tbody.append('<tr class="b sup-header-row-bg"><td colspan="4">' + factory.FactoryName + '</td><td class="b c">' + totalByFactory + '</td></tr>');

            for (var m = 0; m < dhus.length; m++) {
                var defects = dhus[m];
                totalByDHU = 0;

                for (var n = 0; n < defects.length; n++) {
                    if (defects[n].FactoryName === factory.FactoryName) {
                        var defect = defects[n];
                        totalByDHU += defect.Total;
                    }
                }

                if (totalByDHU > 0) {
                    tbody.append('<tr class="b sub-header-row-bg"><td></td><td colspan="3">' + getDHUType(defects[0].DHUType) + '</td><td class="b c">' + totalByDHU + '</td></tr>');
                    var index = 1;

                    for (var n = 0; n < defects.length; n++) {
                        if (defects[n].FactoryName === factory.FactoryName) {
                            var defect = defects[n];
                            tbody.append('<tr><td class="c">' + index + '</td><td>' + defect.DefName + '</td><td>' + defect.LocName + '</td><td>' + getDefectType(defect.Type) + '</td><td class="c">' + defect.Total + '</td></tr>');
                            ++index;
                        }
                    }
                }
            }

            tbody.append('<tr><td colspan="5">&nbsp;</td></tr>');
        }

        $('#report-grand-total' + fn).html(grandTotal);
    }
    else {
        tbody.append('<tr><td colspan="5" class="c">There is no information</td></tr>');
        $('#report-grand-total' + fn).html('0');
    }

    var action = 'getallfactoriesexcel?from=' + $('.datepicker2')[0].value + '&to=' + $('.datepicker2')[1].value;
    $('#report-all-fact').prop('href', '/report/' + action);

    unblockUI();
}

function getDHUType(type) {
    return type == 1 ? 'CUTTING' :
        type == 2 ? 'INLINE' :
        type == 3 ? 'ENDLINE' :
        type == 4 ? 'FINISHING' :
        type == 5 ? 'PACKING' :
        type == 6 ? 'PREFINAL' : 'FINAL';
}

function getDefectType(type) {
    return type == 1 ? 'Major' : 'Minor';
}

function getPassStatus(type) {
    return type == 1 ? 'Yes' : '';
}

function getOnHoldStatus(type) {
    return type == 3 ? 'Yes' : '';
}

function getRejectStatus(type) {
    return type == 2 ? 'Yes' : '';
}