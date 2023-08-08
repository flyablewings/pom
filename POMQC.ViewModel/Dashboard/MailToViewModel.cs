namespace POMQC.ViewModels.Dashboard
{
    public class MailToViewModel
    {
        public string From { get; set; }
        
        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Attachment { get; set; }

        public string PathAndQuery { get; set; }
    }
}