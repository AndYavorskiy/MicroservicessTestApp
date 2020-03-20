namespace AuthorizationService.Models
{
    public class UserTokenModel
    {
        public string Token { get; set; }

        public long ExpiredIn { get; set; }
    }
}
