using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Systems;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KnowledgeSpace.BackendServer.Data;
using Azure.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using Function = KnowledgeSpace.BackendServer.Data.Entities.Function;
using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Helpers;
using System.ComponentModel.Design;
using System.Net.Mail;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public class FunctionsController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        public FunctionsController(ApplicationDbcontext context)
        {
            _context = context;
        }
        [HttpPost]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.CREATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostFunction([FromBody] FunctionCreateRequest request)
        {
            var functionDb = await _context.Functions.FindAsync(request.Id);
            if (functionDb != null)
            {
                return BadRequest(new ApiBadRequestResponse($"function with id {request.Id} has exist"));
            }
            var function = new Function()
            {
                Id = request.Id,
                Name = request.Name,
                Icon = request.Icon,
                ParentId = request.ParentId,
                SortOrder = request.SortOrder,
                Url = request.Url
            };
            _context.Functions.Add(function);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add function"));
            }
        }
        [HttpGet("{functionId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.VIEW)]
        public async Task<IActionResult> GetById(string functionId)
        {
            var function = await _context.Functions.FindAsync(functionId);
            if (function == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found function with id {functionId}"));
            }
            var functionVM = new FunctionViewModel()
            {
                Id = function.Id,
                Name = function.Name,
                Icon = function.Icon,
                ParentId = function.ParentId,
                SortOrder = function.SortOrder,
                Url = function.Url
            };
            return Ok(functionVM);
        }
        [HttpGet]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.VIEW)]
        public async Task<IActionResult> GetFunctions()
        {
            var functions = _context.Functions;
            var functionVMs = await functions.Select(x => new FunctionViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                ParentId = x.ParentId,
                SortOrder = x.SortOrder,
                Url = x.Url
            }).ToListAsync();
            return Ok(functionVMs);
        }

        [HttpGet("{functionId}/parents")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.VIEW)]
        public async Task<IActionResult> GetFunctionsByParentId(string functionId)
        {
            var functions = _context.Functions.Where(x => x.ParentId == functionId);

            var functionvms = await functions.Select(u => new FunctionViewModel()
            {
                Id = u.Id,
                Name = u.Name,
                Url = u.Url,
                SortOrder = u.SortOrder,
                ParentId = u.ParentId,
                Icon = u.Icon
            }).ToListAsync();

            return Ok(functionvms);
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.VIEW)]
        public async Task<IActionResult> GetFunctionsPaging(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _context.Functions.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Id.Contains(keyword) || x.Name.Contains(keyword) || x.Url.Contains(keyword));
            }
            var totals = query.Count();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new FunctionViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                ParentId = x.ParentId,
                SortOrder = x.SortOrder,
                Url = x.Url
            }).ToListAsync();
            var pagination = new Pagination<FunctionViewModel>()
            {
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }

        [HttpPut("{functionId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutFunction(string functionId, [FromBody] FunctionCreateRequest request)
        {
            var function = await _context.Functions.FindAsync(functionId);
            if (function == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found function with id {functionId}"));
            }
            function.Name = request.Name;
            function.SortOrder = request.SortOrder;
            function.Url = request.Url;
            function.Icon = request.Icon;
            function.ParentId = request.ParentId;
            _context.Functions.Update(function);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot apdate function with id {functionId}"));
            }
        }
        
        [HttpDelete("{functionId}")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteFunction(string functionId)
        {
            var function = await _context.Functions.FindAsync(functionId);
            if (function == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found function with id {functionId}"));
            }
            _context.Functions.Remove(function);
            var commands = _context.CommandInFunctions.Where(x => x.FunctionId == functionId);
            _context.CommandInFunctions.RemoveRange(commands);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var functionVM = new FunctionViewModel()
                {
                    Id = function.Id,
                    Name = function.Name,
                    Icon = function.Icon,
                    ParentId = function.ParentId,
                    SortOrder = function.SortOrder,
                    Url = function.Url
                };
                return Ok(functionVM);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete function with id {functionId}"));
            }
        }

        [HttpGet("{functionId}/Commands")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.VIEW)]
        public async Task<IActionResult> GetCommandsInFunction(string functionId)
        {
            var query = from c in _context.Commands
                        join cif in _context.CommandInFunctions on c.Id equals cif.CommandId into result1
                        from command in result1.DefaultIfEmpty()
                        join f in _context.Functions on command.FunctionId equals f.Id into result2
                        from function in result2.DefaultIfEmpty()
                        select new
                        {
                            c.Id,
                            c.Name,
                            command.FunctionId
                        };
            var result = await query.Where(x => x.FunctionId.Equals(functionId)).Select(x => new CommandViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return Ok(result);
        }

        [HttpPost("{functionId}/commands")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.CREATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostCommandToFunction(string functionId, [FromBody] CommandAssignRequest request)
        {
            foreach (var commandId in request.CommandIds)
            {
                if (await _context.CommandInFunctions.FindAsync(commandId, functionId) != null)
                    return BadRequest(new ApiBadRequestResponse("This command has been existed in function"));

                var entity = new CommandInFunction()
                {
                    CommandId = commandId,
                    FunctionId = functionId
                };

                _context.CommandInFunctions.Add(entity);
            }

            if (request.AddToAllFunctions)
            {
                var otherFunctions = _context.Functions.Where(x => x.Id != functionId);
                foreach (var function in otherFunctions)
                {
                    foreach (var commandId in request.CommandIds)
                    {
                        if (await _context.CommandInFunctions.FindAsync(request.CommandIds, function.Id) == null)
                        {
                            _context.CommandInFunctions.Add(new CommandInFunction()
                            {
                                CommandId = commandId,
                                FunctionId = function.Id
                            });
                        }
                    }
                }
            }
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { request.CommandIds, functionId });
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Add command to function failed"));
            }
        }

        [HttpDelete("{functionId}/commands")]
        [ClaimRequirement(FunctionCode.SYSTEM_FUNCTION, CommandCode.UPDATE)]
        public async Task<IActionResult> DeleteCommandToFunction(string functionId, [FromQuery] CommandAssignRequest request)
        {
            foreach (var commandId in request.CommandIds)
            {
                var entity = await _context.CommandInFunctions.FindAsync(commandId, functionId);
                if (entity == null)
                    return BadRequest(new ApiBadRequestResponse("This command is not existed in function"));

                _context.CommandInFunctions.Remove(entity);
            }

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Delete command to function failed"));
            }
        }
    }
}
