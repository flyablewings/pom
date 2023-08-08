/// <reference path="../_references.js" />
function dashboardDetail(owner, custId, custPO, aiglPO, custName, factoryId) {
    try {
        var table = $(owner).parent();
        for (var i = 0; i < table[0].rows.length; i++) {
            $(table[0].rows[i]).removeClass('selected-row');
        }

        $(owner).addClass('selected-row');
        viewDashboardDetail(custId, custPO, aiglPO, custName, factoryId);
    } catch (e) { unblockUI(); }
}

function viewDashboard() {
    blockUI();
    try {
        var factoryId = $('#ddlFACT').val();
        var custId = $('#ddlCUST').val();
        var custPO = $('#ddlCUSTPO').val();
        var aiglPO = $('#ddlAIGLPO').val();
        var from = $('.datepicker')[0].value;
        var to = $('.datepicker')[1].value;
        var status = $('#ddlStatus option:selected').text();

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
            styles = result.Styles;
            resetTabs();
            loadDashboardTable(result.POs);

            loadChecklist(result.FitSample, Checklist.FitSample);
            loadChecklist(result.PPSample, Checklist.PPSample);
            loadChecklist(result.TopSample, Checklist.TopSample);
            loadChecklist(result.QAChecklist, Checklist.QAChecklist);
            loadChecklist(result.PPMeeting, Checklist.PPMeeting);
            loadChecklist(result.Fabric, Checklist.Fabric);

            loadDHU(result.Cutting.Item, result.Cutting.Defects, DHU.Cutting, result.Cutting.Item.CreateDate, result.Cutting.Items);
            loadDHU(result.Inline.Item, result.Inline.Defects, DHU.Inline, result.Inline.Item.CreateDate, result.Inline.Items);
            loadDHU(result.Endline.Item, result.Endline.Defects, DHU.Endline, result.Endline.Item.CreateDate, result.Endline.Items);
            loadDHU(result.Finishing.Item, result.Finishing.Defects, DHU.Finishing, result.Finishing.Item.CreateDate, result.Finishing.Items);
            loadDHU(result.Packing.Item, result.Packing.Defects, DHU.Packing, result.Packing.Item.CreateDate, result.Packing.Items);

            loadFinal(result.Prefinal.Item, result.Prefinal.Defects, DHU.Prefinal, result.Prefinal.Item.CreateDate, result.Prefinal.Items, false, result.Prefinal.POs);
            loadFinal(result.Final.Item, result.Final.Defects, DHU.Final, result.Final.Item.CreateDate, result.Final.Items, false, result.Final.POs);
            bindReport(result.ReportItems, Func.ReportByPO, result.ReportTitle);

            var selectedTab = $("ul#dashboard-tab li.active")[0].firstChild.href.split('#')[1];
            setActiveTab(selectedTab);
            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function viewDashboardDetail(custId, custPO, aiglPO, custName, factoryId) {
    blockUI();
    try {
        var from = $('.datepicker')[0].value;
        var to = $('.datepicker')[1].value;

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
            resetTabs();

            $('.swiper-wrapper').empty(); // Empty loaded images
            loadChecklist(result.FitSample, Checklist.FitSample);
            loadChecklist(result.PPSample, Checklist.PPSample);
            loadChecklist(result.TopSample, Checklist.TopSample);
            loadChecklist(result.QAChecklist, Checklist.QAChecklist);
            loadChecklist(result.PPMeeting, Checklist.PPMeeting);
            loadChecklist(result.Fabric, Checklist.Fabric);

            loadDHU(result.Cutting.Item, result.Cutting.Defects, DHU.Cutting, result.Cutting.Item.CreateDate, result.Cutting.Items);
            loadDHU(result.Inline.Item, result.Inline.Defects, DHU.Inline, result.Inline.Item.CreateDate, result.Inline.Items);
            loadDHU(result.Endline.Item, result.Endline.Defects, DHU.Endline, result.Endline.Item.CreateDate, result.Endline.Items);
            loadDHU(result.Finishing.Item, result.Finishing.Defects, DHU.Finishing, result.Finishing.Item.CreateDate, result.Finishing.Items);
            loadDHU(result.Packing.Item, result.Packing.Defects, DHU.Packing, result.Packing.Item.CreateDate, result.Packing.Items);

            loadFinal(result.Prefinal.Item, result.Prefinal.Defects, DHU.Prefinal, result.Prefinal.Item.CreateDate, result.Prefinal.Items, false, result.Prefinal.POs);
            loadFinal(result.Final.Item, result.Final.Defects, DHU.Final, result.Final.Item.CreateDate, result.Final.Items, false, result.Final.POs);
            bindReport(result.ReportItems, Func.ReportByPO, result.ReportTitle);

            var selectedTab = $("ul#dashboard-tab li.active")[0].firstChild.href.split('#')[1];
            setActiveTab(selectedTab);
            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function viewDHUDetail(owner, custId, custPO, aiglPO, date, type, func) {
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
            resetTabs();

            loadDHU(result.Item, result.Defects, func, result.Item.CreateDate, result.Items, true);

            var selectedTab = $("ul#dashboard-tab li.active")[0].firstChild.href.split('#')[1];
            setActiveTab(selectedTab);
            unblockUI();
        });
    } catch (e) { unblockUI(); }
}

function viewFinalDetail(owner, custId, custPO, aiglPO, date, type, func) {
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
            resetTabs();

            loadFinal(result.Item, result.Defects, func, result.Item.CreateDate, result.Items, true, result.POs);

            var selectedTab = $("ul#dashboard-tab li.active")[0].firstChild.href.split('#')[1];
            setActiveTab(selectedTab);
            unblockUI();
        });
    } catch (e) { unblockUI(); }
}


