using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class CommentCreateRequest
    {
        public string Content { get; set; }
        public int KnowledgeBaseId { get; set; }
        public int? ReplyId { get; set; }
    }
}
