using Dapper;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public class PermissionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PermissionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                var sql = @"select f.Id, f.Name, f.ParentId,
                        sum(case when c.Id='CREATE' then 1 else 0 end) hasCreate,
                        sum(case when c.Id='APPROVE' then 1 else 0 end) hasApprove,
                        sum(case when c.Id='DELETE' then 1 else 0 end) hasDelete,
                        sum(case when c.Id='VIEW' then 1 else 0 end) hasView,
                        sum(case when c.Id='UPDATE' then 1 else 0 end) hasUpdate
                        from CommandInFunctions cif left join Commands c on cif.CommandId = c.Id
                        join Functions f on f.Id = cif.FunctionId
                        group by f.Id, f.Name, f.ParentId order by f.ParentId;";
                var result = await conn.QueryAsync<PermissionsScreenViewModel>(sql, null, null, 120, CommandType.Text);
                return Ok(result.ToList());
            }
        }
    }
}
