using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Api.Domain.Entities
{
    [Table("TaskToDos")]
    public class TaskToDo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; init; }
        [Column("TaskName")]
        [MaxLength(300)]
        public string? TaskName { get; init; }
        [Column("success", TypeName = "bit")]
        public bool Success { get; init; }
        [Column("UserName")]
        [MaxLength(50)]
        public string? UserName { get; init; }
    }
}
