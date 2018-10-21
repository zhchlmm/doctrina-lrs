using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctrina.Core.Migrations
{
    public partial class CreateTables : Migration
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
                name: "ContextActivities",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivities", x => x.Key);
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
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Success = table.Column<bool>(nullable: true),
                    Completion = table.Column<bool>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    Duration = table.Column<long>(nullable: true),
                    ScoreScaled = table.Column<double>(nullable: true),
                    ScoreRaw = table.Column<double>(nullable: true),
                    ScoreMin = table.Column<double>(nullable: true),
                    ScoreMax = table.Column<double>(nullable: true),
                    Extensions = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
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
                name: "ContextActivitiesCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesCategory_ContextActivities_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesGrouping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesGrouping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesGrouping_ContextActivities_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesOther",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesOther", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesOther_ContextActivities_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivitiesParent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContextId = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ContextActivitiesEntityKey = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivitiesParent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivitiesParent_ContextActivities_ContextActivitiesEntityKey",
                        column: x => x.ContextActivitiesEntityKey,
                        principalTable: "ContextActivities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatementContexts",
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
                    ContextActivitiesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementContexts", x => x.ContextId);
                    table.ForeignKey(
                        name: "FK_StatementContexts_ContextActivities_ContextActivitiesId",
                        column: x => x.ContextActivitiesId,
                        principalTable: "ContextActivities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatementContexts_Agents_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatementContexts_Agents_TeamId",
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
                name: "SubStatements",
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
                    table.PrimaryKey("PK_SubStatements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubStatements_Agents_ActorKey",
                        column: x => x.ActorKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubStatements_StatementContexts_ContextId",
                        column: x => x.ContextId,
                        principalTable: "StatementContexts",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_Activities_ObjectActivityKey",
                        column: x => x.ObjectActivityKey,
                        principalTable: "Activities",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_Agents_ObjectAgentKey",
                        column: x => x.ObjectAgentKey,
                        principalTable: "Agents",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_Verbs_VerbKey",
                        column: x => x.VerbKey,
                        principalTable: "Verbs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    StatementId = table.Column<Guid>(nullable: false),
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
                        name: "FK_Statements_StatementContexts_ContextId",
                        column: x => x.ContextId,
                        principalTable: "StatementContexts",
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
                        name: "FK_Statements_SubStatements_ObjectSubStatementId",
                        column: x => x.ObjectSubStatementId,
                        principalTable: "SubStatements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
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
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CanonicalData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    Payload = table.Column<string>(maxLength: 150, nullable: true),
                    StatementId = table.Column<Guid>(nullable: false),
                    SHA = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Statements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ActivityId",
                table: "Activities",
                column: "ActivityId");

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
                name: "IX_Agents_ObjectType_Mbox",
                table: "Agents",
                columns: new[] { "ObjectType", "Mbox" },
                unique: true,
                filter: "[Mbox] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_ObjectType_Mbox_SHA1SUM",
                table: "Agents",
                columns: new[] { "ObjectType", "Mbox_SHA1SUM" },
                unique: true,
                filter: "[Mbox_SHA1SUM] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_ObjectType_OpenId",
                table: "Agents",
                columns: new[] { "ObjectType", "OpenId" },
                unique: true,
                filter: "[OpenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_ObjectType_Account_HomePage_Account_Name",
                table: "Agents",
                columns: new[] { "ObjectType", "Account_HomePage", "Account_Name" },
                unique: true,
                filter: "[Account_HomePage] IS NOT NULL AND [Account_Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_StatementId",
                table: "Attachments",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesCategory_ContextActivitiesEntityKey",
                table: "ContextActivitiesCategory",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesGrouping_ContextActivitiesEntityKey",
                table: "ContextActivitiesGrouping",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesOther_ContextActivitiesEntityKey",
                table: "ContextActivitiesOther",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivitiesParent_ContextActivitiesEntityKey",
                table: "ContextActivitiesParent",
                column: "ContextActivitiesEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_AgentEntityKey",
                table: "GroupMembers",
                column: "AgentEntityKey");

            migrationBuilder.CreateIndex(
                name: "IX_StatementContexts_ContextActivitiesId",
                table: "StatementContexts",
                column: "ContextActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_StatementContexts_InstructorId",
                table: "StatementContexts",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_StatementContexts_TeamId",
                table: "StatementContexts",
                column: "TeamId");

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
                name: "IX_SubStatements_ActorKey",
                table: "SubStatements",
                column: "ActorKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ContextId",
                table: "SubStatements",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ObjectActivityKey",
                table: "SubStatements",
                column: "ObjectActivityKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ObjectAgentKey",
                table: "SubStatements",
                column: "ObjectAgentKey");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ResultId",
                table: "SubStatements",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_VerbKey",
                table: "SubStatements",
                column: "VerbKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityProfiles");

            migrationBuilder.DropTable(
                name: "ActivityStates");

            migrationBuilder.DropTable(
                name: "AgentProfiles");

            migrationBuilder.DropTable(
                name: "Attachments");

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
                name: "Statements");

            migrationBuilder.DropTable(
                name: "SubStatements");

            migrationBuilder.DropTable(
                name: "StatementContexts");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Verbs");

            migrationBuilder.DropTable(
                name: "ContextActivities");

            migrationBuilder.DropTable(
                name: "Agents");
        }
    }
}
