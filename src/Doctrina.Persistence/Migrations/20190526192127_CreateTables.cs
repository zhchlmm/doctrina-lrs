using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctrina.Persistence.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentHash = table.Column<string>(nullable: false),
                    ObjectType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Mbox = table.Column<string>(maxLength: 128, nullable: true),
                    Mbox_SHA1SUM = table.Column<string>(maxLength: 40, nullable: true),
                    OpenId = table.Column<string>(nullable: true),
                    Account_HomePage = table.Column<string>(maxLength: 2083, nullable: true),
                    Account_Name = table.Column<string>(maxLength: 40, nullable: true),
                    GroupEntityAgentHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.AgentHash);
                    table.ForeignKey(
                        name: "FK_Agents_Agents_GroupEntityAgentHash",
                        column: x => x.GroupEntityAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivities",
                columns: table => new
                {
                    ContextActivitiesId = table.Column<Guid>(nullable: false),
                    Parent = table.Column<string>(type: "ntext", nullable: true),
                    Grouping = table.Column<string>(type: "ntext", nullable: true),
                    Category = table.Column<string>(type: "ntext", nullable: true),
                    Other = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivities", x => x.ContextActivitiesId);
                });

            migrationBuilder.CreateTable(
                name: "InteractionActivities",
                columns: table => new
                {
                    InteractionId = table.Column<Guid>(nullable: false),
                    InteractionType = table.Column<string>(nullable: false),
                    CorrectResponsesPattern = table.Column<string>(nullable: true),
                    Choices = table.Column<string>(type: "ntext", nullable: true),
                    Scale = table.Column<string>(type: "ntext", nullable: true),
                    Source = table.Column<string>(type: "ntext", nullable: true),
                    Target = table.Column<string>(type: "ntext", nullable: true),
                    Steps = table.Column<string>(type: "ntext", nullable: true),
                    SequencingInteractionActivity_Choices = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InteractionActivities", x => x.InteractionId);
                });

            migrationBuilder.CreateTable(
                name: "ResultEntity",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(nullable: false),
                    Success = table.Column<bool>(nullable: true),
                    Completion = table.Column<bool>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    DurationTicks = table.Column<long>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    Extensions = table.Column<string>(type: "ntext", nullable: true),
                    Score_Scaled = table.Column<double>(nullable: true),
                    Score_Raw = table.Column<double>(nullable: true),
                    Score_Min = table.Column<double>(nullable: true),
                    Score_Max = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultEntity", x => x.ResultId);
                });

            migrationBuilder.CreateTable(
                name: "StatementRefEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StatementRefId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementRefEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verbs",
                columns: table => new
                {
                    VerbHash = table.Column<string>(maxLength: 32, nullable: false),
                    Id = table.Column<string>(maxLength: 2083, nullable: false),
                    Display = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbs", x => x.VerbHash);
                });

            migrationBuilder.CreateTable(
                name: "AgentProfiles",
                columns: table => new
                {
                    AgentProfileId = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<string>(maxLength: 2083, nullable: false),
                    AgentHash = table.Column<string>(nullable: true),
                    Document_ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Document_Content = table.Column<byte[]>(nullable: true),
                    Document_Checksum = table.Column<string>(maxLength: 50, nullable: false),
                    Document_LastModified = table.Column<DateTimeOffset>(nullable: false, defaultValue: new DateTimeOffset(new DateTime(2019, 5, 26, 19, 21, 27, 563, DateTimeKind.Unspecified).AddTicks(3426), new TimeSpan(0, 0, 0, 0, 0))),
                    Document_CreateDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentProfiles", x => x.AgentProfileId);
                    table.ForeignKey(
                        name: "FK_AgentProfiles_Agents_AgentHash",
                        column: x => x.AgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contexts",
                columns: table => new
                {
                    ContextId = table.Column<Guid>(nullable: false),
                    Registration = table.Column<Guid>(nullable: true),
                    InstructorAgentHash = table.Column<string>(nullable: true),
                    TeamAgentHash = table.Column<string>(nullable: true),
                    Revision = table.Column<string>(nullable: true),
                    Platform = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Extensions = table.Column<string>(type: "ntext", nullable: true),
                    ContextActivitiesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contexts", x => x.ContextId);
                    table.ForeignKey(
                        name: "FK_Contexts_ContextActivities_ContextActivitiesId",
                        column: x => x.ContextActivitiesId,
                        principalTable: "ContextActivities",
                        principalColumn: "ContextActivitiesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contexts_Agents_InstructorAgentHash",
                        column: x => x.InstructorAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contexts_Agents_TeamAgentHash",
                        column: x => x.TeamAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDefinitions",
                columns: table => new
                {
                    ActivityDefinitionId = table.Column<Guid>(nullable: false),
                    Names = table.Column<string>(type: "ntext", nullable: true),
                    Descriptions = table.Column<string>(type: "ntext", nullable: true),
                    Type = table.Column<string>(nullable: true),
                    MoreInfo = table.Column<string>(nullable: true),
                    InteractionActivityInteractionId = table.Column<Guid>(nullable: true),
                    Extensions = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDefinitions", x => x.ActivityDefinitionId);
                    table.ForeignKey(
                        name: "FK_ActivityDefinitions_InteractionActivities_InteractionActivityInteractionId",
                        column: x => x.InteractionActivityInteractionId,
                        principalTable: "InteractionActivities",
                        principalColumn: "InteractionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityHash = table.Column<string>(maxLength: 32, nullable: false),
                    ActivityId = table.Column<string>(maxLength: 2083, nullable: false),
                    DefinitionActivityDefinitionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityHash);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityDefinitions_DefinitionActivityDefinitionId",
                        column: x => x.DefinitionActivityDefinitionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityProfiles",
                columns: table => new
                {
                    ActivityProfileId = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<string>(nullable: false),
                    ActivityHash = table.Column<string>(nullable: true),
                    RegistrationId = table.Column<Guid>(nullable: true),
                    Document_ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Document_Content = table.Column<byte[]>(nullable: true),
                    Document_Checksum = table.Column<string>(maxLength: 50, nullable: false),
                    Document_LastModified = table.Column<DateTimeOffset>(nullable: false, defaultValue: new DateTimeOffset(new DateTime(2019, 5, 26, 19, 21, 27, 539, DateTimeKind.Unspecified).AddTicks(7199), new TimeSpan(0, 0, 0, 0, 0))),
                    Document_CreateDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityProfiles", x => x.ActivityProfileId);
                    table.ForeignKey(
                        name: "FK_ActivityProfiles_Activities_ActivityHash",
                        column: x => x.ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityStates",
                columns: table => new
                {
                    ActivityStateId = table.Column<Guid>(nullable: false),
                    StateId = table.Column<string>(maxLength: 2083, nullable: false),
                    Registration = table.Column<Guid>(nullable: true),
                    Document_ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Document_Content = table.Column<byte[]>(nullable: true),
                    Document_Checksum = table.Column<string>(maxLength: 50, nullable: false),
                    Document_LastModified = table.Column<DateTimeOffset>(nullable: false, defaultValue: new DateTimeOffset(new DateTime(2019, 5, 26, 19, 21, 27, 554, DateTimeKind.Unspecified).AddTicks(2222), new TimeSpan(0, 0, 0, 0, 0))),
                    Document_CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    AgentHash = table.Column<string>(nullable: true),
                    ActivityHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityStates", x => x.ActivityStateId);
                    table.ForeignKey(
                        name: "FK_ActivityStates_Activities_ActivityHash",
                        column: x => x.ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityStates_Agents_AgentHash",
                        column: x => x.AgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubStatements",
                columns: table => new
                {
                    SubStatementId = table.Column<Guid>(nullable: false),
                    VerbHash = table.Column<string>(nullable: false),
                    ActorAgentHash = table.Column<string>(nullable: false),
                    Object_ObjectType = table.Column<int>(nullable: false),
                    Object_AgentHash = table.Column<string>(nullable: true),
                    Object_ActivityHash = table.Column<string>(nullable: true),
                    Object_StatementRefId = table.Column<Guid>(nullable: true),
                    ContextId = table.Column<Guid>(nullable: true),
                    ResultId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubStatements", x => x.SubStatementId);
                    table.ForeignKey(
                        name: "FK_SubStatements_Agents_ActorAgentHash",
                        column: x => x.ActorAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubStatements_Contexts_ContextId",
                        column: x => x.ContextId,
                        principalTable: "Contexts",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_ResultEntity_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ResultEntity",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_Verbs_VerbHash",
                        column: x => x.VerbHash,
                        principalTable: "Verbs",
                        principalColumn: "VerbHash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubStatements_Activities_Object_ActivityHash",
                        column: x => x.Object_ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_Agents_Object_AgentHash",
                        column: x => x.Object_AgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubStatements_StatementRefEntity_Object_StatementRefId",
                        column: x => x.Object_StatementRefId,
                        principalTable: "StatementRefEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    StatementId = table.Column<Guid>(nullable: false),
                    ActorAgentHash = table.Column<string>(nullable: false),
                    VerbHash = table.Column<string>(nullable: false),
                    Object_ObjectType = table.Column<int>(nullable: false),
                    Object_AgentHash = table.Column<string>(nullable: true),
                    Object_ActivityHash = table.Column<string>(nullable: true),
                    Object_SubStatementId = table.Column<Guid>(nullable: true),
                    Object_StatementRefId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false),
                    ResultId = table.Column<Guid>(nullable: true),
                    ContextId = table.Column<Guid>(nullable: true),
                    Stored = table.Column<DateTimeOffset>(nullable: false),
                    Version = table.Column<string>(maxLength: 7, nullable: true),
                    AuthorityId = table.Column<Guid>(nullable: true),
                    FullStatement = table.Column<string>(nullable: true),
                    Voided = table.Column<bool>(nullable: false, defaultValue: false),
                    AuthorityAgentHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_Statements_Agents_ActorAgentHash",
                        column: x => x.ActorAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statements_Agents_AuthorityAgentHash",
                        column: x => x.AuthorityAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Contexts_ContextId",
                        column: x => x.ContextId,
                        principalTable: "Contexts",
                        principalColumn: "ContextId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_ResultEntity_ResultId",
                        column: x => x.ResultId,
                        principalTable: "ResultEntity",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Verbs_VerbHash",
                        column: x => x.VerbHash,
                        principalTable: "Verbs",
                        principalColumn: "VerbHash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statements_Activities_Object_ActivityHash",
                        column: x => x.Object_ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Agents_Object_AgentHash",
                        column: x => x.Object_AgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_StatementRefEntity_Object_StatementRefId",
                        column: x => x.Object_StatementRefId,
                        principalTable: "StatementRefEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_SubStatements_Object_SubStatementId",
                        column: x => x.Object_SubStatementId,
                        principalTable: "SubStatements",
                        principalColumn: "SubStatementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubStatement_Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    UsageType = table.Column<string>(maxLength: 2083, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Display = table.Column<string>(type: "ntext", nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: false),
                    Payload = table.Column<byte[]>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SHA2 = table.Column<string>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    StatementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubStatement_Attachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_SubStatement_Attachments_SubStatements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "SubStatements",
                        principalColumn: "SubStatementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statement_Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    UsageType = table.Column<string>(maxLength: 2083, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    Display = table.Column<string>(type: "ntext", nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: false),
                    Payload = table.Column<byte[]>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SHA2 = table.Column<string>(nullable: false),
                    Length = table.Column<int>(nullable: false),
                    StatementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statement_Attachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Statement_Attachments_Statements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DefinitionActivityDefinitionId",
                table: "Activities",
                column: "DefinitionActivityDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDefinitions_InteractionActivityInteractionId",
                table: "ActivityDefinitions",
                column: "InteractionActivityInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityProfiles_ActivityHash",
                table: "ActivityProfiles",
                column: "ActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStates_ActivityHash",
                table: "ActivityStates",
                column: "ActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStates_AgentHash",
                table: "ActivityStates",
                column: "AgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProfiles_AgentHash",
                table: "AgentProfiles",
                column: "AgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_AgentProfiles_ProfileId",
                table: "AgentProfiles",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_Account_HomePage_Account_Name",
                table: "Agents",
                columns: new[] { "Account_HomePage", "Account_Name" },
                unique: true,
                filter: "[Account_HomePage] IS NOT NULL AND [Account_Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_GroupEntityAgentHash",
                table: "Agents",
                column: "GroupEntityAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Contexts_ContextActivitiesId",
                table: "Contexts",
                column: "ContextActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Contexts_InstructorAgentHash",
                table: "Contexts",
                column: "InstructorAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Contexts_TeamAgentHash",
                table: "Contexts",
                column: "TeamAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Statement_Attachments_StatementId",
                table: "Statement_Attachments",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ActorAgentHash",
                table: "Statements",
                column: "ActorAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_AuthorityAgentHash",
                table: "Statements",
                column: "AuthorityAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ContextId",
                table: "Statements",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ResultId",
                table: "Statements",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_VerbHash",
                table: "Statements",
                column: "VerbHash");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_Object_ActivityHash",
                table: "Statements",
                column: "Object_ActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_Object_AgentHash",
                table: "Statements",
                column: "Object_AgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_Object_StatementRefId",
                table: "Statements",
                column: "Object_StatementRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_Object_SubStatementId",
                table: "Statements",
                column: "Object_SubStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatement_Attachments_StatementId",
                table: "SubStatement_Attachments",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ActorAgentHash",
                table: "SubStatements",
                column: "ActorAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ContextId",
                table: "SubStatements",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_ResultId",
                table: "SubStatements",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_VerbHash",
                table: "SubStatements",
                column: "VerbHash");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_Object_ActivityHash",
                table: "SubStatements",
                column: "Object_ActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_Object_AgentHash",
                table: "SubStatements",
                column: "Object_AgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_SubStatements_Object_StatementRefId",
                table: "SubStatements",
                column: "Object_StatementRefId");
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
                name: "Statement_Attachments");

            migrationBuilder.DropTable(
                name: "SubStatement_Attachments");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "SubStatements");

            migrationBuilder.DropTable(
                name: "Contexts");

            migrationBuilder.DropTable(
                name: "ResultEntity");

            migrationBuilder.DropTable(
                name: "Verbs");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "StatementRefEntity");

            migrationBuilder.DropTable(
                name: "ContextActivities");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "ActivityDefinitions");

            migrationBuilder.DropTable(
                name: "InteractionActivities");
        }
    }
}
