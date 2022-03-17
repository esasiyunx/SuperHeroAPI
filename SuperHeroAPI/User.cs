using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperHeroAPI
{
    [Table("User", Schema = "AUTH")]
    public class User
    {
        public int Id { get; set; }

        [StringLength(55)]
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiredDate { get; set; }
    }
}
 