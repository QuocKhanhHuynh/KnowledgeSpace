using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Contents
{
    public class ReportCreateRequest
    {

        public int KnowledgeBaseId { get; set; }
        public string Content { get; set; }
    }
}
