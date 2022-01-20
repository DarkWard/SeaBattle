using SeaBattleORM;

namespace SeaBattleMvc
{
    [Table("AppUser")]
    public class AppUser
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserPassword { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public string UserLogin { get; set; }

        public virtual string UserName { get; set; }

        [NormalizedName]
        public string NormalizedUserName { get; set; }

        public AppUser(string email, string password, RoleType roleType, string login, string userName, string normalizedName)
        {
            Email = email;
            UserPassword = password;
            RoleId = (int)roleType;
            UserLogin = login;
            UserName = userName;
            NormalizedUserName = normalizedName;
        }

        public AppUser(string email, string password)
        {
            Email = email;
            UserPassword = password;
        }

        public AppUser() { }
    }
}