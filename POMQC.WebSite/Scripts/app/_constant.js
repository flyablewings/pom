var Func = { 
    Dashboard: 0,
    FitSample: 1,
    PPSample: 2,
    TopSample: 3,
    Checklist: 4,
    Meeting: 5,
    Fabric: 6,
    Cutting: 7,
    Inline: 8,
    Endline: 9,
    Finishing: 10,
    Packing: 11,
    Prefinal: 12,
    Final: 13,
    ReportByPO: 14,
    ReportByFactory: 15,
    Sample: 16,
    Inspection: 17,
    Report: 18,
    ReportAllFactories: 19,
    ReportFinal: 20,
    ReportDhuDetail: 21,
    ReportDhuWeekly: 22,
    ReportDhuMonthly: 23
};

var Checklist = {
    FitSample: 1,
    PPSample: 2,
    TopSample: 3,
    QAChecklist: 4,
    PPMeeting: 5,
    Fabric: 6
};

var DHU = {
    Cutting: 1,
    Inline: 2,
    Endline: 3,
    Finishing: 4,
    Packing: 5,
    Prefinal: 6,
    Final: 7
};

function formatInfo (str) {
    var str = (str == '01/01/0001 00:00:00.000' || str == null || str == undefined || str == 'null' || str == 'undefined') ? '--' : str;
    return  str;
}