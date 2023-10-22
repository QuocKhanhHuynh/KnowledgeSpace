using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Helpers;
using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbcontext _context;
        public RolesController(RoleManager<Role> roleManager, ApplicationDbcontext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.CREATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostRole([FromBody] RoleCreateRequest request)
        {
            var role = new Role()
            {
                Id = request.Id,
                Name = request.Name,
                NormalizedName = request.Name.ToUpper()
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add role"));
            }
        }
        [HttpGet("{roleId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.VIEW)]
        public async Task<IActionResult> GetById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found role base with id {roleId}"));
            }
            var roleVM = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return Ok(roleVM);
        }
        [HttpGet]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.VIEW)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = _roleManager.Roles;
            var roleVMs = await roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return Ok(roleVMs);
        }
        [HttpGet("filter")]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.VIEW)]
        public async Task<IActionResult> GetRolesPaging(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Id.Contains(keyword) || x.Name.Contains(keyword));
            }
            var totals = query.Count();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            var pagination = new Pagination<RoleViewModel>()
            {
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }

        [HttpPut("{roleId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutRole(string roleId, [FromBody] RoleCreateRequest request)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found role with id {roleId}"));
            }
            role.Name = request.Name;
            role.NormalizedName = role.Name.ToUpper();
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot update role with id {roleId}"));
            }
        }

        [HttpDelete("{roleId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found role with id {roleId}"));
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                var roleVM = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return Ok(roleVM);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete role with id {roleId}"));
            }
        }

        [HttpGet("{roleId}/permissions")]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.VIEW)]
        public async Task<IActionResult> GetPermissionByRoleId(string roleId)
        {
            var result = await _context.Permissions.Where(x => x.RoleId.Equals(roleId)).Select(x => new PermissionViewModel()
            {
                RoleId = x.RoleId,
                CommandId = x.CommandId,
                FunctionId = x.FunctionId
            }).ToListAsync();
            return Ok(result);
        }

        [HttpPut("{roleId}/Permissions")]
        [ClaimRequirement(FunctionCode.SYSTEM_ROLE, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutPermissionsById(string roleId, [FromBody] UpdatePermissionRequest request)
        {
            var oldPermission =_context.Permissions.Where(x => x.RoleId.Equals(roleId));
            if (oldPermission.Count() == 0) 
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found old permission has role id {roleId}"));
            }
            _context.Permissions.RemoveRange(oldPermission);
            var newPermission = new List<Permission>();
            foreach (var permission in request.Permissions)
            {
                newPermission.Add(new Permission(permission.FunctionId, permission.RoleId, permission.CommandId));
            }
            _context.Permissions.AddRange(newPermission.Distinct(new MyPermissionComparer()));
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot update permission"));
            }
        }

        private class MyPermissionComparer : IEqualityComparer<Permission>
        {
            // Items are equal if their ids are equal.
            public bool Equals(Permission x, Permission y)
            {
                // Check whether the compared objects reference the same data.
                if (Object.ReferenceEquals(x, y)) return true;

                // Check whether any of the compared objects is null.
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                //Check whether the items properties are equal.
                return x.CommandId == y.CommandId && x.FunctionId == x.FunctionId && x.RoleId == x.RoleId;
            }

            // If Equals() returns true for a pair of objects
            // then GetHashCode() must return the same value for these objects.

            public int GetHashCode(Permission permission)
            {
                //Check whether the object is null
                if (Object.ReferenceEquals(permission, null)) return 0;

                //Get hash code for the ID field.
                int hashProductId = (permission.CommandId + permission.FunctionId + permission.RoleId).GetHashCode();

                return hashProductId;
            }
        }
    }
}
