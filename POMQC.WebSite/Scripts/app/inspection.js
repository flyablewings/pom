/// <reference path="../_references.js" />
function NewInspection() {
    blockUI();
    try {
        clearFabric(Func.Fabric);
        clearSelectedDHU(DHU.Cutting);
        clearSelectedDHU(DHU.Inline);
        clearSelectedDHU(DHU.Endline);
        clearSelectedDHU(DHU.Finishing);
        clearSelectedDHU(DHU.Packing);
        clearSelectedFinal(DHU.Prefinal);
        clearSelectedFinal(DHU.Final);

        setTimeout(function () {
            unblockUI();
        }, 200);
    } catch (e) { unblockUI(); }
}

function clearFabric(fn) {
    try {
        $('#errmsg' + fn).hide();
        $('#comment' + fn).val('');
        $('input[name="img' + fn + '"]').val('');

        try {
            var dropzone1 = Dropzone.forElement('#dropzoneForm' + fn);
            dropzone1.removeAllFiles();
        } catch (e) { }
        $('#dropzoneForm' + fn).empty();
    } catch (e) { unblockUI(); }
}

function getDHU(custId, custPO, aiglPO, from, to, status, factoryId) {
    blockUI();
    try {
        $.ajax({
            url: resolveClientUrl('dhu/view'),
            type: 'post',
            data: {
                factoryId: factoryId,
                custId: custId,
                custPO: custPO == '0' ? '' : custPO,
                aiglPO: aiglPO == '0' ? '' : aiglPO,
                from: from,
                to: to,
                status: status
            }
        }).success(function (result) {
            styles = result.Styles;
            var fabric = result.Fabric;
            var cutting = result.Cutting;
            var inline = result.Inline;
            var endline = result.Endline;
            var finishing = result.Finishing;
            var packing = result.Packing;
            var prefinal = result.Prefinal;
            var final = result.Final;

            loadInspectionTable(result.POs);
            filterChecklist(fabric.Function, fabric);

            filterDHU(cutting.Function, cutting.Item, cutting.Items, cutting.Item.CreateDate, cutting.Defects, cutting.Item.Images);
            filterDHU(inline.Function, inline.Item, inline.Items, inline.Item.CreateDate, inline.Defects, inline.Item.Images);
            filterDHU(endline.Function, endline.Item, endline.Items, endline.Item.CreateDate, endline.Defects, endline.Item.Images);
            filterDHU(finishing.Function, finishing.Item, finishing.Items, finishing.Item.CreateDate, finishing.Defects, finishing.Item.Images);
            filterDHU(packing.Function, packing.Item, packing.Items, packing.Item.CreateDate, packing.Defects, packing.Item.Images);

            filterFinal(prefinal.Function, prefinal.Item, prefinal.Items, prefinal.Item.CreateDate, prefinal.Defects, prefinal.Item.Images, prefinal.POs);
            filterFinal(final.Function, final.Item, final.Items, final.Item.CreateDate, final.Defects, final.Item.Images, final.POs);

            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function viewTableDHU(func, DHUs) {
    try {
        var table = $('#dhu' + func + ' tbody').empty();
        if (DHUs.length == 0) {
            table.append('<tr><td colspan="5" class="c">There is no information</td></tr>');
        }
        else {
            for (var i = 0; i < DHUs.length; i++) {
                table.append('<tr onclick="viewDHU(this, ' + DHUs[i].CustId + ', \'' + DHUs[i].CustPO + '\', \'' + DHUs[i].AIGLPO + '\', \'' + DHUs[i].CreateDate + '\', ' + func + ', ' + func + ')" class="' + (i == 0 ? 'selected-row' : '') + '"><td>' + (i + 1) + '</td><td>' + DHUs[i].CreateDate + '</td><td>' + DHUs[i].CreatedUser + '</td><td>' + formatInfo(DHUs[i].UpdateDate) + '</td><td>' + formatInfo(DHUs[i].UpdatedUser) + '</td></tr>');
            }
        }
    } catch (e) { unblockUI(); }
}

function viewTableFinal(func, DHUs) {
    try {
        var table = $('#final' + func + ' tbody').empty();
        if (DHUs.length == 0) {
            table.append('<tr><td colspan="5" class="c">There is no information</td></tr>');
        }
        else {
            for (var i = 0; i < DHUs.length; i++) {
                table.append('<tr onclick="viewFinal(this, ' + DHUs[i].CustId + ', \'' + DHUs[i].CustPO + '\', \'' + DHUs[i].AIGLPO + '\', \'' + DHUs[i].CreateDate + '\', ' + func + ', ' + func + ')" class="' + (i == 0 ? 'selected-row' : '') + '"><td>' + (i + 1) + '</td><td>' + DHUs[i].CreateDate + '</td><td>' + DHUs[i].CreatedUser + '</td><td>' + formatInfo(DHUs[i].UpdateDate) + '</td><td>' + formatInfo(DHUs[i].UpdatedUser) + '</td></tr>');
            }
        }
    } catch (e) { unblockUI(); }
}

function filterChecklist(fn, fabric) {
    try {
        clearFabric(fn);

        loadChecklistTable(fabric, fn);

        $('#isUpdate' + fn).val(fabric.CustId > 0 ? 1 : 0);
        $('#comment' + fn).val(fabric.Comment);

        var mockFiles = fabric.DroppedFiles;
        $(document).ready(function () {
            setTimeout(function () {
                var dropzone = Dropzone.forElement('#dropzoneForm' + fn);
                for (var i = 0; i < mockFiles.length; i++) {
                    if (mockFiles[i].hasImage) {
                        // Create the mock file:
                        var mockFile = mockFiles[i];

                        // Call the default addedfile event handler
                        dropzone.emit("addedfile", mockFile);

                        // And optionally show the thumbnail of the file:
                        dropzone.emit("thumbnail", mockFile, mockFile.url);

                        dropzone.files.push(mockFile);
                    }
                }
            }, 200);
        });
    } catch (e) { unblockUI(); }
}

function loadChecklistTable(checklist, fn) {
    try {
        var tbody = $('#checklist' + fn + ' tbody');
        tbody.empty();

        if (checklist.Doc != null) {
            tbody.append('<tr><td>' + checklist.CreateDate + '</td><td>' + checklist.CreatedUser + '</td><td>' + formatInfo(checklist.UpdateDate) + '</td><td>' + formatInfo(checklist.UpdatedUser) + '</td></tr>');
        }
        else {
            tbody.append('<tr><td colspan="4" class="c">There is no information</td></tr>');
        }
    } catch (e) { unblockUI(); }
}

function filterDHU(fn, dhu, items, date, defects, images) {
    try {
        viewTableDHU(fn, items);
        loadDHUGeneralInfo(dhu, date, fn);
        loadDHUDefect(defects, fn);

        var mockFiles = images;
        setTimeout(function () {

            try {
                var dropzone = Dropzone.forElement('#dropzoneForm2' + fn);
                dropzone.removeAllFiles();
            } catch (e) { }
            $('#dropzoneForm2' + fn).empty();

            for (var i = 0; i < mockFiles.length; i++) {
                if (mockFiles[i].hasImage) {
                    // Create the mock file:
                    var mockFile = mockFiles[i];

                    // Call the default addedfile event handler
                    dropzone.emit("addedfile", mockFile);

                    // And optionally show the thumbnail of the file:
                    dropzone.emit("thumbnail", mockFile, mockFile.url);

                    dropzone.files.push(mockFile);
                }
            }
        }, 1000);
    } catch (e) { unblockUI(); }
}

function filterFinal(fn, dhu, items, date, defects, images, POs) {
    try {
        clearSelectedFinal(fn);
        viewTableFinal(fn, items);
        loadFinalGeneralInfo(dhu, date, fn, POs);
        loadFinalDefect(defects, fn, POs);

        var mockFiles = images;
        setTimeout(function () {

            try {
                var dropzone = Dropzone.forElement('#dropzoneForm2' + fn);
                dropzone.removeAllFiles();
            } catch (e) { }
            $('#dropzoneForm2' + fn).empty();

            for (var i = 0; i < mockFiles.length; i++) {
                if (mockFiles[i].hasImage) {
                    // Create the mock file:
                    var mockFile = mockFiles[i];

                    // Call the default addedfile event handler
                    dropzone.emit("addedfile", mockFile);

                    // And optionally show the thumbnail of the file:
                    dropzone.emit("thumbnail", mockFile, mockFile.url);

                    dropzone.files.push(mockFile);
                }
            }
        }, 1000);
    } catch (e) { unblockUI(); }
}

function viewDHU(owner, custId, custPO, aiglPO, date, type, func) {
    try {
        var defectRows = $('#dhu' + func + ' tbody')[0].rows;
        for (var i = 0; i < defectRows.length; i++) {
            $(defectRows[i]).removeClass('selected-row');
        }

        $(owner).addClass('selected-row');
        blockUI();

        $.ajax({
            url: resolveClientUrl('dhu/select'),
            type: 'post',
            data: {
                custId: custId,
                custPO: custPO,
                aiglPO: aiglPO,
                date: date,
                type: type
            }
        }).success(function (result) {
            var dhu = result.Item;
            loadDHUGeneralInfo(dhu, date, func);
            loadDHUDefect(result.Defects, func);

            var mockFiles = result.Item.Images;
            setTimeout(function () {

                try {
                    var dropzone = Dropzone.forElement('#dropzoneForm2' + func);
                    dropzone.removeAllFiles();
                } catch (e) { }
                $('#dropzoneForm2' + func).empty();

                for (var i = 0; i < mockFiles.length; i++) {
                    if (mockFiles[i].hasImage) {
                        // Create the mock file:
                        var mockFile = mockFiles[i];

                        // Call the default addedfile event handler
                        dropzone.emit("addedfile", mockFile);

                        // And optionally show the thumbnail of the file:
                        dropzone.emit("thumbnail", mockFile, mockFile.url);

                        dropzone.files.push(mockFile);
                    }
                }
            }, 1000);

            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function viewFinal(owner, custId, custPO, aiglPO, date, type, func) {
    try {
        var defectRows = $('#final' + func + ' tbody')[0].rows;
        for (var i = 0; i < defectRows.length; i++) {
            $(defectRows[i]).removeClass('selected-row');
        }

        $(owner).addClass('selected-row');
        blockUI();

        $.ajax({
            url: resolveClientUrl('final/select'),
            type: 'post',
            data: {
                custId: custId,
                custPO: custPO,
                aiglPO: aiglPO,
                date: date,
                type: type
            }
        }).success(function (result) {
            var final = result.Item;
            loadFinalGeneralInfo(final, date, func, result.POs);
            loadFinalDefect(result.Defects, func, result.POs);

            var mockFiles = result.Item.Images;
            setTimeout(function () {

                try {
                    var dropzone = Dropzone.forElement('#dropzoneForm2' + func);
                    dropzone.removeAllFiles();
                } catch (e) { }
                $('#dropzoneForm2' + func).empty();

                for (var i = 0; i < mockFiles.length; i++) {
                    if (mockFiles[i].hasImage) {
                        // Create the mock file:
                        var mockFile = mockFiles[i];

                        // Call the default addedfile event handler
                        dropzone.emit("addedfile", mockFile);

                        // And optionally show the thumbnail of the file:
                        dropzone.emit("thumbnail", mockFile, mockFile.url);

                        dropzone.files.push(mockFile);
                    }
                }
            }, 1000);

            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function loadDHUGeneralInfo(dhu, date, func) {
    try {
        $('#date' + func).html(date);
        $('#type' + func).val(dhu.Type);
        $('#type2' + func).val(dhu.Type2);
        $('#inspector' + func).val(dhu.Inspector);
        $('#po-qty' + func).val(dhu.POQty);
        $('#output-qty' + func).val(dhu.OutputQty);
        $('#audit-size' + func).val(dhu.AuditSampleSize);
        $('#country' + func).val(dhu.Country);
        $('#line-no' + func).val(dhu.LineNumber);
        $('#comment2' + func).val(dhu.Comment);
        $('#factoryName' + func).html(dhu.FactoryName);
        $('#factoryId' + func).html(dhu.FactoryId);
        $('#custName' + func).html(dhu.CustName);
        loadStyles(dhu.Style, func);
        $('#brand' + func).html(dhu.Brand);
        $('#agent' + func).html(dhu.AgentName);
        $('#agentId' + func).val(dhu.AgentId);
        $('#dhuid' + func).val(dhu.DHUId);
        $('#aiglPO' + func).html(dhu.AIGLPO);
        $('#custPO' + func).html(dhu.CustPO);
        $('#export-dhu' + func).prop("href", "/dhu/export?custid=" + dhu.CustId + "&custpo=" + dhu.CustPO + "&aiglpo=" + dhu.AIGLPO + "&date=" + dhu.CreateDate + "&type=" + func);
        $('#export-dhu2' + func).prop("href", "/dhu/exportpdf?custid=" + dhu.CustId + "&custpo=" + dhu.CustPO + "&aiglpo=" + dhu.AIGLPO + "&date=" + dhu.CreateDate + "&type=" + func);
        $('#export-dhu3' + func).prop("href", "javascript:sendMail(true, " + dhu.CustId + ", '" + dhu.CustPO + "', '" + dhu.AIGLPO + "', '" + dhu.CreateDate + "', " + func + ");");
    } catch (e) { unblockUI(); }
}

function loadStyles(value, func) {
    $('#style' + func).empty();

    for (var i = 0; i < styles.length; i++) {
        var selected = styles[i] == value ? 'selected' : '';
        $('#style' + func).append('<option ' + selected + '>' + styles[i] + '</option>');
    }
}

function loadFinalGeneralInfo(final, date, func, pos) {
    try {
        var inspectedQty = 0, actualProductQty = 0;
        for (var i = 0; i < pos.length; i++) {
            inspectedQty += pos[i].InspectedQty;
            actualProductQty += pos[i].ActualProductQty;
        }
        $('#dropzoneForm2' + func).empty();
        $('#date' + func).html(date);
        $('#audited-by' + func).val(final.AuditedBy);
        $('#custName2' + func).html(final.CustName);
        $('#factName2' + func).html(final.FactoryName);
        $('#factory-manager' + func).val(final.FactoryManager);
        $('#main-label' + func).val(final.MainLabel);
        $('#qty-inspected' + func).html(inspectedQty);
        $('#carton-comment' + func).val(final.CartonComment);
        $('#sum-comment' + func).val(final.WorkmanshipComment);
        $('#sum-comment2' + func).val(final.MeasurementComment);
        $('#sum-comment3' + func).val(final.PackingComment);
        $('#final-comment' + func).val(final.Comment);
        $('#qa-comment' + func).val(final.QAComment);
        $('#factory-caption' + func).val(final.FactoryCaption);
        $('#qa-auditor' + func).val(final.QAAuditor);
        $('#factory-rep' + func).val(final.FactoryRep);
        $('#qa-manager' + func).val(final.QAManager);
        $('#finalid' + func).val(final.FinalId);

        var idx = func == DHU.Prefinal ? 0 : 1;
        $('.datepicker2')[idx].value = final.ActualInspectionDate == null ? getFormattedDate(new Date()) : final.ActualInspectionDate;
        $('#act-product-qty' + func).html(actualProductQty);

        var aql = $('input[name="aql' + func + '"]');
        var aql2 = $('input[name="aql2' + func + '"]');
        var finalStatus = $('input[name="final' + func + '"]');
        var finalComment = $('input[name="comment' + func + '"]');
        var wmsStatus = $('input[name="sum-inspect' + func + '"]');
        var msmStatus = $('input[name="sum-inspect2' + func + '"]');
        var ppiStatus = $('input[name="sum-inspect3' + func + '"]');

        if (final.WorkmanshipAQL == 1) {
            $(aql[0]).prop('checked', true);
            changeAQL(aql[0], 1, 2, 3, func);
        }
        else if (final.WorkmanshipAQL == 2) {
            $(aql[1]).prop('checked', true);
            changeAQL(aql[1], 2, 1, 3, func);
        }
        else if (final.WorkmanshipAQL == 3) {
            $(aql[2]).prop('checked', true);
            changeAQL(aql[2], 3, 2, 1, func);
        }

        if (final.MeasurementAQL == 4) {
            $(aql2[1]).prop('checked', true);
            changeAQL2(aql2[1], 1, 2, 3, func);
        }
        else if (final.MeasurementAQL == 5) {
            $(aql2[2]).prop('checked', true);
            changeAQL2(aql2[2], 2, 1, 3, func);
        }
        else if (final.MeasurementAQL == 6) {
            $(aql2[0]).prop('checked', true);
            changeAQL2(aql2[0], 3, 2, 1, func);
        }

        if (final.FinalStatus == 1) {
            $(finalStatus[0]).prop('checked', true);
            $(finalComment[0]).removeAttr('disabled').val(final.FinalComment);
            $(finalComment[1]).attr('disabled', 'disabled').val('');
            $(finalComment[2]).attr('disabled', 'disabled').val('');
        }
        else if (final.FinalStatus == 2) {
            $(finalStatus[1]).prop('checked', true);
            $(finalComment[1]).removeAttr('disabled').val(final.FinalComment);
            $(finalComment[0]).attr('disabled', 'disabled').val('');
            $(finalComment[2]).attr('disabled', 'disabled').val('');
        }
        else if (final.FinalStatus == 3) {
            $(finalStatus[2]).prop('checked', true);
            $(finalComment[2]).removeAttr('disabled').val(final.FinalComment);
            $(finalComment[1]).attr('disabled', 'disabled').val('');
            $(finalComment[0]).attr('disabled', 'disabled').val('');
        }

        if (final.WorkmanshipStatus == 1) {
            $(wmsStatus[0]).prop('checked', true);
        }
        else if (final.WorkmanshipStatus == 2) {
            $(wmsStatus[1]).prop('checked', true);
        }

        if (final.MeasurementStatus == 1) {
            $(msmStatus[0]).prop('checked', true);
        }
        else if (final.MeasurementStatus == 2) {
            $(msmStatus[1]).prop('checked', true);
        }

        if (final.PackingStatus == 1) {
            $(ppiStatus[0]).prop('checked', true);
        }
        else if (final.PackingStatus == 2) {
            $(ppiStatus[1]).prop('checked', true);
        }

        $('#export-final' + func).prop("href", "/final/export?custid=" + final.CustId + "&custpo=" + final.CustPO + "&aiglpo=" + final.AIGLPO + "&date=" + final.CreateDate + "&type=" + func);
        $('#export-final2' + func).prop("href", "/final/exportpdf?custid=" + final.CustId + "&custpo=" + final.CustPO + "&aiglpo=" + final.AIGLPO + "&date=" + final.CreateDate + "&type=" + func);
        $('#export-final3' + func).prop("href", "javascript:sendMail(false, " + final.CustId + ", '" + final.CustPO + "', '" + final.AIGLPO + "', '" + final.CreateDate + "', " + func + ");");
    } catch (e) { unblockUI(); }
}

function loadDHUDefect(defects, func) {
    try {
        $('#dropzoneForm2' + func).empty();

        var totalDefects = 0;
        var pcs = 0;
        var counting = 0;

        var tbody = $('#tbl-dhu' + func + ' tbody')[0];
        for (var i = tbody.rows.length - 1; i > 0; i--) {
            $(tbody.rows[i]).remove();
        }

        if (defects.length == 0) {
            loadDefectValues(func, 0, 0, 10, 1, 0, 0);           
        }
        else {
            for (var i = 0; i < defects.length; i++) {
                loadDefectValues(func, defects[i].DefId, defects[i].LocId, defects[i].PCSQty, defects[i].Total, defects[i].DdefId, i);
                totalDefects += defects[i].Total;
                pcs += defects[i].PCSQty;
                counting += defects[i].Total;
            }

            loadDefectValues(func, 0, 0, 10, 1, 0, defects.length);
        }

        $('#grand-1' + func).html(totalDefects);
        $('#grand-2' + func).html(pcs);
        $('#grand-3' + func).html((counting * 100 / parseFloat($('#audit-size' + func).val())).toFixed(2) + '%');
        $('#grand-4' + func).html((pcs / parseFloat($('#audit-size' + func).val()) * 100).toFixed(2) + '%');
    } catch (e) { unblockUI(); }
}

function loadFinalDefect(defects, func, POs) {
    try {
        $('#dropzoneForm2' + func).empty();

        var totalDefectMajor = 0;
        var totalDefectMinor = 0;

        try {
            var tbody = $('#tbl-final' + func + ' tbody')[0];
            for (var i = tbody.rows.length - 1; i >= 0; i--) {
                $(tbody.rows[i]).remove();
            }
        } catch (e) { }

        if (defects.length == 0) {
            loadFinalDefectValues(func, 0, 0, 0, 0, 0);
        }
        else {
            for (var i = 0; i < defects.length; i++) {
                loadFinalDefectValues(func, defects[i].DefId, defects[i].LocId, defects[i].Type, defects[i].DdefId, i, defects[i].Total);
                if (defects[i].Type == 1) {
                    totalDefectMajor += defects[i].Total;
                }
                else {
                    totalDefectMinor += defects[i].Total;
                }
            }

            loadFinalDefectValues(func, 0, 0, 0, 0, defects.length);
        }

        $('#total-defect' + func).html(totalDefectMajor);
        $('#total-defect2' + func).html(totalDefectMinor);

        var table = $('#po-list' + func + ' tbody').empty();
        if (POs.length == 0) {
            table.append('<tr><td colspan="7" class="c">There is no information</td></tr>');
            $('#sum-po-list' + func).html('0');
            $('#sum-ins-list' + func).html('0');
            $('#sum-act-list' + func).html('0');
        }
        else {
            var qty = 0, qtyInspected = 0, qtyActualProduct = 0, fdid = 0;
            for (var i = 0; i < POs.length; i++) {
                fdid = POs[i].FinalDetailId;
                qty += POs[i].OrderQuantity;
                qtyInspected += POs[i].InspectedQty;
                qtyActualProduct += POs[i].ActualProductQty;

                table.append('<tr><td>' + POs[i].AIGLPO + '<input type="hidden" id="fdid-' + func + '-' + i + '" value="' + fdid + '" /></td><td>' + POs[i].CustPO + '</td><td id="style-' + func + '-' + i + '">' + POs[i].Style + '</td><td id="color-' + func + '-' + i + '">' + POs[i].Color + '</td><td class="r" id="qty-' + func + '-' + i + '">' + POs[i].OrderQuantity + '</td><td><input style="width: 100%" class="r" type="text" id="ins-' + func + '-' + i + '" value="' + POs[i].InspectedQty + '" /></td><td><input style="width: 100%" class="r" type="text" id="act-' + func + '-' + i + '" value="' + POs[i].ActualProductQty + '" /></td></tr>');
            }
            $('#sum-po-list' + func).html(qty);
            $('#sum-ins-list' + func).html(qtyInspected);
            $('#sum-act-list' + func).html(qtyActualProduct);
        }
    } catch (e) { unblockUI(); }
}

function clearSelectedDHU(func) {
    try {
        var custId = '0';
        var custPO = '0';
        var aiglPO = '0';
        var custName = '';
        var tr = $('.selected-row');

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
        custName = custName != '' ? custName : $("#ddlCUST option:selected").text();

        $('#date' + func).html(getFormattedDate(new Date()));
        $('#type' + func).val('0');
        $('#inspector' + func).val('');
        $('#po-qty' + func).val('100');
        $('#output-qty' + func).val('100');
        $('#audit-size' + func).val('10');
        $('#country' + func).val('Vietnam');
        $('#line-no' + func).val('');
        $('#aiglPO' + func).html(aiglPO);
        $('#custPO' + func).html(custPO);
        $('#custName' + func).html(custName);
        $('#factoryName' + func).html('');
        $('#factoryId' + func).html('');
        $('#agent' + func).html('');
        $('#brand' + func).html('');

        try {
            var dropzone = Dropzone.forElement('#dropzoneForm2' + func);
            dropzone.removeAllFiles();
        } catch (e) {
        }
        $('#dropzoneForm2' + func).empty();
        $('#comment2' + func).val('');

        var defectRows = $('#tbl-dhu' + func + ' tbody')[0].rows;
        if (defectRows.length > 2) {
            for (var i = defectRows.length - 1; i >= 2; i--) {
                $(defectRows[i]).remove();
            }
        }

        $($('select[name="codes' + func + '"]')[0]).val('0');
        $($('select[name="locations' + func + '"]')[0]).val('0');
        $($('input[name="pcs' + func + '"]')[0]).val('10');
        $($('input[name="defect' + func + '"]')[0]).val('1');
        $('#total' + func + '-0').html('1');
        $('#grand-1' + func).html('1');
        $('#grand-2' + func).html('10');
        $('#grand-3' + func).html('10%');
        $('#grand-4' + func).html('100%');

        $('#dhuid' + func).val('0');
        var table = $('#dhu' + func)[0];
        for (var i = 0; i < table.rows.length; i++) {
            $(table.rows[i]).removeClass('selected-row');
        }
    } catch (e) { unblockUI(); }
}

function clearSelectedFinal(func) {
    try {
        $('#date' + func).html(getFormattedDate(new Date()));
        $('#finalid' + func).val('0');
        var table = $('#final' + func)[0];
        for (var i = 0; i < table.rows.length; i++) {
            $(table.rows[i]).removeClass('selected-row');
        }

        var defectRows = $('#tbl-final' + func + ' tbody')[0].rows;
        if (defectRows.length > 1) {
            for (var i = defectRows.length - 1; i >= 1; i--) {
                $(defectRows[i]).remove();
            }
        }

        $($('select[name="codes' + func + '"]')[0]).val('0');
        $($('select[name="locations' + func + '"]')[0]).val('0');
        $('#def-00' + func).html('');
        $('#def-10' + func).html('');
        $('#total-defect' + func).html('0');
        $('#total-defect2' + func).html('0');

        try {
            var dropzone1 = Dropzone.forElement('#dropzoneForm2' + func);
            dropzone1.removeAllFiles();
        } catch (e) { }
        $('#dropzoneForm2' + func).empty();

        $('#audited-by' + func).val('');
        $('#custName2' + func).html('');
        $('#factName2' + func).html('');
        $('#factory-manager' + func).val('');
        $('#main-label' + func).val('');
        $('#qty-inspected' + func).html('0');
        $('#act-product-qty' + func).html('0');
        $('#carton-comment' + func).val('');
        $('#sum-comment' + func).val('');
        $('#sum-comment2' + func).val('');
        $('#sum-comment3' + func).val('');
        $('#final-comment' + func).val('');
        $('#qa-comment' + func).val('');
        $('#factory-caption' + func).val('');
        $('#qa-auditor' + func).val('');
        $('#factory-rep' + func).val('');
        $('#qa-manager' + func).val('');
        $('#pass-comment' + func).val('');
        $('#reject-comment' + func).val('');
        $('#on-hold-comment' + func).val('');

        var pos = $('#po-list' + func + ' tbody tr');
        for (var i = 0; i < pos.length; i++) {
            $('#fdid-' + func + '-' + i).val('0');
            $('#ins-' + func + '-' + i).val('0');
            $('#act-' + func + '-' + i).val('0');
        }

        $('#sum-ins-list' + func).html('0');
        $('#sum-act-list' + func).html('0');
    } catch (e) { unblockUI(); }
}

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return month + '/' + day + '/' + year;
}

function loadDefectValues(func, defId, locId, pcs, total, ddefid, rowIndex) {
    var tbody = $('#tbl-dhu' + func + ' tbody')[0];
    var row =
        '<tr><td><input type="hidden" name="ddefid' + func + '" value="' + (ddefid || 0) + '"/>' + initDefectCode(func, defId, rowIndex) + '</td><td>' + initDefectLocation(func, locId, rowIndex) + '</td><td>' + initPCSQty(func, pcs) + '</td><td>' + initDefectQty(func, total) + '</td><td class="r">' + total + '</td></tr>';
    $(tbody).append(row);

    $('#def_' + (func + '_' + (rowIndex))).select2();
    $('#def_' + (func + '_' + (rowIndex + 1))).select2();

    $('#loc_' + (func + '_' + (rowIndex))).select2();
    $('#loc_' + (func + '_' + (rowIndex + 1))).select2();
}

function loadFinalDefectValues(func, defId, locId, type, ddefid, rowIndex, defects) {
    var tbody = $('#tbl-final' + func + ' tbody')[0];
    var row =
        '<tr><td><input type="hidden" name="ddefid' + func + '" value="' + (ddefid || 0) + '"/>' + initDefectFinal(func, rowIndex, defId) + '</td><td>' + initDefectLocation(func, locId, rowIndex) + '</td>' + initDefectMajorMinor(func, type, rowIndex, defects) + '</tr>';
    $(tbody).append(row);

    $('#def_' + (func + '_' + (rowIndex))).select2();
    $('#def_' + (func + '_' + (rowIndex + 1))).select2();

    $('#loc_' + (func + '_' + (rowIndex))).select2();
    $('#loc_' + (func + '_' + (rowIndex + 1))).select2();
}

function changeDefectCode(select, func, rowIndex) {
    var value = $(select).val();
    var selects = $('select[name="codes' + func + '"]');
    var availableSelect = false;
    for (var i = 0; i < selects.length; i++) {
        if ($(selects[i]).val() == '0') {
            availableSelect = true;
            break;
        }
    }

    if (value != '0' && !availableSelect) {
        var tbody = $('#tbl-dhu' + func + ' tbody')[0];
        var row =
        '<tr><td>' + initDefectCode(func, null, rowIndex + 1) + '</td><td>' + initDefectLocation(func, null, rowIndex + 1) + '</td><td>' + initPCSQty(func) + '</td><td>' + initDefectQty(func) + '</td><td class="r">1</td></tr>';
        $(tbody).append(row);

        $('#def_' + (func + '_' + (rowIndex + 1))).select2();
        $('#loc_' + (func + '_' + (rowIndex + 1))).select2();
    }
}



function changeDefectFinal(select, func, rowIndex) {
    var value = $(select).val();
    var selects = $('select[name="codes' + func + '"]');
    var availableSelect = false;
    var type = $(select).find('option:selected').attr('major');

    for (var i = 0; i < selects.length; i++) {
        if ($(selects[i]).val() == '0') {
            availableSelect = true;
            break;
        }
    }

    if (type == '1') {
        $("#def-0" + rowIndex + "" + func).html('<input type="text" name="defect' + func + '" value="1" style="width: 95%" class="r" />');
        $("#def-1" + rowIndex + '' + func).html('');
    }
    else {
        $("#def-1" + rowIndex + '' + func).html('<input type="text" name="defect' + func + '" value="1" style="width: 95%" class="r" />');
        $("#def-0" + rowIndex + "" + func).html('');
    }

    if (value != '0' && !availableSelect) {
        var tbody = $('#tbl-final' + func + ' tbody')[0];
        var row =
        '<tr><td>' + initDefectFinal(func, (rowIndex + 1)) + '</td><td>' + initDefectLocation(func, null, (rowIndex + 1)) + '</td><td id="def-0' + (rowIndex + 1) + '' + func + '"></td><td id="def-1' + (rowIndex + 1) + '' + func + '"></td></tr>';
        $(tbody).append(row);

        $('#def_' + (func + '_' + (rowIndex + 1))).select2();
        $('#loc_' + (func + '_' + (rowIndex + 1))).select2();
    }
}

function initDefectCode(func, selectedValue, rowIndex) {
    var result = '<select id="def_' + (func + '_' + rowIndex) + '" onchange="changeDefectCode(this, ' + func + ', ' + rowIndex + ')" style="width: 550px" name="codes' + func + '">';
    for (var i = 0; i < defectCodes.length; i++) {
        var codes = defectCodes[i];
        if (codes[0].CatName != null) {
            result += '<optgroup label="' + codes[0].CatName + '">';
            for (var j = 0; j < codes.length; j++) {
                var selected = selectedValue == codes[j].DefId ? 'selected' : '';
                var major = codes[j].Type;
                result += '<option major="' + major + '" ' + selected + ' value="' + codes[j].DefId + '">' + (codes[j].DefCode == null || codes[j].DefCode == '' ? '- ' + codes[j].DefName : codes[j].DefCode + ' - ' + codes[j].DefName) + (codes[j].DefVN == null || codes[j].DefVN == '' ? ' -' : ' - ' + codes[j].DefVN) + '</option>';
            }
            result += '</optgroup>';
        }
        else {
            result += '<option value="0">' + codes[i].DefName + '</option>';
        }
    }

    result += '</select>';
    return result;
}

function initDefectFinal(func, rowIndex, selectedValue) {
    try {
        var result = '<select id="def_' + (func + '_' + rowIndex) + '" onchange="changeDefectFinal(this, ' + func + ', ' + rowIndex + ')" style="width: 700px" name="codes' + func + '">';
        for (var i = 0; i < defectCodes.length; i++) {
            var codes = defectCodes[i];
            if (codes[0].CatName != null) {
                result += '<optgroup label="' + codes[0].CatName + '">';
                for (var j = 0; j < codes.length; j++) {
                    var selected = selectedValue == codes[j].DefId ? 'selected' : '';
                    var major = codes[j].Type;
                    result += '<option major="' + major + '" ' + selected + ' value="' + codes[j].DefId + '">' + (codes[j].DefCode == null || codes[j].DefCode == '' ? '- ' + codes[j].DefName : codes[j].DefCode + ' - ' + codes[j].DefName) + (codes[j].DefVN == null || codes[j].DefVN == '' ? ' -' : ' - ' + codes[j].DefVN) + '</option>';
                }
                result += '</optgroup>';
            }
            else {
                result += '<option value="0">' + codes[i].DefName + '</option>';
            }
        }

        result += '</select>';
        return result;
    } catch (e) { return ''; }
}

function initDefectMajorMinor(func, type, rowIndex, defects) {
    var result = '';
    if (type == '1') {
        result = '<td id="def-0' + rowIndex + func + '"><input type="text" name="defect' + func + '" value="' + (defects || 1) + '" style="width: 95%" class="r" /></td><td id="def-1' + rowIndex + func + '"></td>';
    }
    else if (type == '2') {
        result = '<td id="def-0' + rowIndex + func + '"></td><td id="def-1' + rowIndex + func + '"><input type="text" name="defect' + func + '" value="' + (defects || 1) + '" style="width: 95%" class="r" /></td>';
    }
    else {
        result = '<td id="def-0' + rowIndex + func + '"></td><td id="def-1' + rowIndex + func + '"></td>';
    }

    return result;
}

function initDefectLocation(func, selectedValue, rowIndex) {
    try {
        var result = '<select id="loc_' + (func + '_' + rowIndex) + '" style="width: 100%" name="locations' + func + '">';
        for (var i = 0; i < defectLocations.length; i++) {
            var selected = selectedValue == defectLocations[i].LocId ? 'selected' : '';
            result += '<option ' + selected + ' value="' + defectLocations[i].LocId + '">' + defectLocations[i].LocName + '</option>';
        }

        result += '</select>';
        return result;
    } catch (e) { return ''; }
}

function initPCSQty(func, value) {
    var result = '<input type="text" class="r" value="' + (value || 10) + '" name="pcs' + func + '" style="width: 100%" />';

    return result;
}

function initDefectQty(func, value) {
    var result = '<input type="text" class="r" value="' + (value || 1) + '" name="defect' + func + '" style="width: 100%" />';

    return result;
}

function getDefectCodes(func) {
    var result = [];
    var codes = $('select[name="codes' + func + '"]');
    for (var i = 0; i < codes.length; i++) {
        result.push($(codes[i]).val());
    }

    return result.join(',');
}

function getDefectLocations(func) {
    var result = [];
    var locations = $('select[name="locations' + func + '"]');
    for (var i = 0; i < locations.length; i++) {
        result.push($(locations[i]).val());
    }

    return result.join(',');
}

function getDefectPCS(func) {
    var result = [];
    var codes = $('input[name="pcs' + func + '"]');
    for (var i = 0; i < codes.length; i++) {
        result.push($(codes[i]).val());
    }

    return result.join(',');
}

function getFinalPOs(func) {
    var pos = $('#po-list' + func + ' tbody tr');
    var items = [];
    for (var i = 0; i < pos.length; i++) {
        var ins = 0, act = 0;
        ins = parseInt($('#ins-' + func + '-' + i).val());
        act = parseInt($('#act-' + func + '-' + i).val());
        var po = {
            FinalDetailId: $('#fdid-' + func + '-' + i).val(),
            FinalId: $('#finalid' + func).val(),
            Style: $('#style-' + func + '-' + i).html(),
            Color: $('#color-' + func + '-' + i).html(),
            OrderQuantity: $('#qty-' + func + '-' + i).html(),
            InspectedQty: isNaN(ins) ? 0 : ins,
            ActualProductQty: isNaN(act) ? 0 : act
        };

        items.push(po);
    }

    return items;
}

function getDefectIds(func) {
    var result = [];
    var codes = $('input[name="ddefid' + func + '"]');
    for (var i = 0; i < codes.length; i++) {
        result.push($(codes[i]).val());
    }

    return result.join(',');
}

function getDefectTotal(func) {
    var result = [];
    var codes = $('input[name="defect' + func + '"]');
    for (var i = 0; i < codes.length; i++) {
        result.push($(codes[i]).val());
    }

    return result.join(',');
}

function changeFinalStatus(radio, commentId, commentId2, commentId3) {
    if ($(radio).val() == 'on') {
        $('#' + commentId).val('').removeAttr('disabled').focus();
        $('#' + commentId2).val('').attr('disabled', 'disabled');
        $('#' + commentId3).val('').attr('disabled', 'disabled');
    }
}

function changeAQL(radio, fn, fn2, fn3, func) {
    var columns = $('.col1' + fn + func);
    var columns2 = $('.col1' + fn2 + func);
    var columns3 = $('.col1' + fn3 + func);

    for (var i = 0; i < columns.length; i++) {
        $(columns[i]).show();
    }

    for (var i = 0; i < columns2.length; i++) {
        $(columns2[i]).hide();
    }

    for (var i = 0; i < columns3.length; i++) {
        $(columns3[i]).hide();
    }
}

function changeAQL2(radio, fn, fn2, fn3, func) {
    var columns = $('.col2' + fn + func);
    var columns2 = $('.col2' + fn2 + func);
    var columns3 = $('.col2' + fn3 + func);

    for (var i = 0; i < columns.length; i++) {
        $(columns[i]).show();
    }

    for (var i = 0; i < columns2.length; i++) {
        $(columns2[i]).hide();
    }

    for (var i = 0; i < columns3.length; i++) {
        $(columns3[i]).hide();
    }
}

function inspectionDetail(owner, custId, custPO, aiglPO, custName, factoryId) {
    try {
        var table = $(owner).parent();
        for (var i = 0; i < table[0].rows.length; i++) {
            $(table[0].rows[i]).removeClass('selected-row');
        }

        $(owner).addClass('selected-row');

        var from = $('.datepicker')[0].value;
        var to = $('.datepicker')[1].value;

        NewInspection();
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
            styles = result.Styles;
            var fabric = result.Fabric;
            var cutting = result.Cutting;
            var inline = result.Inline;
            var endline = result.Endline;
            var finishing = result.Finishing;
            var packing = result.Packing;
            var prefinal = result.Prefinal;
            var final = result.Final;

            filterChecklist(fabric.Function, fabric);

            filterDHU(cutting.Function, cutting.Item, cutting.Items, cutting.Item.CreateDate, cutting.Defects, cutting.Item.Images);
            filterDHU(inline.Function, inline.Item, inline.Items, inline.Item.CreateDate, inline.Defects, inline.Item.Images);
            filterDHU(endline.Function, endline.Item, endline.Items, endline.Item.CreateDate, endline.Defects, endline.Item.Images);
            filterDHU(finishing.Function, finishing.Item, finishing.Items, finishing.Item.CreateDate, finishing.Defects, finishing.Item.Images);
            filterDHU(packing.Function, packing.Item, packing.Items, packing.Item.CreateDate, packing.Defects, packing.Item.Images);

            filterFinal(prefinal.Function, prefinal.Item, prefinal.Items, prefinal.Item.CreateDate, prefinal.Defects, prefinal.Item.Images, prefinal.POs);
            filterFinal(final.Function, final.Item, final.Items, final.Item.CreateDate, final.Defects, final.Item.Images, final.POs);

            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function loadInspectionTable(POs) {
    try {
        $('#main tbody tr').remove();
        if (POs.length == 0) {
            $('#main tbody').append('<tr><td colspan="9" class="c">There is no information</td></tr>');
        }
        else {
            for (var i = 0; i < POs.length; i++) {
                var po = POs[i];
                $('#main').append('<tr class="' + (i == 0 ? 'selected-row' : '') + '" onclick="inspectionDetail(this, ' + po.CustId + ', \'' + po.CustPO + '\', \'' + po.AIGLPO + '\', \'' + po.Customer + '\', ' + po.FactoryId + ')"><td>' + po.AIGLPO + '</td><td>' + po.CustPO + '</td><td>' + po.Customer + '</td><td>' + po.Factory + '</td><td>' + po.Division + '</td><td>' + po.InlineDate + '</td><td>' + po.Status + '</td><td class="r">' + po.POQty + '</td><td class="r">' + po.Defect + '</td></tr>');
            }
        }
    } catch (e) { unblockUI(); }
}