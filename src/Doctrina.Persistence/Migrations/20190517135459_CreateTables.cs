using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Doctrina.Persistence.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    ActivityHash = table.Column<string>(maxLength: 32, nullable: false),
                    ActivityId = table.Column<string>(maxLength: 2083, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityHash);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    AgentHash = table.Column<string>(nullable: false),
                    ObjectType = table.Column<int>(maxLength: 12, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Mbox = table.Column<string>(maxLength: 128, nullable: true),
                    Mbox_SHA1SUM = table.Column<string>(maxLength: 40, nullable: true),
                    OpenId = table.Column<string>(nullable: true),
                    Account_HomePage = table.Column<string>(maxLength: 2083, nullable: true),
                    Account_Name = table.Column<string>(maxLength: 40, nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
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
                    ContextActivitiesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivities", x => x.ContextActivitiesId);
                });

            migrationBuilder.CreateTable(
                name: "Verbs",
                columns: table => new
                {
                    VerbHash = table.Column<string>(maxLength: 32, nullable: false),
                    Id = table.Column<string>(maxLength: 2083, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbs", x => x.VerbHash);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDefinitions",
                columns: table => new
                {
                    ActivityDefinitionId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    MoreInfo = table.Column<string>(nullable: true),
                    ActivityHash = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    InteractionType = table.Column<string>(maxLength: 12, nullable: true),
                    CorrectResponsesPattern = table.Column<string>(nullable: true),
                    InteractionId = table.Column<int>(nullable: true)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDefinitions", x => x.ActivityDefinitionId);
                    table.ForeignKey(
                        name: "FK_ActivityDefinitions_Activities_ActivityHash",
                        column: x => x.ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Key = table.Column<Guid>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 255, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    Checksum = table.Column<string>(maxLength: 50, nullable: false),
                    LastModified = table.Column<DateTimeOffset>(nullable: false, defaultValue: new DateTimeOffset(new DateTime(2019, 5, 17, 13, 54, 59, 545, DateTimeKind.Unspecified).AddTicks(2852), new TimeSpan(0, 0, 0, 0, 0))),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProfileId = table.Column<string>(nullable: true),
                    ActivityHash = table.Column<string>(nullable: true),
                    RegistrationId = table.Column<Guid>(nullable: true),
                    StateId = table.Column<string>(maxLength: 2083, nullable: true),
                    Registration = table.Column<Guid>(nullable: true),
                    AgentHash = table.Column<string>(nullable: true),
                    ActivityStateEntity_ActivityHash = table.Column<string>(nullable: true),
                    AgentProfileEntity_ProfileId = table.Column<string>(maxLength: 2083, nullable: true),
                    AgentProfileEntity_AgentHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Key);
                    table.ForeignKey(
                        name: "FK_Documents_Activities_ActivityHash",
                        column: x => x.ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Activities_ActivityStateEntity_ActivityHash",
                        column: x => x.ActivityStateEntity_ActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Agents_AgentHash",
                        column: x => x.AgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Agents_AgentProfileEntity_AgentHash",
                        column: x => x.AgentProfileEntity_AgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContextActivityTypeEntity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    ContextActivitiesEntityContextActivitiesId = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityContextActivitiesId1 = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityContextActivitiesId2 = table.Column<Guid>(nullable: true),
                    ContextActivitiesEntityContextActivitiesId3 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContextActivityTypeEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContextActivityTypeEntity_ContextActivities_ContextActivitiesEntityContextActivitiesId",
                        column: x => x.ContextActivitiesEntityContextActivitiesId,
                        principalTable: "ContextActivities",
                        principalColumn: "ContextActivitiesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivityTypeEntity_ContextActivities_ContextActivitiesEntityContextActivitiesId1",
                        column: x => x.ContextActivitiesEntityContextActivitiesId1,
                        principalTable: "ContextActivities",
                        principalColumn: "ContextActivitiesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivityTypeEntity_ContextActivities_ContextActivitiesEntityContextActivitiesId2",
                        column: x => x.ContextActivitiesEntityContextActivitiesId2,
                        principalTable: "ContextActivities",
                        principalColumn: "ContextActivitiesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContextActivityTypeEntity_ContextActivities_ContextActivitiesEntityContextActivitiesId3",
                        column: x => x.ContextActivitiesEntityContextActivitiesId3,
                        principalTable: "ContextActivities",
                        principalColumn: "ContextActivitiesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StatementBase",
                columns: table => new
                {
                    StatementId = table.Column<Guid>(nullable: false),
                    ObjectObjectType = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    ActorAgentHash = table.Column<string>(nullable: false),
                    VerbHash = table.Column<string>(nullable: false),
                    ObjectStatementRefId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false),
                    ObjectAgentAgentHash = table.Column<string>(nullable: true),
                    ObjectActivityActivityHash = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    ObjectSubStatementId = table.Column<Guid>(nullable: true),
                    Stored = table.Column<DateTimeOffset>(nullable: true),
                    Voided = table.Column<bool>(nullable: true, defaultValue: false),
                    Version = table.Column<string>(maxLength: 7, nullable: true),
                    AuthorityId = table.Column<Guid>(nullable: true),
                    FullStatement = table.Column<string>(nullable: true),
                    AuthorityAgentHash = table.Column<string>(nullable: true),
                    ObjectType = table.Column<int>(nullable: true),
                    SubStatementId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatementBase", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_StatementBase_Agents_ActorAgentHash",
                        column: x => x.ActorAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatementBase_Activities_ObjectActivityActivityHash",
                        column: x => x.ObjectActivityActivityHash,
                        principalTable: "Activities",
                        principalColumn: "ActivityHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatementBase_Agents_ObjectAgentAgentHash",
                        column: x => x.ObjectAgentAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatementBase_Verbs_VerbHash",
                        column: x => x.VerbHash,
                        principalTable: "Verbs",
                        principalColumn: "VerbHash",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatementBase_Agents_AuthorityAgentHash",
                        column: x => x.AuthorityAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatementBase_StatementBase_ObjectSubStatementId",
                        column: x => x.ObjectSubStatementId,
                        principalTable: "StatementBase",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Verbs_Display",
                columns: table => new
                {
                    DisplayId = table.Column<Guid>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    VerbHash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbs_Display", x => x.DisplayId);
                    table.ForeignKey(
                        name: "FK_Verbs_Display_Verbs_VerbHash",
                        column: x => x.VerbHash,
                        principalTable: "Verbs",
                        principalColumn: "VerbHash",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDefinitions_Descriptions",
                columns: table => new
                {
                    DescriptionId = table.Column<Guid>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActivityDefinitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDefinitions_Descriptions", x => x.DescriptionId);
                    table.ForeignKey(
                        name: "FK_ActivityDefinitions_Descriptions_ActivityDefinitions_ActivityDefinitionId",
                        column: x => x.ActivityDefinitionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDefinitions_Extensions",
                columns: table => new
                {
                    ExtensionId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ActivityDefinitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDefinitions_Extensions", x => x.ExtensionId);
                    table.ForeignKey(
                        name: "FK_ActivityDefinitions_Extensions_ActivityDefinitions_ActivityDefinitionId",
                        column: x => x.ActivityDefinitionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDefinitions_Names",
                columns: table => new
                {
                    NameId = table.Column<Guid>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActivityDefinitionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDefinitions_Names", x => x.NameId);
                    table.ForeignKey(
                        name: "FK_ActivityDefinitions_Names_ActivityDefinitions_ActivityDefinitionId",
                        column: x => x.ActivityDefinitionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choice_Choices_Components",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice_Choices_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choice_Choices_Components_ActivityDefinitions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likert_Scale_Components",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likert_Scale_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likert_Scale_Components_ActivityDefinitions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matching_Source_Components",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matching_Source_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matching_Source_Components_ActivityDefinitions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matching_Target_Components",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matching_Target_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matching_Target_Components_ActivityDefinitions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Performance_Steps_Components",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance_Steps_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Performance_Steps_Components_ActivityDefinitions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sequencing_Choices_Components",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InteractionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequencing_Choices_Components", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sequencing_Choices_Components_ActivityDefinitions_InteractionId",
                        column: x => x.InteractionId,
                        principalTable: "ActivityDefinitions",
                        principalColumn: "ActivityDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsageType = table.Column<string>(maxLength: 2083, nullable: false),
                    CanonicalData = table.Column<string>(type: "ntext", nullable: true),
                    ContentType = table.Column<string>(maxLength: 255, nullable: false),
                    Payload = table.Column<byte[]>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SHA2 = table.Column<string>(nullable: false),
                    Length = table.Column<long>(nullable: false),
                    StatementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_StatementBase_StatementId",
                        column: x => x.StatementId,
                        principalTable: "StatementBase",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Context",
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
                    StatementId = table.Column<Guid>(nullable: true),
                    ContextActivitiesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Context", x => x.ContextId);
                    table.ForeignKey(
                        name: "FK_Context_ContextActivities_ContextActivitiesId",
                        column: x => x.ContextActivitiesId,
                        principalTable: "ContextActivities",
                        principalColumn: "ContextActivitiesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Context_Agents_InstructorAgentHash",
                        column: x => x.InstructorAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Context_StatementBase_StatementId",
                        column: x => x.StatementId,
                        principalTable: "StatementBase",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Context_Agents_TeamAgentHash",
                        column: x => x.TeamAgentHash,
                        principalTable: "Agents",
                        principalColumn: "AgentHash",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(nullable: false),
                    Success = table.Column<bool>(nullable: true),
                    Completion = table.Column<bool>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    DurationTicks = table.Column<long>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    StatementId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_Results_StatementBase_StatementId",
                        column: x => x.StatementId,
                        principalTable: "StatementBase",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choice_Choices_Components_Description",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice_Choices_Components_Description", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choice_Choices_Components_Description_Choice_Choices_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Choice_Choices_Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likert_Scale_Components_Description",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likert_Scale_Components_Description", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likert_Scale_Components_Description_Likert_Scale_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Likert_Scale_Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matching_Source_Components_Description",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matching_Source_Components_Description", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matching_Source_Components_Description_Matching_Source_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Matching_Source_Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matching_Target_Components_Description",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matching_Target_Components_Description", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matching_Target_Components_Description_Matching_Target_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Matching_Target_Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Performance_Steps_Components_Description",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance_Steps_Components_Description", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Performance_Steps_Components_Description_Performance_Steps_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Performance_Steps_Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sequencing_Choices_Components_Description",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LanguageCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ComponentId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequencing_Choices_Components_Description", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sequencing_Choices_Components_Description_Sequencing_Choices_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Sequencing_Choices_Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Result_Scores",
                columns: table => new
                {
                    ScoreId = table.Column<Guid>(nullable: false),
                    Scaled = table.Column<double>(nullable: true),
                    Raw = table.Column<double>(nullable: true),
                    Min = table.Column<double>(nullable: true),
                    Max = table.Column<double>(nullable: true),
                    ResultId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result_Scores", x => x.ScoreId);
                    table.ForeignKey(
                        name: "FK_Result_Scores_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results_Extensions",
                columns: table => new
                {
                    ExtensionId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ResultId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results_Extensions", x => x.ExtensionId);
                    table.ForeignKey(
                        name: "FK_Results_Extensions_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDefinitions_ActivityHash",
                table: "ActivityDefinitions",
                column: "ActivityHash",
                unique: true,
                filter: "[ActivityHash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDefinitions_Descriptions_ActivityDefinitionId",
                table: "ActivityDefinitions_Descriptions",
                column: "ActivityDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDefinitions_Extensions_ActivityDefinitionId",
                table: "ActivityDefinitions_Extensions",
                column: "ActivityDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDefinitions_Names_ActivityDefinitionId",
                table: "ActivityDefinitions_Names",
                column: "ActivityDefinitionId");

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
                name: "IX_Agents_ObjectType_AgentHash",
                table: "Agents",
                columns: new[] { "ObjectType", "AgentHash" },
                unique: true);

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
                name: "IX_Attachments_StatementId",
                table: "Attachments",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_Choices_Components_InteractionId",
                table: "Choice_Choices_Components",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_Choices_Components_Description_ComponentId",
                table: "Choice_Choices_Components_Description",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Context_ContextActivitiesId",
                table: "Context",
                column: "ContextActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Context_InstructorAgentHash",
                table: "Context",
                column: "InstructorAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Context_StatementId",
                table: "Context",
                column: "StatementId",
                unique: true,
                filter: "[StatementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Context_TeamAgentHash",
                table: "Context",
                column: "TeamAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivityTypeEntity_ContextActivitiesEntityContextActivitiesId",
                table: "ContextActivityTypeEntity",
                column: "ContextActivitiesEntityContextActivitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivityTypeEntity_ContextActivitiesEntityContextActivitiesId1",
                table: "ContextActivityTypeEntity",
                column: "ContextActivitiesEntityContextActivitiesId1");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivityTypeEntity_ContextActivitiesEntityContextActivitiesId2",
                table: "ContextActivityTypeEntity",
                column: "ContextActivitiesEntityContextActivitiesId2");

            migrationBuilder.CreateIndex(
                name: "IX_ContextActivityTypeEntity_ContextActivitiesEntityContextActivitiesId3",
                table: "ContextActivityTypeEntity",
                column: "ContextActivitiesEntityContextActivitiesId3");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ActivityHash",
                table: "Documents",
                column: "ActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProfileId_ActivityHash",
                table: "Documents",
                columns: new[] { "ProfileId", "ActivityHash" },
                unique: true,
                filter: "[ProfileId] IS NOT NULL AND [ActivityHash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ActivityStateEntity_ActivityHash",
                table: "Documents",
                column: "ActivityStateEntity_ActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AgentHash",
                table: "Documents",
                column: "AgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StateId_AgentHash_ActivityStateEntity_ActivityHash_Registration",
                table: "Documents",
                columns: new[] { "StateId", "AgentHash", "ActivityStateEntity_ActivityHash", "Registration" },
                unique: true,
                filter: "[StateId] IS NOT NULL AND [AgentHash] IS NOT NULL AND [ActivityStateEntity_ActivityHash] IS NOT NULL AND [Registration] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AgentProfileEntity_AgentHash",
                table: "Documents",
                column: "AgentProfileEntity_AgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AgentProfileEntity_ProfileId",
                table: "Documents",
                column: "AgentProfileEntity_ProfileId",
                unique: true,
                filter: "[AgentProfileEntity_ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AgentProfileEntity_ProfileId_AgentProfileEntity_AgentHash",
                table: "Documents",
                columns: new[] { "AgentProfileEntity_ProfileId", "AgentProfileEntity_AgentHash" },
                unique: true,
                filter: "[AgentProfileEntity_ProfileId] IS NOT NULL AND [AgentProfileEntity_AgentHash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Likert_Scale_Components_InteractionId",
                table: "Likert_Scale_Components",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Likert_Scale_Components_Description_ComponentId",
                table: "Likert_Scale_Components_Description",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matching_Source_Components_InteractionId",
                table: "Matching_Source_Components",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Matching_Source_Components_Description_ComponentId",
                table: "Matching_Source_Components_Description",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matching_Target_Components_InteractionId",
                table: "Matching_Target_Components",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Matching_Target_Components_Description_ComponentId",
                table: "Matching_Target_Components_Description",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Performance_Steps_Components_InteractionId",
                table: "Performance_Steps_Components",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Performance_Steps_Components_Description_ComponentId",
                table: "Performance_Steps_Components_Description",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_Scores_ResultId",
                table: "Result_Scores",
                column: "ResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_StatementId",
                table: "Results",
                column: "StatementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_Extensions_ResultId",
                table: "Results_Extensions",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Sequencing_Choices_Components_InteractionId",
                table: "Sequencing_Choices_Components",
                column: "InteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sequencing_Choices_Components_Description_ComponentId",
                table: "Sequencing_Choices_Components_Description",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_StatementBase_ActorAgentHash",
                table: "StatementBase",
                column: "ActorAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_StatementBase_ObjectActivityActivityHash",
                table: "StatementBase",
                column: "ObjectActivityActivityHash");

            migrationBuilder.CreateIndex(
                name: "IX_StatementBase_ObjectAgentAgentHash",
                table: "StatementBase",
                column: "ObjectAgentAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_StatementBase_VerbHash",
                table: "StatementBase",
                column: "VerbHash");

            migrationBuilder.CreateIndex(
                name: "IX_StatementBase_AuthorityAgentHash",
                table: "StatementBase",
                column: "AuthorityAgentHash");

            migrationBuilder.CreateIndex(
                name: "IX_StatementBase_ObjectSubStatementId",
                table: "StatementBase",
                column: "ObjectSubStatementId",
                unique: true,
                filter: "[ObjectSubStatementId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Verbs_Display_VerbHash",
                table: "Verbs_Display",
                column: "VerbHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDefinitions_Descriptions");

            migrationBuilder.DropTable(
                name: "ActivityDefinitions_Extensions");

            migrationBuilder.DropTable(
                name: "ActivityDefinitions_Names");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Choice_Choices_Components_Description");

            migrationBuilder.DropTable(
                name: "Context");

            migrationBuilder.DropTable(
                name: "ContextActivityTypeEntity");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Likert_Scale_Components_Description");

            migrationBuilder.DropTable(
                name: "Matching_Source_Components_Description");

            migrationBuilder.DropTable(
                name: "Matching_Target_Components_Description");

            migrationBuilder.DropTable(
                name: "Performance_Steps_Components_Description");

            migrationBuilder.DropTable(
                name: "Result_Scores");

            migrationBuilder.DropTable(
                name: "Results_Extensions");

            migrationBuilder.DropTable(
                name: "Sequencing_Choices_Components_Description");

            migrationBuilder.DropTable(
                name: "Verbs_Display");

            migrationBuilder.DropTable(
                name: "Choice_Choices_Components");

            migrationBuilder.DropTable(
                name: "ContextActivities");

            migrationBuilder.DropTable(
                name: "Likert_Scale_Components");

            migrationBuilder.DropTable(
                name: "Matching_Source_Components");

            migrationBuilder.DropTable(
                name: "Matching_Target_Components");

            migrationBuilder.DropTable(
                name: "Performance_Steps_Components");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Sequencing_Choices_Components");

            migrationBuilder.DropTable(
                name: "StatementBase");

            migrationBuilder.DropTable(
                name: "ActivityDefinitions");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Verbs");

            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
