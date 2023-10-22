using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.Model.Systems
{
    public class UpdatePermissionRequest
    {
        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}
