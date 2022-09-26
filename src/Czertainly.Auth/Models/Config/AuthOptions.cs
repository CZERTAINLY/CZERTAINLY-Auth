namespace Czertainly.Auth.Models.Config
{
    public class AuthOptions
    {
        public const string Section = "AuthOptions";

        public bool CreateUnknownUsers { get; set; } = false;
        public bool CreateUnknownRoles { get; set; } = false;
    }
}
