namespace Server.Entities.Identity
{
    public class User : BaseEntitiy
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
