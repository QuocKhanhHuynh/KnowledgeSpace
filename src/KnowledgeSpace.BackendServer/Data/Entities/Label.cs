using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    [Table("Labels")]
    public class Label
    {
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        [Key]
        public string Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        public List<LabelInKnowledgeBase> LabelInKnowledgeBases { get; set; }
    }
}
