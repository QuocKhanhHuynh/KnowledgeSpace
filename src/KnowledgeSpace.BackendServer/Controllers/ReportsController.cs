using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KnowledgeSpace.BackendServer.Authorization;
using KnowledgeSpace.BackendServer.Constants;
using KnowledgeSpace.BackendServer.Helpers;
using System.Net.Mail;
using KnowledgeSpace.BackendServer.Extensions;

namespace KnowledgeSpace.BackendServer.Controllers
{
    public partial class KnowledgeBasesController
    {
        [HttpGet("{knowledgeBaseId}/reports/filter")]
        public async Task<IActionResult> GetReportsPaging(int? knowledgeBaseId, int pageIndex, int pageSize, string filter = null)
        {
            var query = from r in _context.Reports
                        join u in _context.Users
                            on r.ReportUserId equals u.Id
                        select new { r, u };
            if (knowledgeBaseId.HasValue)
            {
                query = query.Where(x => x.r.KnowledgeBaseId == knowledgeBaseId.Value);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.r.Content.Contains(filter));
            }
            var totals = query.Count();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new ReportViewModel()
            {
               Id = x.r.Id,
               Content = x.r.Content,
               CreateDate = x.r.CreateDate,
               IsProcessed = x.r.IsProcessed,
               ReportUserId = x.r.ReportUserId,
               KnowledgeBaseId = x.r.KnowledgeBaseId,
               LastModifiedDate = x.r.LastModifiedDate,
                ReportUserName = x.u.FirstName + " " + x.u.LastName
            }).ToListAsync();
            var pagination = new Pagination<ReportViewModel>()
            {
                Items = items,
                TotalRecords = totals
            };
            return Ok(pagination);
        }

        [HttpGet("{knowledgeBaseId}/reports/{reportId}")]
        public async Task<IActionResult> GetReportDetail(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found report base with id {reportId}"));
            }
            var user = await _context.Users.FindAsync(report.ReportUserId);
            var reportVM = new ReportViewModel()
            {
                Id = report.Id,
                Content = report.Content,
                CreateDate = report.CreateDate,
                IsProcessed = report.IsProcessed,
                ReportUserId = report.ReportUserId,
                KnowledgeBaseId = report.KnowledgeBaseId,
                LastModifiedDate = report.LastModifiedDate,
                ReportUserName = user.FirstName + " " + user.LastName
            };
            return Ok(reportVM);
        }

        [HttpPost("{knowledgeBaseId}/reports")]
        [ApiValidatorFilter]
        public async Task<IActionResult> PostReport([FromBody] ReportCreateRequest request)
        {
            var report = new Report()
            {
                KnowledgeBaseId = request.KnowledgeBaseId,
                Content = request.Content,
                ReportUserId = User.GetUserId()
            };
            _context.Reports.Add(report);
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(request.KnowledgeBaseId);
            if (knowledgeBase == null)
            {
                return BadRequest(new ApiBadRequestResponse($"knowledge base with id {request.KnowledgeBaseId} not exist"));
            }
            var numberOfReports = knowledgeBase.NumberOfReports.GetValueOrDefault(0) + 1;
            knowledgeBase.NumberOfReports = numberOfReports;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot add report"));
            }
        }

        [HttpDelete("{knowledgeBaseId}/comments/{reportId}")]
        public async Task<IActionResult> DeleteReport(int knowledgeBaseId, int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report == null)
            {
                return NotFound(new ApiNotFoundResponse($"Cannot found report base with id {reportId}"));
            }
            _context.Reports.Remove(report);
            var knowledgeBase = await _context.KnowledgeBases.FindAsync(knowledgeBaseId);
            if (knowledgeBase == null)
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete report with id {reportId}"));
            }
            var numberOfReports = knowledgeBase.NumberOfReports.GetValueOrDefault(0) - 1;
            knowledgeBase.NumberOfReports = numberOfReports;
            _context.KnowledgeBases.Update(knowledgeBase);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var reportVM = new ReportViewModel()
                {
                    Id = report.Id,
                    Content = report.Content,
                    CreateDate = report.CreateDate,
                    IsProcessed = report.IsProcessed,
                    ReportUserId = report.ReportUserId,
                    KnowledgeBaseId = report.KnowledgeBaseId,
                    LastModifiedDate = report.LastModifiedDate
                };
                return Ok(reportVM);
            }
            else
            {
                return BadRequest(new ApiBadRequestResponse($"Cannot delete report with id {reportId}"));
            }
        }
    }
}
