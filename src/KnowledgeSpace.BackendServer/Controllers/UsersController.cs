using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Systems;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Helpers;
using System.Net.Mail;
using System.Globalization;
using KnowledgeSpace.Model.Contents;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbcontext _context;
        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbcontext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.CREATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostUser([FromBody] UserCreateRequest request)
        {
            var idUser = Guid.NewGuid().ToString();
            var user = new User()
                {
                    Id = idUser,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Dob = DateTime.ParseExact(request.Dob, "MM/dd/yyyy", null),
                    NormalizedUserName = request.UserName.ToLower(),
                    CreateDate = DateTime.Now
                };            
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = idUser }, request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add user"));
            }
        }
        [HttpGet("{userId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.VIEW)]
        public async Task<IActionResult> GetById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found user base with id {userId}"));
            }
            var userVM = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Dob = user.Dob,
                CreateDate = user.CreateDate
            };
            return Ok(userVM);
        }
        [HttpGet]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.VIEW)]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users;
            var userVMs = await users.Select(x => new UserViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Dob = x.Dob,
                CreateDate = x.CreateDate
            }).ToListAsync();
            return Ok(userVMs);
        }
        [HttpGet("filter")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.VIEW)]
        public async Task<IActionResult> GetUsersPaging(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.UserName.Contains(keyword) || x.PhoneNumber.Contains(keyword));
            }
            var totals = query.Count();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new UserViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserName = x.UserName,
                PhoneNumber = x.PhoneNumber,
                Dob = x.Dob,
                CreateDate = x.CreateDate
            }).ToListAsync();
            var pagination = new Pagination<UserViewModel>()
            {
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }
        [HttpPut("{userId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutUser(string userId, [FromBody] UserCreateRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found user base with id {userId}"));
            }
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Dob = DateTime.ParseExact(request.Dob, "MM/dd/yyyy", null);
            user.LastModifiedDate = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot update user with id {userId}"));
            }
        }

        [HttpDelete("{userId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found user base with id {userId}"));
            }
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            var otherAdmin = adminUsers.Where(x => !x.Id.Equals(userId));
            if (otherAdmin.Count() == 0)
            {
                return BadRequest(new ApiBadRequestResponse("Cannot delete when only 1 admin"));
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var userVM = new UserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    Dob = user.Dob,
                    CreateDate = user.CreateDate
                };
                return Ok(userVM);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete user with id {userId}"));
            }
        }

        [HttpPut("{userId}/change-password")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutUserPassword(string userId, [FromBody]UserPasswordChangeRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found user base with id {userId}"));
            }
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot update password has user id {userId}"));
            }
        }

        [HttpGet("{userId}/menu")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.VIEW)]
        public async Task<IActionResult> GetMenuByUserIdPermissions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found user base with id {userId}"));
            }
            var roles = _roleManager.Roles;
            var roleUsers =await _userManager.GetRolesAsync(user);
            var result = from f in _context.Functions
                         join p in _context.Permissions on f.Id equals p.FunctionId
                         join r in roles on p.RoleId equals r.Id
                         where roleUsers.Contains(r.Name) && p.CommandId.Equals("VIEW")
                         select new FunctionViewModel()
                         {
                             Id = f.Id,
                             Name = f.Name,
                             ParentId = f.ParentId,
                             Icon = f.Icon,
                             SortOrder = f.SortOrder,
                             Url = f.Url
                         };
            var Menus = await result.OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).Distinct().ToListAsync();
            return Ok(Menus);
        }

        [HttpGet("{userId}/roles")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.VIEW)]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found user with id: {userId}"));
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("{userId}/roles")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.UPDATE)]
        public async Task<IActionResult> PostRolesToUser(string userId, [FromBody] RoleAssignRequest request)
        {
            if (request.RoleNames?.Length == 0)
            {
                return BadRequest(new ApiBadRequestResponse("Role names cannot empty"));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found user with id: {userId}"));
            var result = await _userManager.AddToRolesAsync(user, request.RoleNames);
            if (result.Succeeded)
                return Ok();

            return BadRequest(new ApiBadRequestResponse(result));
        }

        [HttpDelete("{userId}/roles")]
        [ClaimRequirement(FunctionCode.SYSTEM_USER, CommandCode.VIEW)]
        public async Task<IActionResult> RemoveRolesFromUser(string userId, [FromQuery] RoleAssignRequest request)
        {
            if (request.RoleNames?.Length == 0)
            {
                return BadRequest(new ApiBadRequestResponse("Role names cannot empty"));
            }
            if (request.RoleNames.Length == 1 && request.RoleNames[0] == Constants.SystemConstants.Roles.Admin)
            {
                return base.BadRequest(new ApiBadRequestResponse($"Cannot remove {Constants.SystemConstants.Roles.Admin} role"));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found user with id: {userId}"));
            var result = await _userManager.RemoveFromRolesAsync(user, request.RoleNames);
            if (result.Succeeded)
                return Ok();

            return BadRequest(new ApiBadRequestResponse(result));
        }

        [HttpGet("{userId}/knowledgeBases")]
        public async Task<IActionResult> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize)
        {
            var query = from k in _context.KnowledgeBases
                        join c in _context.Categories on k.CategoryId equals c.Id
                        where k.OwnerUserId == userId
                        orderby k.CreateDate descending
                        select new { k, c };

            var totalRecords = await query.CountAsync();

            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
               .Select(u => new KnowledgeBaseQuickViewModel()
               {
                   Id = u.k.Id,
                   CategoryId = u.k.CategoryId,
                   Description = u.k.Description,
                   SeoAlias = u.k.SeoAlias,
                   Title = u.k.Title,
                   CategoryAlias = u.c.SeoAlias,
                   CategoryName = u.c.Name,
                   NumberOfVotes = u.k.NumberOfVotes,
                   CreateDate = u.k.CreateDate,
                   ViewCount = u.k.ViewCount
               }).ToListAsync();

            var pagination = new Pagination<KnowledgeBaseQuickViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return Ok(pagination);
        }
    }
}
