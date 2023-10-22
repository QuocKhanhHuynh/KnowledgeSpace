using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    [Table("CommandInFunctions")]
    public class CommandInFunction
    {
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string CommandId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string FunctionId { get; set; }
        public Command Command { get; set; }
        public Function Function {get; set;}
    }
}
