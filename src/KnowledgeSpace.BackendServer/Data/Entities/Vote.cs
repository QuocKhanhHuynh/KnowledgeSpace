using KnowledgeSpace.BackendServer.Data.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeSpace.BackendServer.Data.Entities
{
    [Table("Votes")]
    public class Vote : IDateTracking
    {
        public int KnowledgeBaseId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public KnowledgeBase KnowledgeBase { get; set; }
        public User User { get; set; }
    }
}
