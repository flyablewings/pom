namespace POMQC.Entities.Base
{
    public class DroppedImage
    {
        public string name { get; set; }

        public int size { get { return 1000000; } }

        public string status { get { return "queued"; } }

        public bool accepted { get { return true; } }

        public string type { get { return "image.*"; } }

        public string url { get { return "/Images/Uploads/" + name; } }

        public bool hasImage { get { return !string.IsNullOrWhiteSpace(name); } }
    }
}