namespace POMQC.Entities.Base
{
    public class Result
    {
        public long Id { get; set; }

        public bool IsSuccess { get { return Id > 0 || string.IsNullOrWhiteSpace(Message); } }

        public string Message { get; set; }
    }
}