function loadDashboardTable(POs) {
    try {
        $('#main tbody tr').remove();
        if (POs.length == 0) {
            $('#main tbody').append('<tr><td colspan="9" class="c">There is no information</td></tr>');
        }
        else {
            for (var i = 0; i < POs.length; i++) {
                var po = POs[i];
                $('#main').append('<tr class="' + (i == 0 ? 'selected-row' : '') + '" onclick="dashboardDetail(this, ' + po.CustId + ', \'' + po.CustPO + '\', \'' + po.AIGLPO + '\', \'' + po.Customer + '\', ' + po.FactoryId + ')"><td>' + po.AIGLPO + '</td><td>' + po.CustPO + '</td><td>' + po.Customer + '</td><td>' + po.Factory + '</td><td>' + po.Division + '</td><td>' + po.InlineDate + '</td><td>' + po.Status + '</td><td class="r">' + po.POQty + '</td><td class="r">' + po.Defect + '</td></tr>');
            }
        }
    } catch (e) { unblockUI(); }
}

function loadChecklist(checklist, func) {
    try {
        loadChecklistTable(checklist, func);
        var doc = $('#doc-item' + func).empty();
        var img = $('#img' + func).empty();
        $('#comment' + func).empty();

        if (checklist.Doc != null) {
            $('#errmsg' + func).html('').removeClass('error').hide();

            for (var i = 0; i < checklist.Images.length; i++) {
                var image = checklist.Images[i];
                img.append('<a class="swiper-slide" data-lightbox="img' + func + '" href="/Images/Uploads/' + image + '" style="background:url(/Images/Uploads/' + image + ')"></a>');
            }

            if (func == 1) {
                swiper1 = new Swiper('.swiper-container1', {
                    nextButton: '.swiper-button-next1',
                    prevButton: '.swiper-button-prev1',
                    effect: 'coverflow',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    autoplay: 5000,
                    paginationClickable: true,
                    autoplayDisableOnInteraction: false,
                    coverflow: {
                        rotate: 50,
                        stretch: 0,
                        depth: 100,
                        modifier: 1,
                        slideShadows: true
                    }
                });
            }
            if (func == 2) {
                swiper2 = new Swiper('.swiper-container2', {
                    nextButton: '.swiper-button-next2',
                    prevButton: '.swiper-button-prev2',
                    effect: 'coverflow',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    autoplay: 5000,
                    paginationClickable: true,
                    autoplayDisableOnInteraction: false,
                    coverflow: {
                        rotate: 50,
                        stretch: 0,
                        depth: 100,
                        modifier: 1,
                        slideShadows: true
                    }
                });
            }
            if (func == 3) {
                swiper3 = new Swiper('.swiper-container3', {
                    nextButton: '.swiper-button-next3',
                    prevButton: '.swiper-button-prev3',
                    effect: 'coverflow',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    autoplay: 5000,
                    paginationClickable: true,
                    autoplayDisableOnInteraction: false,
                    coverflow: {
                        rotate: 50,
                        stretch: 0,
                        depth: 100,
                        modifier: 1,
                        slideShadows: true
                    }
                });
            }
            if (func == 4) {
                swiper4 = new Swiper('.swiper-container4', {
                    nextButton: '.swiper-button-next4',
                    prevButton: '.swiper-button-prev4',
                    effect: 'coverflow',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    autoplay: 5000,
                    paginationClickable: true,
                    autoplayDisableOnInteraction: false,
                    coverflow: {
                        rotate: 50,
                        stretch: 0,
                        depth: 100,
                        modifier: 1,
                        slideShadows: true
                    }
                });
            }
            if (func == 5) {
                swiper5 = new Swiper('.swiper-container5', {
                    nextButton: '.swiper-button-next5',
                    prevButton: '.swiper-button-prev5',
                    effect: 'coverflow',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    autoplay: 5000,
                    paginationClickable: true,
                    autoplayDisableOnInteraction: false,
                    coverflow: {
                        rotate: 50,
                        stretch: 0,
                        depth: 100,
                        modifier: 1,
                        slideShadows: true
                    }
                });
            }
            if (func == 6) {
                swiper6 = new Swiper('.swiper-container6', {
                    nextButton: '.swiper-button-next6',
                    prevButton: '.swiper-button-prev6',
                    effect: 'coverflow',
                    grabCursor: true,
                    centeredSlides: true,
                    slidesPerView: 'auto',
                    autoplay: 5000,
                    paginationClickable: true,
                    autoplayDisableOnInteraction: false,
                    coverflow: {
                        rotate: 50,
                        stretch: 0,
                        depth: 100,
                        modifier: 1,
                        slideShadows: true
                    }
                });
            }

            for (var i = 0; i < checklist.Documents.length; i++) {
                var dc = checklist.Documents[i];
                doc.append('<a class="doc" href="/Images/Uploads/' + dc + '" target="_blank">' + dc + '</a>');
            }

            $('#comment' + func).html(checklist.Comment);

            if (checklist.Documents.length > 0) {
                $('#doc' + func).show();
            }
            else {
                $('#doc' + func).hide();
            }
        }
        else {
            $('#doc' + func).hide();

            var funcName = '';
            switch (func) {
                case Checklist.FitSample:
                    funcName = 'Fit Sample Approved';
                    break;
                case Checklist.PPSample:
                    funcName = 'PP Sample Approved';
                    break;
                case Checklist.TopSample:
                    funcName = 'Top Sample Approved';
                    break;
                case Checklist.QAChecklist:
                    funcName = 'QA/QC Checklist';
                    break;
                case Checklist.PPMeeting:
                    funcName = 'PP Meeting';
                    break;
                default:
                    funcName = "Fabric Inspection";
                    break;
            }

            //$('#errmsg' + func).html('This Customer PO does not have ' + funcName).addClass('error').show();
        }
    } catch (e) { unblockUI(); }
}

