using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class FunctionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public string ParentId { get; set; }
        public string Icon { get; set; }
    }
}
