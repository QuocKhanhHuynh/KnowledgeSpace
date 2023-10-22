using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KnowledgeSpace.BackendServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "KnowledgeBaseSequence");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfKnowledgeBases = table.Column<int>(type: "int", nullable: true),
                    NumberOfVotes = table.Column<int>(type: "int", nullable: true),
                    NumberOfReports = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SeoAlias = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    SeoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    NumberOfTickets = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    EntityName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(50)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SeoAlias = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Environment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Problem = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StepToReproduce = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Workaround = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerUserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Labels = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfComments = table.Column<int>(type: "int", nullable: true),
                    NumberOfVotes = table.Column<int>(type: "int", nullable: true),
                    NumberOfReports = table.Column<int>(type: "int", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeBases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeBases_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandInFunctions",
                columns: table => new
                {
                    CommandId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FunctionId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandInFunctions", x => new { x.CommandId, x.FunctionId });
                    table.ForeignKey(
                        name: "FK_CommandInFunctions_Commands_CommandId",
                        column: x => x.CommandId,
                        principalTable: "Commands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommandInFunctions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    FunctionId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CommandId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => new { x.FunctionId, x.CommandId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Permissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Commands_CommandId",
                        column: x => x.CommandId,
                        principalTable: "Commands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileType = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    KnowledgeBaseId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_KnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "KnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KnowledgeBaseId = table.Column<int>(type: "int", nullable: false),
                    OwnerUserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_KnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "KnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabelInKnowledgeBases",
                columns: table => new
                {
                    KnowledgeBaseId = table.Column<int>(type: "int", nullable: false),
                    LabelId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelInKnowledgeBases", x => new { x.KnowledgeBaseId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_LabelInKnowledgeBases_KnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "KnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelInKnowledgeBases_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnowledgeBaseId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ReportUserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_KnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "KnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    KnowledgeBaseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => new { x.KnowledgeBaseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Votes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_KnowledgeBases_KnowledgeBaseId",
                        column: x => x.KnowledgeBaseId,
                        principalTable: "KnowledgeBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "Admin", null, "Admin", "ADMIN" },
                    { "Member", null, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreateDate", "Dob", "Email", "EmailConfirmed", "FirstName", "LastModifiedDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "NumberOfKnowledgeBases", "NumberOfReports", "NumberOfVotes", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "84a9475e-2c1e-4a36-b4a2-1b93920944e2", 0, "3bb7c77c-2517-44c8-b57c-ec7e830141b0", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khanhhuynh912@gmail.com", false, "Quản trị", null, "1", false, null, null, "admin", null, null, null, "AQAAAAIAAYagAAAAELqBlIdhae4Rvx/drbTHf3n0TkUsp+4WwGmIvxN4bumy4kWinXlwDb0ACX6/Yw1r5g==", null, false, "daf2bc8b-4a1f-4843-87c8-61b1497e75d2", false, "admin" });

            migrationBuilder.InsertData(
                table: "Commands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "APPROVE", "Duyệt" },
                    { "CREATE", "Thêm" },
                    { "DELETE", "Xoá" },
                    { "UPDATE", "Sửa" },
                    { "VIEW", "Xem" }
                });

            migrationBuilder.InsertData(
                table: "Functions",
                columns: new[] { "Id", "Icon", "Name", "ParentId", "SortOrder", "Url" },
                values: new object[,]
                {
                    { "CONTENT", "fa-table", "Nội dung", null, 0, "/contents" },
                    { "CONTENT_CATEGORY", null, "Danh mục", "CONTENT", 1, "/contents/categories" },
                    { "CONTENT_COMMENT", "fa-edit", "Bình luận", "CONTENT", 3, "/contents/knowledge-bases/comments" },
                    { "CONTENT_KNOWLEDGEBASE", "fa-edit", "Bài viết", "CONTENT", 2, "/contents/knowledge-bases" },
                    { "CONTENT_REPORT", "fa-edit", "Báo xấu", "CONTENT", 4, "/contents/knowledge-bases/reports" },
                    { "DASHBOARD", "fa-dashboard", "Bảng điều khiển", null, 0, "/dashboard" },
                    { "STATISTIC", "fa-bar-chart-o", "Thống kê", null, 0, "/statistics" },
                    { "STATISTIC_MONTHLY_COMMENT", "fa-wrench", "Comment theo tháng", "STATISTIC", 3, "/statistics/monthly-comments" },
                    { "STATISTIC_MONTHLY_NEWKB", "fa-wrench", "Bài đăng hàng tháng", "STATISTIC", 2, "/statistics/monthly-newkbs" },
                    { "STATISTIC_MONTHLY_NEWMEMBER", "fa-wrench", "Đăng ký từng tháng", "STATISTIC", 1, "/statistics/monthly-registers" },
                    { "SYSTEM", "fa-th-list", "Hệ thống", null, 0, "/systems" },
                    { "SYSTEM_FUNCTION", "fa-desktop", "Chức năng", "SYSTEM", 3, "/systems/functions" },
                    { "SYSTEM_PERMISSION", "fa-desktop", "Quyền hạn", "SYSTEM", 4, "/systems/permissions" },
                    { "SYSTEM_ROLE", "fa-desktop", "Nhóm quyền", "SYSTEM", 2, "/systems/roles" },
                    { "SYSTEM_USER", "fa-desktop", "Người dùng", "SYSTEM", 1, "/systems/users" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "Admin", "84a9475e-2c1e-4a36-b4a2-1b93920944e2" });

            migrationBuilder.InsertData(
                table: "CommandInFunctions",
                columns: new[] { "CommandId", "FunctionId" },
                values: new object[,]
                {
                    { "CREATE", "CONTENT" },
                    { "CREATE", "CONTENT_CATEGORY" },
                    { "CREATE", "CONTENT_COMMENT" },
                    { "CREATE", "CONTENT_KNOWLEDGEBASE" },
                    { "CREATE", "CONTENT_REPORT" },
                    { "CREATE", "DASHBOARD" },
                    { "CREATE", "STATISTIC" },
                    { "CREATE", "STATISTIC_MONTHLY_COMMENT" },
                    { "CREATE", "STATISTIC_MONTHLY_NEWKB" },
                    { "CREATE", "STATISTIC_MONTHLY_NEWMEMBER" },
                    { "CREATE", "SYSTEM" },
                    { "CREATE", "SYSTEM_FUNCTION" },
                    { "CREATE", "SYSTEM_PERMISSION" },
                    { "CREATE", "SYSTEM_ROLE" },
                    { "CREATE", "SYSTEM_USER" },
                    { "DELETE", "CONTENT" },
                    { "DELETE", "CONTENT_CATEGORY" },
                    { "DELETE", "CONTENT_COMMENT" },
                    { "DELETE", "CONTENT_KNOWLEDGEBASE" },
                    { "DELETE", "CONTENT_REPORT" },
                    { "DELETE", "DASHBOARD" },
                    { "DELETE", "STATISTIC" },
                    { "DELETE", "STATISTIC_MONTHLY_COMMENT" },
                    { "DELETE", "STATISTIC_MONTHLY_NEWKB" },
                    { "DELETE", "STATISTIC_MONTHLY_NEWMEMBER" },
                    { "DELETE", "SYSTEM" },
                    { "DELETE", "SYSTEM_FUNCTION" },
                    { "DELETE", "SYSTEM_PERMISSION" },
                    { "DELETE", "SYSTEM_ROLE" },
                    { "DELETE", "SYSTEM_USER" },
                    { "UPDATE", "CONTENT" },
                    { "UPDATE", "CONTENT_CATEGORY" },
                    { "UPDATE", "CONTENT_COMMENT" },
                    { "UPDATE", "CONTENT_KNOWLEDGEBASE" },
                    { "UPDATE", "CONTENT_REPORT" },
                    { "UPDATE", "DASHBOARD" },
                    { "UPDATE", "STATISTIC" },
                    { "UPDATE", "STATISTIC_MONTHLY_COMMENT" },
                    { "UPDATE", "STATISTIC_MONTHLY_NEWKB" },
                    { "UPDATE", "STATISTIC_MONTHLY_NEWMEMBER" },
                    { "UPDATE", "SYSTEM" },
                    { "UPDATE", "SYSTEM_FUNCTION" },
                    { "UPDATE", "SYSTEM_PERMISSION" },
                    { "UPDATE", "SYSTEM_ROLE" },
                    { "UPDATE", "SYSTEM_USER" },
                    { "VIEW", "CONTENT" },
                    { "VIEW", "CONTENT_CATEGORY" },
                    { "VIEW", "CONTENT_COMMENT" },
                    { "VIEW", "CONTENT_KNOWLEDGEBASE" },
                    { "VIEW", "CONTENT_REPORT" },
                    { "VIEW", "DASHBOARD" },
                    { "VIEW", "STATISTIC" },
                    { "VIEW", "STATISTIC_MONTHLY_COMMENT" },
                    { "VIEW", "STATISTIC_MONTHLY_NEWKB" },
                    { "VIEW", "STATISTIC_MONTHLY_NEWMEMBER" },
                    { "VIEW", "SYSTEM" },
                    { "VIEW", "SYSTEM_FUNCTION" },
                    { "VIEW", "SYSTEM_PERMISSION" },
                    { "VIEW", "SYSTEM_ROLE" },
                    { "VIEW", "SYSTEM_USER" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "CommandId", "FunctionId", "RoleId" },
                values: new object[,]
                {
                    { "CREATE", "CONTENT", "Admin" },
                    { "DELETE", "CONTENT", "Admin" },
                    { "UPDATE", "CONTENT", "Admin" },
                    { "VIEW", "CONTENT", "Admin" },
                    { "CREATE", "CONTENT_CATEGORY", "Admin" },
                    { "DELETE", "CONTENT_CATEGORY", "Admin" },
                    { "UPDATE", "CONTENT_CATEGORY", "Admin" },
                    { "VIEW", "CONTENT_CATEGORY", "Admin" },
                    { "CREATE", "CONTENT_COMMENT", "Admin" },
                    { "DELETE", "CONTENT_COMMENT", "Admin" },
                    { "UPDATE", "CONTENT_COMMENT", "Admin" },
                    { "VIEW", "CONTENT_COMMENT", "Admin" },
                    { "CREATE", "CONTENT_KNOWLEDGEBASE", "Admin" },
                    { "DELETE", "CONTENT_KNOWLEDGEBASE", "Admin" },
                    { "UPDATE", "CONTENT_KNOWLEDGEBASE", "Admin" },
                    { "VIEW", "CONTENT_KNOWLEDGEBASE", "Admin" },
                    { "CREATE", "CONTENT_REPORT", "Admin" },
                    { "DELETE", "CONTENT_REPORT", "Admin" },
                    { "UPDATE", "CONTENT_REPORT", "Admin" },
                    { "VIEW", "CONTENT_REPORT", "Admin" },
                    { "CREATE", "DASHBOARD", "Admin" },
                    { "DELETE", "DASHBOARD", "Admin" },
                    { "UPDATE", "DASHBOARD", "Admin" },
                    { "VIEW", "DASHBOARD", "Admin" },
                    { "CREATE", "STATISTIC", "Admin" },
                    { "DELETE", "STATISTIC", "Admin" },
                    { "UPDATE", "STATISTIC", "Admin" },
                    { "VIEW", "STATISTIC", "Admin" },
                    { "CREATE", "STATISTIC_MONTHLY_COMMENT", "Admin" },
                    { "DELETE", "STATISTIC_MONTHLY_COMMENT", "Admin" },
                    { "UPDATE", "STATISTIC_MONTHLY_COMMENT", "Admin" },
                    { "VIEW", "STATISTIC_MONTHLY_COMMENT", "Admin" },
                    { "CREATE", "STATISTIC_MONTHLY_NEWKB", "Admin" },
                    { "DELETE", "STATISTIC_MONTHLY_NEWKB", "Admin" },
                    { "UPDATE", "STATISTIC_MONTHLY_NEWKB", "Admin" },
                    { "VIEW", "STATISTIC_MONTHLY_NEWKB", "Admin" },
                    { "CREATE", "STATISTIC_MONTHLY_NEWMEMBER", "Admin" },
                    { "DELETE", "STATISTIC_MONTHLY_NEWMEMBER", "Admin" },
                    { "UPDATE", "STATISTIC_MONTHLY_NEWMEMBER", "Admin" },
                    { "VIEW", "STATISTIC_MONTHLY_NEWMEMBER", "Admin" },
                    { "CREATE", "SYSTEM", "Admin" },
                    { "DELETE", "SYSTEM", "Admin" },
                    { "UPDATE", "SYSTEM", "Admin" },
                    { "VIEW", "SYSTEM", "Admin" },
                    { "CREATE", "SYSTEM_FUNCTION", "Admin" },
                    { "DELETE", "SYSTEM_FUNCTION", "Admin" },
                    { "UPDATE", "SYSTEM_FUNCTION", "Admin" },
                    { "VIEW", "SYSTEM_FUNCTION", "Admin" },
                    { "CREATE", "SYSTEM_PERMISSION", "Admin" },
                    { "DELETE", "SYSTEM_PERMISSION", "Admin" },
                    { "UPDATE", "SYSTEM_PERMISSION", "Admin" },
                    { "VIEW", "SYSTEM_PERMISSION", "Admin" },
                    { "CREATE", "SYSTEM_ROLE", "Admin" },
                    { "DELETE", "SYSTEM_ROLE", "Admin" },
                    { "UPDATE", "SYSTEM_ROLE", "Admin" },
                    { "VIEW", "SYSTEM_ROLE", "Admin" },
                    { "CREATE", "SYSTEM_USER", "Admin" },
                    { "DELETE", "SYSTEM_USER", "Admin" },
                    { "UPDATE", "SYSTEM_USER", "Admin" },
                    { "VIEW", "SYSTEM_USER", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_KnowledgeBaseId",
                table: "Attachments",
                column: "KnowledgeBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandInFunctions_FunctionId",
                table: "CommandInFunctions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_KnowledgeBaseId",
                table: "Comments",
                column: "KnowledgeBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeBases_CategoryId",
                table: "KnowledgeBases",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelInKnowledgeBases_LabelId",
                table: "LabelInKnowledgeBases",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CommandId",
                table: "Permissions",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_KnowledgeBaseId",
                table: "Reports",
                column: "KnowledgeBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CommandInFunctions");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "LabelInKnowledgeBases");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "KnowledgeBases");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropSequence(
                name: "KnowledgeBaseSequence");
        }
    }
}
