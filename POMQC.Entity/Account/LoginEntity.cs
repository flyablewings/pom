namespace POMQC.Entities.Account
{
    public class LoginEntity
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Epwd { get; set; }
    }
}