function loadTableDHU(func, DHUs) {
    try {
        var table = $('#dhu' + func + ' tbody').empty();
        if (DHUs.length == 0) {
            table.append('<tr><td colspan="5" class="c">There is no information</td></tr>');
        }
        else {
            for (var i = 0; i < DHUs.length; i++) {
                table.append('<tr onclick="viewDHUDetail(this, ' + DHUs[i].CustId + ', \'' + DHUs[i].CustPO + '\', \'' + DHUs[i].AIGLPO + '\', \'' + DHUs[i].CreateDate + '\', ' + func + ', ' + func + ')" class="' + (i == 0 ? 'selected-row' : '') + '"><td>' + (i + 1) + '</td><td>' + DHUs[i].CreateDate + '</td><td>' + DHUs[i].CreatedUser + '</td><td>' + formatInfo(DHUs[i].UpdateDate) + '</td><td>' + formatInfo(DHUs[i].UpdatedUser) + '</td></tr>');
            }
        }
    } catch (e) { unblockUI(); }
}

function loadTableFinal(func, DHUs) {
    try {
        var table = $('#final' + func + ' tbody').empty();
        if (DHUs.length == 0) {
            table.append('<tr><td colspan="5" class="c">There is no information</td></tr>');
        }
        else {
            for (var i = 0; i < DHUs.length; i++) {
                table.append('<tr onclick="viewFinalDetail(this, ' + DHUs[i].CustId + ', \'' + DHUs[i].CustPO + '\', \'' + DHUs[i].AIGLPO + '\', \'' + DHUs[i].CreateDate + '\', ' + func + ', ' + func + ')" class="' + (i == 0 ? 'selected-row' : '') + '"><td>' + (i + 1) + '</td><td>' + DHUs[i].CreateDate + '</td><td>' + DHUs[i].CreatedUser + '</td><td>' + formatInfo(DHUs[i].UpdateDate) + '</td><td>' + formatInfo(DHUs[i].UpdatedUser) + '</td></tr>');
            }
        }
    } catch (e) { unblockUI(); }
}

