using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Api.Domain.Entities
{
    [Table("UserAccounts")]
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; init; }
        [Column("UserName")]
        [MaxLength(50)]
        public string? UserName { get; init; }
        [Column("Password")]
        [MaxLength(50)]
        public string? Password { get; init; }
    }
}
