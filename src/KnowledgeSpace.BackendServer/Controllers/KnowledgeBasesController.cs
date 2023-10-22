using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Systems;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.BackendServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using FluentAssertions.Equivalency;
using IdentityServer4.Models;
using KnowledgeSpace.BackendServer.Service;
using System.Net.Http.Headers;
using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using KnowledgeSpace.BackendServer.Extensions;
using System.Net.Mail;
using KnowledgeSpace.BackendServer.Extensions;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("IdPolicy")]
    public partial class KnowledgeBasesController : ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        private readonly ISequenceService _sequence;
        private readonly IStorageService _storageService;
        public KnowledgeBasesController(ApplicationDbcontext context, ISequenceService sequence, IStorageService storageService)
        {
            _context = context;
            _sequence = sequence;
            _storageService = storageService;
        }

        [HttpPost]
        [ClaimRequirement(FunctionCode.CONTENT_KNOWLEDGEBASE, CommandCode.CREATE)]
        [ApiValidatorFilter]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostKnowledgeBase([FromForm] KnowledgeBaseCreateRequest request)
        {
            var knowledgeBase = CreateKnowledgeBaseEntity(request);
            knowledgeBase.OwnerUserId = User.GetUserId();
            if (string.IsNullOrEmpty(knowledgeBase.SeoAlias))
            {
                knowledgeBase.SeoAlias = TextHelper.ToUnsignString(knowledgeBase.Title);
            }
            knowledgeBase.Id = await _sequence.GetKnowledgeBaseNewId();

            //Process attachment
            if (request.Attachments != null && request.Attachments.Count > 0)
            {
                foreach (var attachment in request.Attachments)
                {
                    var attachmentEntity = await SaveFile(knowledgeBase.Id, attachment);
                    _context.Attachments.Add(attachmentEntity);
                }
            }
            _context.KnowledgeBases.Add(knowledgeBase);

            //Process label
         //   if (request.Labels?.Length > 0)
         //   {
        //        await ProcessLabel(request, knowledgeBase);
        //    }

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new
                {
                    id = knowledgeBase.Id
                });
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse("Create knowledge failed"));
            }
        }

        [HttpGet("{knowledgeBaseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int knowledgeBaseId)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found knowledge base with id {knowledgeBaseId}"));
            }
            var attachments = await _context.Attachments
                .Where(x => x.KnowledgeBaseId == knowledgeBaseId)
                .Select(x => new AttachmentViewModel()
                {
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    FileSize = x.FileSize,
                    Id = x.Id,
                    FileType = x.FileType
                }).ToListAsync();
            var knowledgeBaseViewModel = CreateKnowledgeBaseViewModel(knowledgeBase);
            knowledgeBaseViewModel.Attachments = attachments;

            return Ok(knowledgeBaseViewModel);
        }

        [HttpGet]
        [ClaimRequirement(FunctionCode.CONTENT_KNOWLEDGEBASE, CommandCode.VIEW)]
        public async Task<IActionResult> GetKnowledgeBases()
        {
            var knowledgeBases = _context.KnowledgeBases;
            var knowledgeBaseVMs = await knowledgeBases.Select(x => new KnowledgeBaseQuickViewModel()
            {
                Id = x.Id,

                CategoryId = x.CategoryId,

                Title = x.Title,

                SeoAlias = x.SeoAlias,

                Description = x.Description
            }).ToListAsync();
            return Ok(knowledgeBaseVMs);
        }

        [HttpGet("latest/{take:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestKnowledgeBases(int take)
        {
            var knowledgeBases = from k in _context.KnowledgeBases
                                 join c in _context.Categories on k.CategoryId equals c.Id
                                 orderby k.CreateDate descending
                                 select new { k, c };

            var knowledgeBasevms = await knowledgeBases.Take(take)
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
                    CreateDate = u.k.CreateDate
                }).ToListAsync();
            return Ok(knowledgeBasevms);
        }

        [HttpGet("popular/{take:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPopularKnowledgeBases(int take)
        {
            var knowledgeBases = from k in _context.KnowledgeBases
                                 join c in _context.Categories on k.CategoryId equals c.Id
                                 orderby k.ViewCount descending
                                 select new { k, c };

            var knowledgeBasevms = await knowledgeBases.Take(take)
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
                    CreateDate = u.k.CreateDate
                }).ToListAsync();

            return Ok(knowledgeBasevms);
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKnowledgeBasesPaging(string? filter, int? categoryId, int pageIndex, int pageSize)
        {
            var query = from k in _context.KnowledgeBases
                        join c in _context.Categories on k.CategoryId equals c.Id
                        select new { k, c };
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.k.Title.Contains(filter));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.k.CategoryId == categoryId.Value);
            }
            var totals = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new KnowledgeBaseQuickViewModel()
            {
                Id = x.k.Id,
                CategoryId = x.k.CategoryId,
                Description = x.k.Description,
                SeoAlias = x.k.SeoAlias,
                Title = x.k.Title,
                CategoryAlias = x.c.SeoAlias,
                CategoryName = x.c.Name,
                NumberOfVotes = x.k.NumberOfVotes,
                CreateDate = x.k.CreateDate,
                NumberOfComments = x.k.NumberOfComments
            }).ToListAsync();
            var pagination = new Pagination<KnowledgeBaseQuickViewModel>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }

        [HttpGet("tags/{labelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKnowledgeBasesByTagId(string labelId, int pageIndex, int pageSize)
        {
            var query = from k in _context.KnowledgeBases
                        join lik in _context.LabelInKnowledgeBases on k.Id equals lik.KnowledgeBaseId
                        join l in _context.Labels on lik.LabelId equals l.Id
                        join c in _context.Categories on k.CategoryId equals c.Id
                        where lik.LabelId == labelId
                        select new { k, l, c };

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
                    NumberOfComments = u.k.NumberOfComments
                })
                .ToListAsync();

            var pagination = new Pagination<KnowledgeBaseQuickViewModel>
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = items,
                TotalRecords = totalRecords,
            };
            return Ok(pagination);
        }

        [HttpPut("{knowledgeBaseId}")]
        [ClaimRequirement(FunctionCode.CONTENT_KNOWLEDGEBASE, CommandCode.UPDATE)]
        [ApiValidatorFilter]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutKnowledgeBase(int knowledgeBaseId, [FromForm]KnowledgeBaseCreateRequest request)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found knowledge base with id {knowledgeBaseId}"));
            UpdateKnowledgeBase(request, knowledgeBase);
            if (request.Attachments != null && request.Attachments.Count > 0)
            {
                foreach (var attachment in request.Attachments)
                {
                    var attachmentEntity = await SaveFile(knowledgeBase.Id, attachment);
                    _context.Attachments.Add(attachmentEntity);
                }
            }
            _context.KnowledgeBases.Update(knowledgeBase);

        //    if (request.Labels?.Length > 0)
         //   {
        //        await ProcessLabel(request, knowledgeBase);
        //    }
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse($"Update knowledge base failed"));
        }

        [HttpPut("{id}/view-count")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateViewCount(int id)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(id);
            if (knowledgeBase == null)
                return NotFound();
            if (knowledgeBase.ViewCount == null)
                knowledgeBase.ViewCount = 0;

            knowledgeBase.ViewCount += 1;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{knowledgeBaseId}")]
        [ClaimRequirement(FunctionCode.CONTENT_KNOWLEDGEBASE, CommandCode.DELETE)]
        public async Task<IActionResult> DeleteKnowledgeBase(int knowledgeBaseId)
        {
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found knowledge base with id {knowledgeBaseId}"));
            }
            _context.KnowledgeBases.Remove(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var knowledgeBaseViewModel = CreateKnowledgeBaseViewModel(knowledgeBase);
                return Ok(knowledgeBaseViewModel);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete knowledge base with id {knowledgeBaseId}"));
            }
        }

        [HttpGet("{knowledgeBaseId}/labels")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLabelsByKnowledgeBaseId(int knowledgeBaseId)
        {
            var query = from lik in _context.LabelInKnowledgeBases 
                        join l in _context.Labels on lik.LabelId equals l.Id
                        orderby l.Name ascending
                        where lik.KnowledgeBaseId == knowledgeBaseId
                        select new { l.Id, l.Name };

            var labels = await query.Select(u => new LabelViewModel()
            {
                Id = u.Id,
                Name = u.Name
            }).ToListAsync();

            return Ok(labels);
        }/*
        private async Task ProcessLabel(KnowledgeBaseCreateRequest request, KnowledgeBase knowledgeBase)
        {
            foreach (var labelText in request.Labels)
            {
                if (labelText == null) continue;
                var labelId = TextHelper.ToUnsignString(labelText.ToString());
                var existingLabel = await _context.Labels.FindAsync(labelId);
                if (existingLabel == null)
                {
                    var labelEntity = new Label()
                    {
                        Id = labelId,
                        Name = labelText.ToString()
                    };
                    _context.Labels.Add(labelEntity);
                }
                if (await _context.LabelInKnowledgeBases.FindAsync(labelId, knowledgeBase.Id) == null)
                {
                    _context.LabelInKnowledgeBases.Add(new LabelInKnowledgeBase()
                    {
                        KnowledgeBaseId = knowledgeBase.Id,
                        LabelId = labelId
                    });
                }
            }
        }
        */
        private async Task<Data.Entities.Attachment> SaveFile(int knowledegeBaseId, IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            var attachmentEntity = new Data.Entities.Attachment()
            {
                FileName = fileName,
                FilePath = _storageService.GetFileUrl(fileName),
                FileSize = file.Length,
                FileType = Path.GetExtension(fileName),
                KnowledgeBaseId = knowledegeBaseId,
            };
            return attachmentEntity;
        }
        private static KnowledgeBaseViewModel CreateKnowledgeBaseViewModel(KnowledgeBase knowledgeBase)
        {
            return new KnowledgeBaseViewModel()
            {
                Id = knowledgeBase.Id,

                CategoryId = knowledgeBase.CategoryId,

                Title = knowledgeBase.Title,

                SeoAlias = knowledgeBase.SeoAlias,

                Description = knowledgeBase.Description,

                Environment = knowledgeBase.Environment,

                Problem = knowledgeBase.Problem,

                StepToReproduce = knowledgeBase.StepToReproduce,

                ErrorMessage = knowledgeBase.ErrorMessage,

                Workaround = knowledgeBase.Workaround,

                Note = knowledgeBase.Note,

                OwnerUserId = knowledgeBase.OwnerUserId,

               // Labels = !string.IsNullOrEmpty(knowledgeBase.Labels) ? knowledgeBase.Labels.Split(',') : null,

                CreateDate = knowledgeBase.CreateDate,

                LastModifiedDate = knowledgeBase.LastModifiedDate,

                NumberOfComments = knowledgeBase.NumberOfComments,

                NumberOfVotes = knowledgeBase.NumberOfVotes,

                NumberOfReports = knowledgeBase.NumberOfReports,
            };
        }

        private static KnowledgeBase CreateKnowledgeBaseEntity(KnowledgeBaseCreateRequest request)
        {
            var entity = new KnowledgeBase()
            {
                CategoryId = request.CategoryId,

                Title = request.Title,

                SeoAlias = TextHelper.ToUnsignString(request.Title),

                Description = request.Description,

                Environment = request.Environment,

                Problem = request.Problem,

                StepToReproduce = request.StepToReproduce,

                ErrorMessage = request.ErrorMessage,

                Workaround = request.Workaround,

                Note = request.Note,
                Labels = "LabelTest"
            };
          //  if (request.Labels?.Length > 0)
      //      {
       //         entity.Labels = string.Join(',', request.Labels);
     //       }
            return entity;
        }
        private static void UpdateKnowledgeBase(KnowledgeBaseCreateRequest request, KnowledgeBase knowledgeBase)
        {
            knowledgeBase.CategoryId = request.CategoryId;

            knowledgeBase.Title = request.Title;

            knowledgeBase.SeoAlias = TextHelper.ToUnsignString(request.Title);
            knowledgeBase.Description = request.Description;

            knowledgeBase.Environment = request.Environment;

            knowledgeBase.Problem = request.Problem;

            knowledgeBase.StepToReproduce = request.StepToReproduce;

            knowledgeBase.ErrorMessage = request.ErrorMessage;

            knowledgeBase.Workaround = request.Workaround;

            knowledgeBase.Note = request.Note;

         //   if (request.Labels != null)
         //       knowledgeBase.Labels = string.Join(',', request.Labels);
        }

    }
}
