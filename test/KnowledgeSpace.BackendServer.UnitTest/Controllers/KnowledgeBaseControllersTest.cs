using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.Model.Systems;
using KnowledgeSpace.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KnowledgeSpace.Model.Contents;
using KnowledgeSpace.BackendServer.Service;
using Moq;
using Microsoft.Extensions.Configuration;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{/*
    public class KnowledgeBaseControllersTest
    {
        private ApplicationDbcontext _context;
        private Mock<ISequenceService> _mockSequence;
        private Mock<IStorageService> _mockStorageService;
        public KnowledgeBaseControllersTest()
        {
            _context = InMemoryDbContextFactory.GetApplicationDbContext("KnowledgeBaseControllersTest");
            _mockSequence= new Mock<ISequenceService>();
            _mockStorageService = new Mock<IStorageService>();

        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            Assert.NotNull(_knowledgeBaseControllers);
        }
        
        [Fact]
        public async Task PostKnowledgeBase_Validate_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.PostKnowledgeBase(new KnowledgeBaseCreateRequest()
            {
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                //Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }
        *//*
        [Fact]
        public async Task GetById_HasData_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.GetById(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var knowledgeBaseVM = okResult.Value as KnowledgeBaseViewModel;
            Assert.True(knowledgeBaseVM.Id == 1);
        }

        [Fact]
        public async Task GetById_HasData_Fail()
        {
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.GetById(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetKnowledgeBases_HasData_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.AddRange(
                new KnowledgeBase()
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "Test1",
                    SeoAlias = "/Test1",
                    OwnerUserId = "UserId",
                    Description = "Test1",
                    Environment = "Test1",
                    ErrorMessage = "Test1",
                    Labels = "Test1",
                    Note = "Test1",
                    Problem = "Test1",
                    StepToReproduce = "Test1",
                    Workaround = "Test1"
                },
                new KnowledgeBase()
                {
                    Id = 2,
                    CategoryId = 1,
                    Title = "Test2",
                    SeoAlias = "/Test2",
                    OwnerUserId = "UserId",
                    Description = "Test2",
                    Environment = "Test2",
                    ErrorMessage = "Test2",
                    Labels = "Test2",
                    Note = "Test2",
                    Problem = "Test2",
                    StepToReproduce = "Test2",
                    Workaround = "Test2"
                }
            );
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = _knowledgeBaseControllers.GetKnowledgeBases();
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var knowledgeBases = okResult.Value as List<KnowledgeBaseQuickViewModel>;
            Assert.True(knowledgeBases.Count() > 0);
            ;
        }
        /*
        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.AddRange(
                new KnowledgeBase()
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "Test1",
                    SeoAlias = "/Test1",
                    OwnerUserId = "UserId",
                    Description = "Test1",
                    Environment = "Test1",
                    ErrorMessage = "Test1",
                    Labels = "Test1",
                    Note = "Test1",
                    Problem = "Test1",
                    StepToReproduce = "Test1",
                    Workaround = "Test1"
                },
                new KnowledgeBase()
                {
                    Id = 2,
                    CategoryId = 1,
                    Title = "Test2",
                    SeoAlias = "/Test2",
                    OwnerUserId = "UserId",
                    Description = "Test2",
                    Environment = "Test2",
                    ErrorMessage = "Test2",
                    Labels = "Test2",
                    Note = "Test2",
                    Problem = "Test2",
                    StepToReproduce = "Test2",
                    Workaround = "Test2"
                }
            );
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.GetKnowledgeBasesPaging(1, 2, null);
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var knowledgeBases = okResult.Value as Pagination<KnowledgeBaseQuickViewModel>;
            Assert.True(/*functions.TotalRecords == 2 &&*//* knowledgeBases.Items.Count() == 2);
        }

        [Fact]
        public async Task GetPaging_HasFilter_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.AddRange(
                new KnowledgeBase()
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "Test1",
                    SeoAlias = "/Test1",
                    OwnerUserId = "UserId",
                    Description = "Test1",
                    Environment = "Test1",
                    ErrorMessage = "Test1",
                    Labels = "Test1",
                    Note = "Test1",
                    Problem = "Test1",
                    StepToReproduce = "Test1",
                    Workaround = "Test1"
                },
                new KnowledgeBase()
                {
                    Id = 2,
                    CategoryId = 1,
                    Title = "Test2",
                    SeoAlias = "/Test2",
                    OwnerUserId = "UserId",
                    Description = "Test2",
                    Environment = "Test2",
                    ErrorMessage = "Test2",
                    Labels = "Test2",
                    Note = "Test2",
                    Problem = "Test2",
                    StepToReproduce = "Test2",
                    Workaround = "Test2"
                }
            );
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = _knowledgeBaseControllers.GetKnowledgeBasesPaging(1, 2, "Test1");
            Assert.NotNull(result);
            var okResult = await result as OkObjectResult;
            var knowledgeBases = okResult.Value as Pagination<KnowledgeBaseQuickViewModel>;
            Assert.True(knowledgeBases.TotalRecords == 1 && knowledgeBases.Items.Count() == 1 && knowledgeBases.Items.Any(x => x.Title.Contains("Test1")));
        }

        [Fact]
        public async Task PutKnowledgeBase_Validata_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Title = "Test",
                SeoAlias = "/Test",
                OwnerUserId = "UserId",
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                Problem = "Test",
                StepToReproduce = "Test",
                Workaround = "Test"
            });
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.PutKnowledgeBase(1, new KnowledgeBaseCreateRequest()
            {
                CategoryId = 1,
                Description = "Test1",
                Environment = "Tes1t",
                ErrorMessage = "Test1",
                //Labels = "Test1",
                Note = "Test1",
                OwnerUserId = "Test1",
                Problem = "Test1",
                SeoAlias = "/Test1",
                StepToReproduce = "Test1",
                Title = "Test1",
                Workaround = "Test1"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutKnowledgeBase_Validata_Failled()
        {
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.PutKnowledgeBase(1, new KnowledgeBaseCreateRequest()
            {
                CategoryId = 1,
                Description = "Test1",
                Environment = "Tes1t",
                ErrorMessage = "Test1",
               // Labels = "Test1",
                Note = "Test1",
                OwnerUserId = "Test1",
                Problem = "Test1",
                SeoAlias = "/Test1",
                StepToReproduce = "Test1",
                Title = "Test1",
                Workaround = "Test1"
            });
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        /*
        [Fact]
        public async Task DeleteKowledgeBase_Validata_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Title = "Test",
                SeoAlias = "/Test",
                OwnerUserId = "UserId",
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                Problem = "Test",
                StepToReproduce = "Test",
                Workaround = "Test"
            });
            await _context.SaveChangesAsync();
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.DeleteKnowledgeBase(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var knowledgeBaseVM = okResult.Value as KnowledgeBaseQuickViewModel;
            Assert.True(knowledgeBaseVM.Id == 1);
        }
        *//*
        [Fact]
        public async Task DeleteKowledgeBase_Validata_Failled()
        {
            var _knowledgeBaseControllers = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _knowledgeBaseControllers.DeleteKnowledgeBase(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAttachments_HasData_Success()
        {
            _context.Attachments.AddRange(
            new Attachment()
            {
                Id = 1,
                FileName = "Test1",
                FilePath = "/Test",
                FileSize = 1024,
                FileType = "Test1",
                KnowledgeBaseId = 1,

            },
            new Attachment()
            {
                Id = 2,
                FileName = "Test2",
                FilePath = "/Tes2",
                FileSize = 1024,
                FileType = "Test2",
                KnowledgeBaseId = 1,
            }
            );
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetAttachments(1);
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var votes = okResult.Value as List<AttachmentViewModel>;
            Assert.True(votes.Count() > 0);
            ;
        }

        [Fact]
        public async Task DeleteAttachment_Validata_Success()
        {
            _context.Attachments.Add(new Attachment()
            {
                Id = 1,
                FileName = "Test1",
                FilePath = "/Test",
                FileSize = 1024,
                FileType = "Test1",
                KnowledgeBaseId = 1,
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteAttachment(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAttachment_Validata_Failled()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteAttachment(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PostVote_Validate_Success()
        {
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PostVote(new VoteCreateRequest()
            {
                KnowledgeBaseId = 1,
                UserId = "user"
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PostVote_Validate_Fail()
        {
            _context.Votes.Add(new Vote()
            {
                KnowledgeBaseId = 1,
                UserId = "user"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PostVote(new VoteCreateRequest()
            {
                KnowledgeBaseId = 1,
                UserId = "user"
            });
            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetVotes_HasData_Success()
        {
            _context.Votes.AddRange(
            new Vote()
            {
                KnowledgeBaseId = 1,
                UserId = "user"
            },
            new Vote()
            {
                KnowledgeBaseId = 2,
                UserId = "user"
            }
            );
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetVotes();
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var votes = okResult.Value as List<VoteViewModel>;
            Assert.True(votes.Count() > 0);
            ;
        }

        [Fact]
        public async Task DeleteVote_Validata_Success()
        {
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            _context.Votes.Add(new Vote()
            {
                KnowledgeBaseId = 1,
                UserId = "user"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteVote(1, "user");
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVote_Validata_Failled()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteVote(1, "user");
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PostReport_Validate_Success()
        {
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PostReport(new ReportCreateRequest()
            {
                KnowledgeBaseId = 1,
                Content = "Test",
                ReportUserId = "user"
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task GetReportDetail_HasData_Success()
        {
            _context.Reports.Add(new Report()
            {
                Id = 1,
                KnowledgeBaseId = 1,
                Content = "Test",
                ReportUserId = "user"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetReportDetail(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var reportVM = okResult.Value as ReportViewModel;
            Assert.Equal(1, reportVM.Id);
        }

        [Fact]
        public async Task GetRepoerttDetail_HasData_Fail()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetReportDetail(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPaging_Report_NoFilter_Success()
        {
            _context.Reports.AddRange(
            new Report()
            {
                Id = 1,
                KnowledgeBaseId = 1,
                Content = "Test1",
                ReportUserId = "user2"
            },
            new Report()
            {
                Id = 2,
                KnowledgeBaseId = 1,
                Content = "Test2",
                ReportUserId = "user1"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetReportsPaging(1, 1, 2, null);
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var comments = okResult.Value as Pagination<ReportViewModel>;
            Assert.True(/*functions.TotalRecords == 2 &&*//* comments.Items.Count() == 2);
        }

        [Fact]
        public async Task GetPaging_Report_HasFilter_Success()
        {
            _context.Reports.AddRange(
            new Report()
            {
                Id = 1,
                KnowledgeBaseId = 1,
                Content = "Test1",
                ReportUserId = "user2"
            },
            new Report()
            {
                Id = 2,
                KnowledgeBaseId = 1,
                Content = "Test2",
                ReportUserId = "user1"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetReportsPaging(1, 1, 2, "Test1");
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var comments = okResult.Value as Pagination<ReportViewModel>;
            Assert.True(comments.Items.Count() == 1);
        }

        [Fact]
        public async Task PutReport_Validata_Success()
        {
            _context.Reports.Add(new Report()
            {
                Id = 1,
                KnowledgeBaseId = 1,
                Content = "Test",
                ReportUserId = "user"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PutReport(1, new ReportCreateRequest()
            {
                Content = "Test1",
                ReportUserId = "user",
                KnowledgeBaseId = 1
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutReport_Validata_Fail()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PutReport(1, new ReportCreateRequest()
            {
                Content = "Test1",
                ReportUserId = "user",
                KnowledgeBaseId = 1
            });
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteReport_Validata_Success()
        {
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            _context.Reports.Add(new Report()
            {
                Id = 1,
                KnowledgeBaseId = 1,
                Content = "Test",
                ReportUserId = "user"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteReport(1, 1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var reportVM = okResult.Value as ReportViewModel;
            Assert.True(reportVM.Id == 1);
        }

        [Fact]
        public async Task DeleteReport_Validata_Failled()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteReport(1, 1);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        [Fact]
        public async Task PostComment_Validate_Success()
        {
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PostComment(new CommentCreateRequest()
            {
                Content = "Test",
                KnowledgeBaseId = 1,
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task GetCommentDetail_HasData_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            _context.Comments.Add(new Comment()
            {
                Id = 1,
                Content = "Test",
                KnowledgeBaseId = 1,
                OwnerUserId = "Test"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetCommentDetail(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var commentVM = okResult.Value as CommentViewModel;
            Assert.Equal(1, commentVM.Id);
        }

        [Fact]
        public async Task GetCommentDetail_HasData_Fail()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetCommentDetail(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPaging_Comment_NoFilter_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            _context.Comments.AddRange(
            new Comment()
            {
                Id = 1,
                Content = "Test1",
                KnowledgeBaseId = 1,
                OwnerUserId = "Test1"
            },
            new Comment()
            {
                Id = 2,
                Content = "Test2",
                KnowledgeBaseId = 1,
                OwnerUserId = "Test2"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetCommentsPaging(1, 1, 2, null);
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var comments = okResult.Value as Pagination<CommentViewModel>;
            Assert.True(/*functions.TotalRecords == 2 &&*//* comments.Items.Count() == 2);
        }

        [Fact]
        public async Task GetPaging_Comment_HasFilter_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test"
            });
            _context.Comments.AddRange(
            new Comment()
            {
                Id = 1,
                Content = "Test1",
                KnowledgeBaseId = 1,
                OwnerUserId = "Test1"
            },
            new Comment()
            {
                Id = 2,
                Content = "Test2",
                KnowledgeBaseId = 1,
                OwnerUserId = "Test2",
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.GetCommentsPaging(1, 1, 2, "Test1");
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var comments = okResult.Value as Pagination<CommentViewModel>;
            Assert.True(comments.Items.Count() == 1);
        }

        [Fact]
        public async Task PutComment_Validata_Failled()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.PutComment(1, new CommentCreateRequest()
            {
                Content = "Test1",
                KnowledgeBaseId = 1
            });
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteComment_Validata_Success()
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Category",
                NumberOfTickets = 1,
                ParentId = 0,
                SeoAlias = "Category",
                SeoDescription = "Category",
                SortOrder = 1,
            });
            _context.KnowledgeBases.Add(new KnowledgeBase()
            {
                Id = 1,
                CategoryId = 1,
                Description = "Test",
                Environment = "Test",
                ErrorMessage = "Test",
                Labels = "Test",
                Note = "Test",
                OwnerUserId = "Test",
                Problem = "Test",
                SeoAlias = "/Test",
                StepToReproduce = "Test",
                Title = "Test",
                Workaround = "Test",

            });
            _context.Comments.Add(new Comment()
            {
                Id = 1,
                Content = "Test",
                KnowledgeBaseId = 1,
                OwnerUserId = "Test"
            });
            await _context.SaveChangesAsync();
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteComment(1, 1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var commentVM = okResult.Value as CommentViewModel;
            Assert.True(commentVM.Id == 1);
        }

        [Fact]
        public async Task DeleteComment_Validata_Failled()
        {
            var _controller = new KnowledgeBasesController(_context, _mockSequence.Object, _mockStorageService.Object);
            var result = await _controller.DeleteComment(1, 1);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }*/
}