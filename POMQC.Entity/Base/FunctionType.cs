namespace POMQC.Entities.Base
{
    public enum ChecklistType
    {
        Unknown = 0,
        FitSample = 1,
        PPSample = 2,
        TopSample = 3,
        QAChecklist = 4,
        PPMeeting = 5,
        Fabric = 6
    }

    public enum DHUType
    {
        Unknown = 0,
        Cutting = 1,
        Inline = 2,
        Endline = 3,
        Finishing = 4,
        Packing = 5,
        Prefinal = 6,
        Final = 7
    }

    public enum DHUItem
    {
        Unknown = 0,
        Shirt = 1,
        Pant = 2,
        Short = 3,
        CoatJacket = 4
    }

    public enum DHUStyle
    {
        Unknown = 0,
        Woven = 1,
        Knit = 2
    }

    public enum StatusType
    {
        Unknown = 0,
        Pass = 1,
        Failed = 2,
        OnHold = 3
    }

    public enum DefectType
    {
        Unknown = 0,
        Major = 1,
        Minor = 2
    }

    public enum AQL
    {
        Unknown = 0,
        AQL10 = 1,
        AQL15 = 2,
        AQL25 = 3,
        AQL40 = 4,
        AQL65 = 5,
        AQL252 = 6
    }
}