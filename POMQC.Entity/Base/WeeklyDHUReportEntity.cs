namespace POMQC.Entities.Base
{
    public class WeeklyDHUReportEntity
    {
        public WeeklyDHUReportEntity()
        {
            Week1 = new WeeklyDHUEntity();
            Week2 = new WeeklyDHUEntity();
            Week3 = new WeeklyDHUEntity();
            Week4 = new WeeklyDHUEntity();
        }

        public string FactoryName { get; set; }

        public WeeklyDHUEntity Week1 { get; set; }

        public WeeklyDHUEntity Week2 { get; set; }

        public WeeklyDHUEntity Week3 { get; set; }

        public WeeklyDHUEntity Week4 { get; set; }
    }
}