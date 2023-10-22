using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public class CommandsController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        public CommandsController(ApplicationDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommands()
        {
            var commands = await _context.Commands.Select(x => new CommandViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return Ok(commands);
        }
    }
}
