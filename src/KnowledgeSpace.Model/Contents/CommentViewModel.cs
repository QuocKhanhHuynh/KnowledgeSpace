using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int KnowledgeBaseId { get; set; }
        public string OwnerUserId { get; set; }
        public string OwnerName { get; set; }
        public string KnowledgeBaseTitle { get; set; }
        public string KnowledgeBaseSeoAlias { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public int? ReplyId { get; set; }
        public List<CommentViewModel> Children { get; set; } = new List<CommentViewModel>();
    }
}
