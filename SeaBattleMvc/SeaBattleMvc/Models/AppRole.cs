using Microsoft.AspNetCore.Identity;
using SeaBattleORM;

namespace SeaBattleMvc
{
    [Table("UserRole")]
    public class AppRole : IdentityRole<int>
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string RoleName { get; set; }

        [NormalizedName]
        public string NormalizedRoleName { get; set; }

        public AppRole(int id, RoleType roleType, string normalizedRoleName)
        {
            Id = id;
            RoleName = roleType.ToString();
            NormalizedRoleName = normalizedRoleName;
        }

        public AppRole() { }
    }
}