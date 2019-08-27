﻿// <auto-generated />
using System;
using CROPS.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CROPS.Migrations
{
    [DbContext(typeof(CROPSDbContext))]
    [Migration("20190523131749_initial_sql_script")]
    partial class initial_sql_script
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CROPS.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountName");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Descriptions");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ParentId");

                    b.HasKey("AccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("CROPS.Models.Assignee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IsActive");

                    b.Property<int?>("LevelCode");

                    b.Property<string>("Name");

                    b.Property<int?>("ProjectId");

                    b.Property<int?>("RoleCode");

                    b.HasKey("Id");

                    b.ToTable("Assignee");
                });

            modelBuilder.Entity("CROPS.Models.AssigneeLevels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Level");

                    b.Property<int?>("LevelCode");

                    b.HasKey("Id");

                    b.ToTable("AssigneeLevels");
                });

            modelBuilder.Entity("CROPS.Models.AssigneeRoles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Role");

                    b.Property<int?>("RoleCode");

                    b.HasKey("Id");

                    b.ToTable("AssigneeRoles");
                });

            modelBuilder.Entity("CROPS.Models.Bug", b =>
                {
                    b.Property<int>("BugId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Activity");

                    b.Property<string>("AssignedTo");

                    b.Property<string>("BugName");

                    b.Property<int?>("BugSourceId");

                    b.Property<decimal?>("CompletedWork");

                    b.Property<DateTime?>("CreationDate");

                    b.Property<int?>("IsActive");

                    b.Property<int?>("IterationSk");

                    b.Property<decimal?>("OriginalEstimate");

                    b.Property<int?>("ProjectId");

                    b.Property<decimal?>("RemainingWork");

                    b.Property<string>("State");

                    b.Property<int?>("UserStorySourceId");

                    b.HasKey("BugId");

                    b.ToTable("Bug");
                });

            modelBuilder.Entity("CROPS.Models.Dashboard", b =>
                {
                    b.Property<string>("DashboardId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContainerWorkspaceWorkspaceId");

                    b.Property<string>("Description")
                        .HasMaxLength(1500);

                    b.Property<string>("Id");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("DashboardId");

                    b.HasIndex("ContainerWorkspaceWorkspaceId");

                    b.ToTable("Dashboard");
                });

            modelBuilder.Entity("CROPS.Models.Epic", b =>
                {
                    b.Property<int>("EpicId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreationDate");

                    b.Property<string>("EpicName");

                    b.Property<int?>("EpicSourceId");

                    b.Property<int?>("IsActive");

                    b.Property<int?>("IterationSk");

                    b.Property<int?>("ProjectId");

                    b.Property<string>("State");

                    b.HasKey("EpicId");

                    b.ToTable("Epic");
                });

            modelBuilder.Entity("CROPS.Models.Feature", b =>
                {
                    b.Property<int>("FeatureId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreationDate");

                    b.Property<string>("FeatureName");

                    b.Property<int?>("FeatureSourceId");

                    b.Property<int?>("IsActive");

                    b.Property<int?>("IterationSk");

                    b.Property<int?>("ProjectId");

                    b.Property<string>("State");

                    b.HasKey("FeatureId");

                    b.ToTable("Feature");
                });

            modelBuilder.Entity("CROPS.Models.Iteration", b =>
                {
                    b.Property<int>("IterationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Depth");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("IterationName");

                    b.Property<int?>("IterationSourceId");

                    b.Property<DateTime?>("LastUpdatedDateTime");

                    b.Property<int?>("ProjectId");

                    b.Property<int?>("Scope");

                    b.Property<DateTime?>("StartDate");

                    b.HasKey("IterationId");

                    b.ToTable("Iteration");
                });

            modelBuilder.Entity("CROPS.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int?>("AuthType");

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ParentId");

                    b.Property<string>("ParentName");

                    b.Property<int?>("ParentSourceId");

                    b.Property<string>("Password");

                    b.Property<string>("ProjectAreaPath");

                    b.Property<string>("ProjectName");

                    b.Property<int?>("ProjectSourceId");

                    b.Property<string>("ProjectType");

                    b.Property<int?>("ReleaseSourceId");

                    b.Property<string>("UserName");

                    b.Property<string>("WorkspaceId");

                    b.HasKey("ProjectId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("CROPS.Models.ProjectData", b =>
                {
                    b.Property<int>("ProjectDataId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreationDate");

                    b.Property<int>("OptimisticDate");

                    b.Property<int>("PessimisticDate");

                    b.Property<int>("ProjectId");

                    b.HasKey("ProjectDataId");

                    b.ToTable("ProjectData","Confg");
                });

            modelBuilder.Entity("CROPS.Models.ProjectDetails", b =>
                {
                    b.Property<int>("ProjectDetailsId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CreationDate");

                    b.Property<string>("Descriptions");

                    b.Property<byte[]>("Logo");

                    b.Property<int>("OptimisticFinalIteration");

                    b.Property<int>("PessimisticFinalIteration");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime>("PullDataFromDate");

                    b.Property<DateTime>("PullDataToDate");

                    b.Property<int?>("Scope");

                    b.HasKey("ProjectDetailsId");

                    b.ToTable("ProjectDetails");
                });

            modelBuilder.Entity("CROPS.Models.Report", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContainerWorkspaceWorkspaceId")
                        .HasMaxLength(450);

                    b.Property<string>("Description")
                        .HasMaxLength(1500);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.Property<string>("ReportId")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("ContainerWorkspaceWorkspaceId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("CROPS.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Activity");

                    b.Property<string>("AssignedTo");

                    b.Property<decimal?>("CompletedWork");

                    b.Property<DateTime?>("CreationDate");

                    b.Property<int?>("IsActive");

                    b.Property<int?>("IterationSk");

                    b.Property<decimal?>("OriginalEstimate");

                    b.Property<int?>("ProjectId");

                    b.Property<decimal?>("RemainingWork");

                    b.Property<string>("State");

                    b.Property<string>("TaskName");

                    b.Property<int?>("TaskSourceId");

                    b.Property<int?>("UserStorySourceId");

                    b.HasKey("TaskId");

                    b.ToTable("Task");
                });

            modelBuilder.Entity("CROPS.Models.UserStory", b =>
                {
                    b.Property<int>("UserStoryId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedTo");

                    b.Property<DateTime?>("CreationDate");

                    b.Property<int?>("FeatureSourceId");

                    b.Property<int?>("IsActive");

                    b.Property<bool?>("IsCr");

                    b.Property<int?>("IterationSk");

                    b.Property<int?>("ProjectId");

                    b.Property<string>("State");

                    b.Property<int?>("StoryPoints");

                    b.Property<string>("UserStoryName");

                    b.Property<int?>("UserStorySourceId");

                    b.HasKey("UserStoryId");

                    b.ToTable("UserStory");
                });

            modelBuilder.Entity("CROPS.Models.Workspace", b =>
                {
                    b.Property<string>("WorkspaceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Id");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("WorkspaceId");

                    b.ToTable("Workspace");
                });

            modelBuilder.Entity("CROPS.Models.Dashboard", b =>
                {
                    b.HasOne("CROPS.Models.Workspace", "ContainerWorkspace")
                        .WithMany()
                        .HasForeignKey("ContainerWorkspaceWorkspaceId");
                });

            modelBuilder.Entity("CROPS.Models.Report", b =>
                {
                    b.HasOne("CROPS.Models.Workspace", "ContainerWorkspace")
                        .WithMany()
                        .HasForeignKey("ContainerWorkspaceWorkspaceId");
                });
#pragma warning restore 612, 618
        }
    }
}
