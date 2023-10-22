using KnowledgeSpace.BackendServer.Controllers;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.Model;
using KnowledgeSpace.Model.Contents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KnowledgeSpace.BackendServer.UnitTest.Controllers
{/*
    public class CategorysControllerTest
    {
        private ApplicationDbcontext _context;
        public CategorysControllerTest()
        {
            _context = InMemoryDbContextFactory.GetApplicationDbContext("CategorysControllerTest");
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            var _categoryController = new CategoriesController(_context);
            Assert.NotNull(_categoryController);
        }

        [Fact]
        public async Task PostCategory_Validate_Success()
        {
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.PostCategory(new CategoryCreateRequest()
            {
                Name = "Test",
                ParentId = null,
                SeoAlias = "/Test",
                SeoDescription = "Test",
                SortOrder = 1
            });
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            _context.Categories.Add(
                new Category()
                {
                    Id = 1,
                    Name = "Test",
                    ParentId = null,
                    SeoAlias = "/Test",
                    SeoDescription = "Test",
                    SortOrder = 1,
                    NumberOfTickets = 1
                }
            );
            await _context.SaveChangesAsync();
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.GetById(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var categoryVM = okResult.Value as CategoryViewModel;
            Assert.True(categoryVM.Id == 1);
        }

        [Fact]
        public async Task GetById_HasData_Fail()
        {
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.GetById(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetFunctions_HasData_Success()
        {
            _context.Categories.AddRange(
                new Category()
                {
                    Id = 1,
                    Name = "Test1",
                    ParentId = null,
                    SeoAlias = "/Test1",
                    SeoDescription = "Test1",
                    SortOrder = 1,
                    NumberOfTickets = 1
                },
                new Category()
                {
                    Id = 2,
                    Name = "Test2",
                    ParentId = null,
                    SeoAlias = "/Test2",
                    SeoDescription = "Test2",
                    SortOrder = 2,
                    NumberOfTickets = 1
                }
            );
            await _context.SaveChangesAsync();
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.GetCategorys();
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var categorys = okResult.Value as List<CategoryViewModel>;
            Assert.True(categorys.Count() > 0);
            ;
        }

        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            _context.Categories.AddRange(
                new Category()
                {
                    Id = 1,
                    Name = "Test1",
                    ParentId = null,
                    SeoAlias = "/Test1",
                    SeoDescription = "Test1",
                    SortOrder = 1,
                    NumberOfTickets = 1
                },
                new Category()
                {
                    Id = 2,
                    Name = "Test2",
                    ParentId = null,
                    SeoAlias = "/Test2",
                    SeoDescription = "Test2",
                    SortOrder = 2,
                    NumberOfTickets = 1
                }
            );
            await _context.SaveChangesAsync();
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.GetCategorysPaging(1, 2, null);
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var categorys = okResult.Value as Pagination<CategoryViewModel>;
            Assert.True(/*functions.TotalRecords == 2 &&*//* categorys.Items.Count() == 2);
        }

        [Fact]
        public async Task GetPaging_HasFilter_Success()
        {
            _context.Categories.AddRange(
                new Category()
                {
                    Id = 1,
                    Name = "Test1",
                    ParentId = null,
                    SeoAlias = "/Test1",
                    SeoDescription = "Test1",
                    SortOrder = 1,
                    NumberOfTickets = 1
                },
                new Category()
                {
                    Id = 2,
                    Name = "Test2",
                    ParentId = null,
                    SeoAlias = "/Test2",
                    SeoDescription = "Test2",
                    SortOrder = 2,
                    NumberOfTickets = 1
                }
            );
            await _context.SaveChangesAsync();
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.GetCategorysPaging(1, 2, "Test1");
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var categorys = okResult.Value as Pagination<CategoryViewModel>;
            Assert.True(categorys.TotalRecords == 1 && categorys.Items.Count() == 1 && categorys.Items.Any(x => x.Name.Contains("Test1")));
        }

        [Fact]
        public async Task PutCategory_Validata_Success()
        {
            _context.Categories.Add(
                new Category()
                {
                    Id = 1,
                    Name = "Test",
                    ParentId = null,
                    SeoAlias = "/Test",
                    SeoDescription = "Test",
                    SortOrder = 1,
                    NumberOfTickets = 1
                }
            );
            await _context.SaveChangesAsync();
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.PutCategory(1, new CategoryCreateRequest()
            {
                Name = "Test1",
                ParentId = null,
                SeoAlias = "Test1",
                SeoDescription = "Test",
                SortOrder = 2
            });
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCategory_Validata_Failled()
        {
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.PutCategory(1, new CategoryCreateRequest()
            {
                Name = "Test1",
                ParentId = null,
                SeoAlias = "Test1",
                SeoDescription = "Test",
                SortOrder = 2
            });
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_Validata_Success()
        {
            _context.Categories.Add(
                new Category()
                {
                    Id = 1,
                    Name = "Test",
                    ParentId = null,
                    SeoAlias = "/Test",
                    SeoDescription = "Test",
                    SortOrder = 1,
                    NumberOfTickets = 1
                }
            );
            await _context.SaveChangesAsync();
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.DeleteCategory(1);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var categoryVM = okResult.Value as CategoryViewModel;
            Assert.True(categoryVM.Id == 1);
        }

        [Fact]
        public async Task DeleteFunction_Validata_Failled()
        {
            var _categoryController = new CategoriesController(_context);
            var result = await _categoryController.DeleteCategory(1);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }*/
}
