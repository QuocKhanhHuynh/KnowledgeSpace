using KnowledgeSpace.BackendServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KnowledgeSpace.BackendServer.Data
{
    public static class Seeding
    {
        public static void Seed(this ModelBuilder builder)
        {
            var userId = Guid.NewGuid().ToString();
            var Hash = new PasswordHasher<User>();

            builder.Entity<Role>().HasData(
                new Role() { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" },
                new Role() { Id = "Member", Name = "Member", NormalizedName = "MEMBER" }
                );

            builder.Entity<User>().HasData(
                new User() {
                    Id = userId,
                    UserName = "admin",
                    FirstName = "Quản trị",
                    LastName = "1",
                    Email = "khanhhuynh912@gmail.com",
                    LockoutEnabled = false,
                    PasswordHash = Hash.HashPassword(null, "Admin@123"),
                    NormalizedUserName = "admin"
                    }
                );

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { UserId = userId, RoleId = "Admin" });

            var FunctionList = new List<Function>()
            {
                new Function { Id = "DASHBOARD", Name = "Bảng điều khiển", ParentId = null, Url = "/dashboard", Icon = "fa-dashboard" },

                new Function { Id = "CONTENT", Name = "Nội dung", ParentId = null, Url = "/contents", Icon = "fa-table" },
                new Function { Id = "CONTENT_CATEGORY", Name = "Danh mục", ParentId = "CONTENT", Url = "/contents/categories",SortOrder = 1 },
                new Function { Id = "CONTENT_KNOWLEDGEBASE", Name = "Bài viết", ParentId = "CONTENT", SortOrder = 2, Url = "/contents/knowledge-bases", Icon = "fa-edit" },
                new Function { Id = "CONTENT_COMMENT", Name = "Bình luận", ParentId = "CONTENT", SortOrder = 3, Url = "/contents/knowledge-bases/comments", Icon = "fa-edit" },
                new Function { Id = "CONTENT_REPORT", Name = "Báo xấu", ParentId = "CONTENT", SortOrder = 4, Url = "/contents/knowledge-bases/reports", Icon = "fa-edit" },

                new Function { Id = "STATISTIC", Name = "Thống kê", ParentId = null, Url = "/statistics", Icon = "fa-bar-chart-o" },
                new Function { Id = "STATISTIC_MONTHLY_NEWMEMBER", Name = "Đăng ký từng tháng", ParentId = "STATISTIC", SortOrder = 1, Url = "/statistics/monthly-registers", Icon = "fa-wrench" },
                new Function { Id = "STATISTIC_MONTHLY_NEWKB", Name = "Bài đăng hàng tháng", ParentId = "STATISTIC", SortOrder = 2, Url = "/statistics/monthly-newkbs", Icon = "fa-wrench" },
                new Function { Id = "STATISTIC_MONTHLY_COMMENT", Name = "Comment theo tháng", ParentId = "STATISTIC", SortOrder = 3, Url = "/statistics/monthly-comments", Icon = "fa-wrench" },

                new Function { Id = "SYSTEM", Name = "Hệ thống", ParentId = null, Url = "/systems", Icon = "fa-th-list" },
                new Function { Id = "SYSTEM_USER", Name = "Người dùng", ParentId = "SYSTEM", Url = "/systems/users", Icon = "fa-desktop", SortOrder = 1 },
                new Function { Id = "SYSTEM_ROLE", Name = "Nhóm quyền", ParentId = "SYSTEM", Url = "/systems/roles", Icon = "fa-desktop" , SortOrder = 2},
                new Function { Id = "SYSTEM_FUNCTION", Name = "Chức năng", ParentId = "SYSTEM", Url = "/systems/functions", Icon = "fa-desktop", SortOrder = 3 },
                new Function { Id = "SYSTEM_PERMISSION", Name = "Quyền hạn", ParentId = "SYSTEM", Url = "/systems/permissions", Icon = "fa-desktop" , SortOrder = 4}
            };
            builder.Entity<Function>().HasData(FunctionList);

            var CommandList = new List<Command>()
            {
                new Command() { Id = "VIEW", Name = "Xem" },
                new Command() { Id = "CREATE", Name = "Thêm" },
                new Command() { Id = "UPDATE", Name = "Sửa" },
                new Command() { Id = "DELETE", Name = "Xoá" },
                new Command() { Id = "APPROVE", Name = "Duyệt" }
            };
            builder.Entity<Command>().HasData(CommandList);

            var CommandInFunctionList = new List<CommandInFunction>();
            foreach(var function in FunctionList)
            {
                foreach(var command in CommandList)
                {
                    if (!command.Id.Equals("APPROVE"))
                    {
                        CommandInFunctionList.Add(new CommandInFunction() { CommandId = command.Id, FunctionId = function.Id });
                    }
                }
            }
            builder.Entity<CommandInFunction>().HasData(CommandInFunctionList);

            var PermissionList = new List<Permission>();
            foreach (var function in FunctionList)
            {
                foreach (var command in CommandList)
                {
                    if (!command.Id.Equals("APPROVE"))
                    {
                        PermissionList.Add(new Permission(function.Id, "Admin", command.Id));
                    }
                }
            }
            builder.Entity<Permission>().HasData(PermissionList);
        }
    }
}