function loadDHU(dhu, defects, func, createdDate, DHUs, ignoreLoadTable) {
    try {
        if (!ignoreLoadTable) {
            loadTableDHU(func, DHUs);
        }

        loadDHUGeneralInfo(dhu, createdDate, func);
        viewDHUDefect(defects, func);

        $('#export-dhu' + func).prop("href", "/dhu/export?custid=" + dhu.CustId + "&custpo=" + dhu.CustPO + "&aiglpo=" + dhu.AIGLPO + "&date=" + dhu.CreateDate + "&type=" + func);
        $('#export-dhu2' + func).prop("href", "/dhu/exportpdf?custid=" + dhu.CustId + "&custpo=" + dhu.CustPO + "&aiglpo=" + dhu.AIGLPO + "&date=" + dhu.CreateDate + "&type=" + func);
        $('#export-dhu3' + func).prop("href", "javascript:sendMail(true, " + dhu.CustId + ", '" + dhu.CustPO + "', '" + dhu.AIGLPO + "', '" + dhu.CreateDate + "', " + func + ");");

        var mockFiles = dhu.Images;
        // Load swiper
        $('#img2' + func).empty();
        for (var i = 0; i < mockFiles.length; i++) {
            if (mockFiles[i].hasImage) {
                var item = mockFiles[i];
                $('#img2' + func).append('<a class="swiper-slide" data-lightbox="img2' + func + '" href="/Images/Uploads/' + item.name + '" style="background:url(/Images/Uploads/' + item.name + ')"></a>');
            }
        }
        if (func == 1) {
            swiper21 = new Swiper('.swiper-container21', {
                nextButton: '.swiper-button-next21',
                prevButton: '.swiper-button-prev21',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
        if (func == 2) {
            swiper22 = new Swiper('.swiper-container22', {
                nextButton: '.swiper-button-next22',
                prevButton: '.swiper-button-prev22',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
        if (func == 3) {
            swiper23 = new Swiper('.swiper-container23', {
                nextButton: '.swiper-button-next23',
                prevButton: '.swiper-button-prev23',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
        if (func == 4) {
            swiper24 = new Swiper('.swiper-container24', {
                nextButton: '.swiper-button-next24',
                prevButton: '.swiper-button-prev24',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
        if (func == 5) {
            swiper25 = new Swiper('.swiper-container25', {
                nextButton: '.swiper-button-next25',
                prevButton: '.swiper-button-prev25',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
    } catch (e) { unblockUI(); }
}

function loadFinal(final, defects, func, createdDate, DHUs, ignoreLoadTable, POs) {
    try {
        if (!ignoreLoadTable) {
            loadTableFinal(func, DHUs);
        }

        loadFinalGeneralInfo(final, createdDate, func, POs);
        loadFinalDefect(defects, func, POs);

        var mockFiles = final.Images;
        // Load swiper
        $('#img2' + func).empty();
        for (var i = 0; i < mockFiles.length; i++) {
            if (mockFiles[i].hasImage) {
                var item = mockFiles[i];
                $('#img2' + func).append('<a class="swiper-slide" data-lightbox="img2' + func + '" href="/Images/Uploads/' + item.name + '" style="background:url(/Images/Uploads/' + item.name + ')"></a>');
            }
        }
        if (func == 6) {
            swiper26 = new Swiper('.swiper-container26', {
                nextButton: '.swiper-button-next26',
                prevButton: '.swiper-button-prev26',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
        if (func == 7) {
            swiper27 = new Swiper('.swiper-container27', {
                nextButton: '.swiper-button-next27',
                prevButton: '.swiper-button-prev27',
                effect: 'coverflow',
                grabCursor: true,
                centeredSlides: true,
                slidesPerView: 'auto',
                autoplay: 5000,
                paginationClickable: true,
                autoplayDisableOnInteraction: false,
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                }
            });
        }
    } catch (e) { unblockUI(); }
}

function viewDHUDefect(defects, func) {
    try {
        var totalDefects = 0;
        var defectProducts = defects.length;
        var defectRateProduct = 0;
        var defectRate = 0;
        var pcs = 0;
        var counting = 0;

        try {
            var tbody = $('#tbl-dhu' + func + ' tbody')[0];
            for (var i = tbody.rows.length - 1; i > 0; i--) {
                $(tbody.rows[i]).remove();
            }
        } catch (e) { }

        if (defects.length > 0) {
            for (var i = 0; i < defects.length; i++) {
                loadDefectValues(func, defects[i].DefId, defects[i].LocId, defects[i].PCSQty, defects[i].Total, defects[i].DdefId, i);
                totalDefects += defects[i].Total;
                pcs += defects[i].PCSQty;
                counting += defects[i].Total;
            }

            $('#grand-1' + func).html(totalDefects);
            $('#grand-2' + func).html(pcs);
            $('#grand-3' + func).html((counting * 100 / parseFloat($('#audit-size' + func).val())).toFixed(2) + '%');
            $('#grand-4' + func).html((pcs / parseFloat($('#audit-size' + func).val()) * 100).toFixed(2) + '%');
        }
        else {
            $('#grand-1' + func).html('0');
            $('#grand-2' + func).html('0');
            $('#grand-3' + func).html('0%');
            $('#grand-4' + func).html('0%');
        }
    } catch (e) { unblockUI(); }
}

function clearActiveTabs() {
    $('#FitSample').removeClass('active');
    $('#PPSample').removeClass('active');
    $('#TopSample').removeClass('active');
    $('#QAChecklist').removeClass('active');
    $('#PPMeeting').removeClass('active');
    $('#Fabric').removeClass('active');
    $('#Cutting').removeClass('active');
    $('#Inline').removeClass('active');
    $('#Endline').removeClass('active');
    $('#Finishing').removeClass('active');
    $('#Packing').removeClass('active');
    $('#Prefinal').removeClass('active');
    $('#FitSample').removeClass('active');
    $('#ReportByPO').removeClass('active');
}

function setActiveTabs() {
    $('#FitSample').addClass('active');
    $('#PPSample').addClass('active');
    $('#TopSample').addClass('active');
    $('#QAChecklist').addClass('active');
    $('#PPMeeting').addClass('active');
    $('#Fabric').addClass('active');
    $('#Cutting').addClass('active');
    $('#Inline').addClass('active');
    $('#Endline').addClass('active');
    $('#Finishing').addClass('active');
    $('#Packing').addClass('active');
    $('#Prefinal').addClass('active');
    $('#FitSample').addClass('active');
    $('#ReportByPO').addClass('active');
}

function resetTabs() {
    clearActiveTabs();
    setActiveTabs();
}

function setActiveTab(tab) {
    clearActiveTabs();
    $('#' + tab).addClass('active');
}