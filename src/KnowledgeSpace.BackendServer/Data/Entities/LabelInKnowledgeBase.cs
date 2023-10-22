using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    [Table("LabelInKnowledgeBases")]
    public class LabelInKnowledgeBase
    {
        public int KnowledgeBaseId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string LabelId { get; set; }
        public KnowledgeBase KnowledgeBase { get; set; }
        public Label Label { get; set; }
    }
}
