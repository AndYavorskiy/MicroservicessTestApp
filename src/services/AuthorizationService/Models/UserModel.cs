namespace AuthorizationService.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
