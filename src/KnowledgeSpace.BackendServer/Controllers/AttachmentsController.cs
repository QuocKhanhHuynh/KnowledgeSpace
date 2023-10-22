﻿using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Helpers;
using KnowledgeSpace.Model.Contents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public partial class KnowledgeBasesController
    {
        [HttpGet("{knowledgeBaseId}/attachments")]
        public async Task<IActionResult> GetAttachments(int knowledgeBaseId)
        {
            var attachments = await _context.Attachments
                .Where(x => x.KnowledgeBaseId == knowledgeBaseId)
                .Select(c => new AttachmentViewModel()
                {
                    Id = c.Id,
                    LastModifiedDate = c.LastModifiedDate,
                    CreateDate = c.CreateDate,
                    FileName = c.FileName,
                    FilePath = c.FilePath,
                    FileSize = c.FileSize,
                    FileType = c.FileType,
                    KnowledgeBaseId = c.KnowledgeBaseId
                }).ToListAsync();

            return Ok(attachments);
        }

        [HttpDelete("{knowledgeBaseId}/attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(int attachmentId)
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            if (attachment == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found attachment with id {attachmentId}"));

            _context.Attachments.Remove(attachment);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse($"Cannot delete attachment with id {attachmentId}"));
        }
    }
}