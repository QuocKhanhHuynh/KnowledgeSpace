using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Systems;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Contents;
using Microsoft.EntityFrameworkCore;
using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Helpers;
using System.Net.Mail;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        public CategoriesController(ApplicationDbcontext context)
        {
            _context = context;
        }
        [HttpPost]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.CREATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostCategory([FromBody] CategoryCreateRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
                SeoAlias = request.SeoAlias,
                SeoDescription = request.SeoDescription,
                SortOrder = request.SortOrder,
                ParentId = request.ParentId
            };
            _context.Categories.Add(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add category"));
            }
        }
        [HttpGet("{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found category with id {categoryId}"));
            }
            var categoryVM = new CategoryViewModel()
            {
                Id = category.Id,
                SeoAlias = category.SeoAlias,
                SeoDescription = category.SeoDescription,
                SortOrder = category.SortOrder,
                ParentId = category.ParentId,
                Name = category.Name,
                NumberOfTickets = category.NumberOfTickets 
            };
            return Ok(categoryVM);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categorys = _context.Categories;
            var categoryVMs = await categorys.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                SeoAlias = x.SeoAlias,
                SeoDescription = x.SeoDescription,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets
            }).ToListAsync();
            return Ok(categoryVMs);
        }
        [HttpGet("filter")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.VIEW)]
        public async Task<IActionResult> GetCategoriesPaging(int pageIndex, int pageSize, string keyword = null)
        {
            var query = _context.Categories.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            var totals = query.Count();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                SeoAlias = x.SeoAlias,
                SeoDescription = x.SeoDescription,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId,
                Name = x.Name,
                NumberOfTickets = x.NumberOfTickets
            }).ToListAsync();
            var pagination = new Pagination<CategoryViewModel>()
            {
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }
        [HttpPut("{categoryId}")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutCategory(int categoryId, [FromBody] CategoryCreateRequest request)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found category with id {categoryId}"));
            }
            category.Name = request.Name;
            category.SeoAlias = request.SeoAlias;
            category.SeoDescription = request.SeoDescription;
            category.SortOrder = request.SortOrder;
            category.ParentId = request.ParentId;
            _context.Categories.Update(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot apdate category with id {categoryId}"));
            }
        }
        [HttpDelete("{categoryId}")]
        [ClaimRequirement(FunctionCode.CONTENT_CATEGORY, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found category with id {categoryId}"));
            }
            _context.Categories.Remove(category);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var categoryVM = new CategoryViewModel()
                {
                    Id = category.Id,
                    SeoAlias = category.SeoAlias,
                    SeoDescription = category.SeoDescription,
                    SortOrder = category.SortOrder,
                    ParentId = category.ParentId,
                    Name = category.Name,
                    NumberOfTickets = category.NumberOfTickets
                };
                return Ok(categoryVM);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot category attachment with id {categoryId}"));
            }
        }
    }
}
