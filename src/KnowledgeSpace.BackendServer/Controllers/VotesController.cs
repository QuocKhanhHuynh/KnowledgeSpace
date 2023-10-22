using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using KnowledgeSpace.BackendServer.Data;
using Azure.Core;
using KnowledgeSpace.BackendServer.Helpers;
using System.Net.Mail;
using KnowledgeSpace.BackendServer.Extensions;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public partial class KnowledgeBasesController
    {

        [HttpPost("{knowledgeBaseId}/Votes")]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostVote(int knowledgeBaseId, [FromBody] VoteCreateRequest request)
        {
            var userId = User.GetUserId();
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
                return BadRequest(new ApiBadRequestResponse($"Cannot found knowledge base with id {knowledgeBaseId}"));

            var numberOfVotes = await _context.Votes.CountAsync(x => x.KnowledgeBaseId == knowledgeBaseId);
            var vote = await _context.Votes.FindAsync(knowledgeBaseId, userId);
            if (vote != null)
            {
                _context.Votes.Remove(vote);
                numberOfVotes -= 1;
            }
            else
            {
                vote = new Vote()
                {
                    KnowledgeBaseId = knowledgeBaseId,
                    UserId = userId
                };
                _context.Votes.Add(vote);
                numberOfVotes += 1;
            }

            knowledgeBase.NumberOfVotes = numberOfVotes;
            _context.KnowledgeBases.Update(knowledgeBase);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(numberOfVotes);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add vote with knowledge base id {request.KnowledgeBaseId} and user id {userId}"));
            }
        }

        [HttpGet("{knowledgeBaseId}/Votes")]
        public async Task<IActionResult> GetVotes()
        {
            var votes = _context.Votes;
            var voteVMs = await votes.Select(x => new VoteViewModel()
            {
                KnowledgeBaseId = x.KnowledgeBaseId,
                UserId = x.UserId,
                CreateDate = x.CreateDate,
                LastModifiedDate = x.LastModifiedDate 
            }).ToListAsync();
            return Ok(voteVMs);
        }

        [HttpDelete("{knowledgeBaseId}/votes/{userId}")]
        public async Task<IActionResult> DeleteVote(int knowledgeBaseId, string userId)
        {
            var vote = await _context.Votes.FindAsync(knowledgeBaseId, userId);
            if (vote == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found user base with id {userId}"));
            }
            _context.Votes.Remove(vote);
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
            {
                return BadRequest(new ApiBadRequestResponse($"knowledge base with id {knowledgeBaseId} not exist"));
            }
            var numberOfVotes = knowledgeBase.NumberOfVotes.GetValueOrDefault(0) - 1;
            knowledgeBase.NumberOfVotes = numberOfVotes;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add delete vote with knowledge base id {knowledgeBaseId} and user id {userId}"));
            }
        }
    }
}
