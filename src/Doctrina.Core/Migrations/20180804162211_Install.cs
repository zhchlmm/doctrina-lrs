using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctrina.Core.Migrations
{
    public partial class Install : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    ObjectType = table.Column<int>(maxLength: 6, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Mbox = table.Column<string>(maxLength: 128, nullable: true),
                    Mbox_SHA1SUM = table.Column<string>(maxLength: 40, nullable: true),
                    OpenId = table.Column<string>(maxLength: 2083, nullable: true),
                    OauthIdentifier = table.Column<string>(maxLength: 192, nullable: true),
                    Account_HomePage = table.Column<string>(maxLength: 2083, nullable: true),
                    Account_Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesEntity",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesEntity", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    ETag = table.Column<string>(maxLength: 50, nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Success = table.Column<bool>(nullable: true),
                    Completion = table.Column<bool>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: true),
                    ScoreScaled = table.Column<double>(nullable: true),
                    ScoreRaw = table.Column<double>(nullable: true),
                    ScoreMin = table.Column<double>(nullable: true),
                    ScoreMax = table.Column<double>(nullable: true),
                    Extensions = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verbs",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    CanonicalData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbs", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: false),
                    CanonicalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Activities_Agents_AuthorityId",
                        column: x => x.AuthorityId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false),
                    AgentEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Agents_AgentEntityKey",
                        column: x => x.AgentEntityKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContextEntity",
                columns: table => new
                {
                    ContextId = table.Column<Guid>(nullable: false),
                    Registration = table.Column<Guid>(nullable: true),
                    InstructorId = table.Column<Guid>(nullable: true),
                    TeamId = table.Column<Guid>(nullable: true),
                    Revision = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Extensions = table.Column<string>(nullable: true),
                    StatementId = table.Column<Guid>(nullable: true),
                    ContextActivitiesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextEntity", x => x.ContextId);
                    table.ForeignKey(
                        name: "FK_ContextEntity_ContextActivitiesEntity_ContextActivitiesId",
                        column: x => x.ContextActivitiesId,
                        principalTable: "ContextActivitiesEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContextEntity_Agents_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextEntity_Agents_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AgentProfiles",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<string>(maxLength: 2083, nullable: false),
                    Updated = table.Column<DateTime>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 255, nullable: false),
                    ETag = table.Column<string>(maxLength: 50, nullable: false),
                    AgentId = table.Column<Guid>(nullable: false),
                    DocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentProfiles", x => x.Key);
                    table.ForeignKey(
                        name: "FK_AgentProfiles_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentProfiles_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityProfiles",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<string>(maxLength: 2083, nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    ActivityKey = table.Column<Guid>(maxLength: 2083, nullable: false),
                    RegistrationId = table.Column<Guid>(nullable: true),
                    AgentId = table.Column<Guid>(nullable: false),
                    DocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityProfiles", x => x.Key);
                    table.ForeignKey(
                        name: "FK_ActivityProfiles_Activities_ActivityKey",
                        column: x => x.ActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityProfiles_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityProfiles_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityStates",
                columns: table => new
                {
                    ActivityStateId = table.Column<Guid>(nullable: false),
                    StateId = table.Column<string>(maxLength: 2083, nullable: false),
                    ActivityKey = table.Column<Guid>(maxLength: 2083, nullable: false),
                    RegistrationId = table.Column<Guid>(nullable: true),
                    AgentId = table.Column<Guid>(nullable: false),
                    DocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityStates", x => x.ActivityStateId);
                    table.ForeignKey(
                        name: "FK_ActivityStates_Activities_ActivityKey",
                        column: x => x.ActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityStates_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityStates_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityKey = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesCategory_Activities_ActivityKey",
                        column: x => x.ActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesCategory_ContextActivitiesEntity_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivitiesEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesCategory_ContextEntity_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContextEntity",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesGrouping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityKey = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesGrouping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesGrouping_Activities_ActivityKey",
                        column: x => x.ActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesGrouping_ContextActivitiesEntity_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivitiesEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesGrouping_ContextEntity_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContextEntity",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesOther",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityKey = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesOther", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesOther_Activities_ActivityKey",
                        column: x => x.ActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesOther_ContextActivitiesEntity_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivitiesEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesOther_ContextEntity_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContextEntity",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesParent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ActivityKey = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesParent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesParent_Activities_ActivityKey",
                        column: x => x.ActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesParent_ContextActivitiesEntity_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivitiesEntity",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesParent_ContextEntity_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContextEntity",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    StatementId = table.Column<Guid>(nullable: false),
                    ObjectType = table.Column<int>(maxLength: 12, nullable: false),
                    ObjectAgentKey = table.Column<Guid>(nullable: true),
                    ObjectActivityKey = table.Column<Guid>(nullable: true),
                    ObjectSubStatementId = table.Column<Guid>(nullable: true),
                    ObjectStatementRefId = table.Column<Guid>(nullable: true),
                    ActorKey = table.Column<Guid>(nullable: false),
                    VerbKey = table.Column<Guid>(nullable: false),
                    ResultId = table.Column<Guid>(nullable: true),
                    ContextId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Stored = table.Column<DateTime>(nullable: false),
                    Voided = table.Column<bool>(nullable: false),
                    Version = table.Column<string>(maxLength: 7, nullable: true),
                    User = table.Column<Guid>(nullable: true),
                    AuthorityId = table.Column<Guid>(nullable: true),
                    FullStatement = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_Statements_Agents_ActorKey",
                        column: x => x.ActorKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statements_Agents_AuthorityId",
                        column: x => x.AuthorityId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_ContextEntity_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContextEntity",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Activities_ObjectActivityKey",
                        column: x => x.ObjectActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Agents_ObjectAgentKey",
                        column: x => x.ObjectAgentKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Statements_ObjectStatementRefId",
                        column: x => x.ObjectStatementRefId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_ResultEntity_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ResultEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Verbs_VerbKey",
                        column: x => x.VerbKey,
                        principalTable: "Verbs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CanonicalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    Payload = table.Column<string>(maxLength: 150, nullable: true),
                    StatementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentEntity_Statements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubStatementEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActorKey = table.Column<Guid>(nullable: false),
                    VerbKey = table.Column<Guid>(nullable: false),
                    ObjectType = table.Column<int>(nullable: false),
                    ObjectAgentKey = table.Column<Guid>(nullable: true),
                    ObjectActivityKey = table.Column<Guid>(nullable: true),
                    ObjectStatementRefId = table.Column<Guid>(nullable: true),
                    ResultId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: true),
                    ContextId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubStatementEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_Agents_ActorKey",
                        column: x => x.ActorKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_ContextEntity_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ContextEntity",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_Activities_ObjectActivityKey",
                        column: x => x.ObjectActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_Agents_ObjectAgentKey",
                        column: x => x.ObjectAgentKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_Statements_ObjectStatementRefId",
                        column: x => x.ObjectStatementRefId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_ResultEntity_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ResultEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatementEntity_Verbs_VerbKey",
                        column: x => x.VerbKey,
                        principalTable: "Verbs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AuthorityId",
                table: "Activities",
                column: "AuthorityId",
                unique: true,
                filter: "[AuthorityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfiles_ActivityKey",
                table: "ActivityProfiles",
                column: "ActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfiles_AgentId",
                table: "ActivityProfiles",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfiles_DocumentId",
                table: "ActivityProfiles",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStates_ActivityKey",
                table: "ActivityStates",
                column: "ActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStates_AgentId",
                table: "ActivityStates",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStates_DocumentId",
                table: "ActivityStates",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProfiles_AgentId",
                table: "AgentProfiles",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProfiles_DocumentId",
                table: "AgentProfiles",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Mbox",
                table: "Agents",
                column: "Mbox",
                unique: true,
                filter: "[Mbox] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Mbox_SHA1SUM",
                table: "Agents",
                column: "Mbox_SHA1SUM",
                unique: true,
                filter: "[Mbox_SHA1SUM] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_OpenId",
                table: "Agents",
                column: "OpenId",
                unique: true,
                filter: "[OpenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Account_HomePage_Account_Name",
                table: "Agents",
                columns: new[] { "Account_HomePage", "Account_Name" },
                unique: true,
                filter: "[Account_HomePage] IS NOT NULL AND [Account_Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentEntity_StatementId",
                table: "AttachmentEntity",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesCategory_ActivityKey",
                table: "ContextActivitiesCategory",
                column: "ActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesCategory_ContextActivitiesEntityKey",
                table: "ContextActivitiesCategory",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesCategory_ContextId",
                table: "ContextActivitiesCategory",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesGrouping_ActivityKey",
                table: "ContextActivitiesGrouping",
                column: "ActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesGrouping_ContextActivitiesEntityKey",
                table: "ContextActivitiesGrouping",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesGrouping_ContextId",
                table: "ContextActivitiesGrouping",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesOther_ActivityKey",
                table: "ContextActivitiesOther",
                column: "ActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesOther_ContextActivitiesEntityKey",
                table: "ContextActivitiesOther",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesOther_ContextId",
                table: "ContextActivitiesOther",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesParent_ActivityKey",
                table: "ContextActivitiesParent",
                column: "ActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesParent_ContextActivitiesEntityKey",
                table: "ContextActivitiesParent",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesParent_ContextId",
                table: "ContextActivitiesParent",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextEntity_ContextActivitiesId",
                table: "ContextEntity",
                column: "ContextActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextEntity_InstructorId",
                table: "ContextEntity",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextEntity_TeamId",
                table: "ContextEntity",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_AgentEntityKey",
                table: "GroupMembers",
                column: "AgentEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ActorKey",
                table: "Statements",
                column: "ActorKey");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_AuthorityId",
                table: "Statements",
                column: "AuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ContextId",
                table: "Statements",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ObjectActivityKey",
                table: "Statements",
                column: "ObjectActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ObjectAgentKey",
                table: "Statements",
                column: "ObjectAgentKey");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ObjectStatementRefId",
                table: "Statements",
                column: "ObjectStatementRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ObjectSubStatementId",
                table: "Statements",
                column: "ObjectSubStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ResultId",
                table: "Statements",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_VerbKey",
                table: "Statements",
                column: "VerbKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_ActorKey",
                table: "SubStatementEntity",
                column: "ActorKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_ContextId",
                table: "SubStatementEntity",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_ObjectActivityKey",
                table: "SubStatementEntity",
                column: "ObjectActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_ObjectAgentKey",
                table: "SubStatementEntity",
                column: "ObjectAgentKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_ObjectStatementRefId",
                table: "SubStatementEntity",
                column: "ObjectStatementRefId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_ResultId",
                table: "SubStatementEntity",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatementEntity_VerbKey",
                table: "SubStatementEntity",
                column: "VerbKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_SubStatementEntity_ObjectSubStatementId",
                table: "Statements",
                column: "ObjectSubStatementId",
                principalTable: "SubStatementEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Agents_AuthorityId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ContextEntity_Agents_InstructorId",
                table: "ContextEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ContextEntity_Agents_TeamId",
                table: "ContextEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Agents_ActorKey",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Agents_AuthorityId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Agents_ObjectAgentKey",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_SubStatementEntity_Agents_ActorKey",
                table: "SubStatementEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SubStatementEntity_Agents_ObjectAgentKey",
                table: "SubStatementEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Activities_ObjectActivityKey",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_SubStatementEntity_Activities_ObjectActivityKey",
                table: "SubStatementEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SubStatementEntity_Statements_ObjectStatementRefId",
                table: "SubStatementEntity");

            migrationBuilder.DropTable(
                name: "ActivityProfiles");

            migrationBuilder.DropTable(
                name: "ActivityStates");

            migrationBuilder.DropTable(
                name: "AgentProfiles");

            migrationBuilder.DropTable(
                name: "AttachmentEntity");

            migrationBuilder.DropTable(
                name: "ContextActivitiesCategory");

            migrationBuilder.DropTable(
                name: "ContextActivitiesGrouping");

            migrationBuilder.DropTable(
                name: "ContextActivitiesOther");

            migrationBuilder.DropTable(
                name: "ContextActivitiesParent");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "SubStatementEntity");

            migrationBuilder.DropTable(
                name: "ContextEntity");

            migrationBuilder.DropTable(
                name: "ResultEntity");

            migrationBuilder.DropTable(
                name: "Verbs");

            migrationBuilder.DropTable(
                name: "ContextActivitiesEntity");
        }
    }
}
