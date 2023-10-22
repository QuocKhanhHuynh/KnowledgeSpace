using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KnowledgeSpace.BackendServer.Data.Entities;
using Azure.Core;
using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Helpers;
using System.Net.Mail;
using KnowledgeSpace.BackendServer.Extensions;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public partial class KnowledgeBasesController
    {
        [HttpGet("{knowledgeBaseId}/Comment/filter")]
        public async Task<IActionResult> GetCommentsPaging(int? knowledgeBaseId, int pageIndex, int pageSize, string filter = null)
        {
            var query = from c in _context.Comments
                        join u in _context.Users
                            on c.OwnerUserId equals u.Id
                        select new { c, u };
            if (knowledgeBaseId.HasValue)
            {
                query = query.Where(x => x.c.KnowledgeBaseId == knowledgeBaseId.Value);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.c.Content.Contains(filter));
            }
            var totals = query.Count();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new CommentViewModel()
            {
                Id = x.c.Id,
                Content = x.c.Content,
                CreateDate = x.c.CreateDate,
                KnowledgeBaseId = x.c.KnowledgeBaseId,
                LastModifiedDate = x.c.LastModifiedDate,
                OwnerUserId = x.c.OwnerUserId,
                ReplyId = x.c.ReplyId
            }).ToListAsync();
            var pagination = new Pagination<CommentViewModel>()
            {
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }

        [HttpGet("{knowledgeBaseId}/comments/{commentId}")]
        public async Task<IActionResult> GetCommentDetail(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found comment with id {commentId}"));
            }
            var user = await _context.Users.FindAsync(comment.OwnerUserId);
            var commentVM = new CommentViewModel()
            {
                Id = comment.Id,
                Content = comment.Content,
                CreateDate = comment.CreateDate,
                KnowledgeBaseId = comment.KnowledgeBaseId,
                LastModifiedDate = comment.LastModifiedDate,
                OwnerUserId = comment.OwnerUserId,
                ReplyId = comment.ReplyId,
                OwnerName = user.FirstName + " " + user.LastName
            };
            return Ok(commentVM);
        }

        [HttpGet("comments/recent/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRecentComments(int take)
        {
            var query = from c in _context.Comments
                        join u in _context.Users
                            on c.OwnerUserId equals u.Id
                        join k in _context.KnowledgeBases
                        on c.KnowledgeBaseId equals k.Id
                        orderby c.CreateDate descending
                        select new { c, u, k };

            var comments = await query.Take(take).Select(x => new CommentViewModel()
            {
                Id = x.c.Id,
                CreateDate = x.c.CreateDate,
                KnowledgeBaseId = x.c.KnowledgeBaseId,
                OwnerUserId = x.c.OwnerUserId,
                KnowledgeBaseTitle = x.k.Title,
                OwnerName = x.u.FirstName + " " + x.u.LastName,
                KnowledgeBaseSeoAlias = x.k.SeoAlias
            }).ToListAsync();

            return Ok(comments);
        }

        [HttpPost("{knowledgeBaseId}/comments")]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostComment([FromBody] CommentCreateRequest request)
        {
            var comment = new Comment()
            {
                Content = request.Content,
                KnowledgeBaseId = request.KnowledgeBaseId,
                OwnerUserId = User.GetUserId(),
                ReplyId =  request.ReplyId
            };
            _context.Comments.Add(comment);
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(request.KnowledgeBaseId);
            if (knowledgeBase == null)
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot knowledge base with id {request.KnowledgeBaseId} not exist"));
            }
            var numberOfComents = knowledgeBase.NumberOfComments.GetValueOrDefault(0) + 1;
            knowledgeBase.NumberOfComments = numberOfComents;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetCommentDetail), new { commentId = comment.Id }, request);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add comment"));
            }
        }

        [HttpPut("{knowledgeBaseId}/comments/{commentId}")]
        [ApiValidatorFilter]
        public async Task<IActionResult> PutComment(int commentId, [FromBody] CommentCreateRequest request)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found comment with id {commentId}"));
            }
            if (comment.OwnerUserId != User.GetUserId())
            {
                return Forbid();
            }
            comment.Content = request.Content;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot update comment with id {commentId}"));
            }
        }

        [HttpDelete("{knowledgeBaseId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int knowledgeBaseId,int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found comment with id {commentId}"));
            }
            _context.Comments.Remove(comment);
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
            {
                return BadRequest(new ApiBadRequestResponse($"knowledge base with id {knowledgeBaseId} not exist"));
            }
            var numberOfComents = knowledgeBase.NumberOfComments.GetValueOrDefault(0) - 1;
            knowledgeBase.NumberOfComments = numberOfComents;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var commentVM = new CommentViewModel()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreateDate = comment.CreateDate,
                    KnowledgeBaseId = comment.KnowledgeBaseId,
                    LastModifiedDate = comment.LastModifiedDate,
                    OwnerUserId = comment.OwnerUserId,
                    ReplyId = comment.ReplyId
                };
                return Ok(commentVM);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete comment with id {commentId}"));
            }
        }

        [HttpGet("{knowledgeBaseId}/comments/tree")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentTreeByKnowledgeBaseId(int knowledgeBaseId)
        {
            var query = from c in _context.Comments
                        join u in _context.Users
                            on c.OwnerUserId equals u.Id
                        where c.KnowledgeBaseId == knowledgeBaseId
                        select new { c, u };

            var flatComments = await query.Select(x => new CommentViewModel()
            {
                Id = x.c.Id,
                Content = x.c.Content,
                CreateDate = x.c.CreateDate,
                KnowledgeBaseId = x.c.KnowledgeBaseId,
                OwnerUserId = x.c.OwnerUserId,
                OwnerName = x.u.FirstName + " " + x.u.LastName,
                ReplyId = x.c.ReplyId
            }).ToListAsync();

            var lookup = flatComments.ToLookup(c => c.ReplyId);
            var rootCategories = flatComments.Where(x => x.ReplyId == null);

            foreach (var c in rootCategories)//only loop through root categories
            {
                // you can skip the check if you want an empty list instead of null
                // when there is no children
                if (lookup.Contains(c.Id))
                    c.Children = lookup[c.Id].ToList();
            }

            return Ok(rootCategories);
        }
    }
}
