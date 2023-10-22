using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    [Table("Commands")]
    public class Command
    {
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Key]
        public string Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public List<CommandInFunction> CommandInFunctions { get; set; }
        public List<Permission> Permissions {set; get; }
    }
}
