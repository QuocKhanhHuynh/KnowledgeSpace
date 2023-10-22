using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace KnowledgeSpace.BackendServer.Service
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbcontext _context;
        private readonly RoleManager<Role> _roleManager;

        public IdentityProfileService(IUserClaimsPrincipalFactory<User> claimsFactory,
            UserManager<User> userManager,
            ApplicationDbcontext context,
           RoleManager<Role> roleManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            var roles = await _userManager.GetRolesAsync(user);

            var query = from p in _context.Permissions
                        join c in _context.Commands
                        on p.CommandId equals c.Id
                        join f in _context.Functions
                        on p.FunctionId equals f.Id
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name)
                        select f.Id + "_" + c.Id;
            var permissions = await query.Distinct().ToListAsync();

            //Add more claims like this
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim("fullName", user.FirstName + " " + user.LastName));
            //claims.Add(new Claim((ClaimTypes.Role, string.Join(";", roles))));
            claims.Add(new Claim("permissions", JsonConvert.SerializeObject(permissions)));

            context.IssuedClaims = claims;
        }
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
