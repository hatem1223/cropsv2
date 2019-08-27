using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class initial_sql_script : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
sp_changedbowner 'sa'
/****** Object:  User [AppUser]    Script Date: 5/23/2019 12:56:46 PM ******/
CREATE USER[AppUser] FOR LOGIN[AppUser] WITH DEFAULT_SCHEMA =[dbo]
GO
ALTER ROLE[db_owner] ADD MEMBER[AppUser]
GO
/****** Object:  Schema [ETL]    Script Date: 5/23/2019 12:56:47 PM ******/
CREATE SCHEMA[ETL]
GO
/****** Object:  Schema [FN]    Script Date: 5/23/2019 12:56:47 PM ******/
CREATE SCHEMA[FN]
GO
/****** Object:  Schema [ML]    Script Date: 5/23/2019 12:56:47 PM ******/
CREATE SCHEMA[ML]
GO
/****** Object:  Schema [TFS]    Script Date: 5/23/2019 12:56:47 PM ******/
CREATE SCHEMA[TFS]
GO
/****** Object:  View [dbo].[CompletedWorkByIteration]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW[dbo].[CompletedWorkByIteration]
AS
SELECT
  I.projectID,
  I.IterationSourceID,
  I.IterationName,
  SUM(T.CompletedWork) AS ActualWork
FROM task AS t,
     iteration AS I
WHERE t.iterationsk = I.iterationsourceID
GROUP BY I.projectID,
         I.iterationname,
         I.IterationSourceID
GO
/****** Object:  View [dbo].[VAssigneeRoles]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View[dbo].[VAssigneeRoles]
as
select A.Projectid, A.Name, isnull(Ar.Role, 'NOT Assigned') Roles, sum(t.CompletedWork) as CompletedWork, sum(t.OriginalEstimate) as OriginalEstimate
from Assignee as A left join AssigneeRoles as AR on A.Rolecode = AR.rolecode
left join Task as t on t.AssignedTo = a.Name

group by A.Projectid, Ar.Role, A.Name
GO

/****** Object:  View [dbo].[VPD_CountOfClosedUserStoriesWithoutActiveTasks]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_CountOfClosedUserStoriesWithoutActiveTasks]
as
(
select t.*from
                   (
                      select t.* from userstory as U , task as t

                      where  u.state = 'Closed' and u.projectid = t.projectid and

                             t.UserStorySourceId = u.UserStorySourceId--and t.ProjectID = 1
                   )
as t inner join Iteration as I on I.IterationSourceID = t.IterationSK and t.ProjectID = I.ProjectID
where t.state <> 'closed' and I.Depth <> 0  and I.StartDate <= getdate()
)
GO
/****** Object:  View [dbo].[VPD_CountOfOpenedTasksInPastIteration]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_CountOfOpenedTasksInPastIteration]
as
(
 select t.*from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
 where t.state <> 'Closed'  and t.ProjectID = I.ProjectID  and I.StartDate <= getdate() and I.Depth <> 0
 )
GO
/****** Object:  View [dbo].[VPD_CountOfTasksWithoutAssignee]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create VIEW[dbo].[VPD_CountOfTasksWithoutAssignee]
as
(
Select t.*from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
where t.state <> 'new' and t.Assignedto = 'NOT Assigned' and t.ProjectID = I.ProjectID   and I.Depth <> 0
)
GO
/****** Object:  View [dbo].[VPD_CountTasksWithoutActivity]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_CountTasksWithoutActivity]
as
(
Select t.*from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
where t.State <> 'new' and t.Activity is null  and t.ProjectID = I.ProjectID   and I.Depth <> 0 and I.StartDate <= getdate()
)
GO
/****** Object:  View [dbo].[VPD_MissingCompletedWork]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_MissingCompletedWork]
as
(
 select t.*from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
 where t.CompletedWork = 0 and t.state <> 'new' and t.ProjectID = I.ProjectID    and I.Depth <> 0 and I.StartDate <= getdate()
)
GO
/****** Object:  View [dbo].[VPD_MissingStoryPoints]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_MissingStoryPoints]
as
(
SELECT U.*FROM userstory as U inner join Iteration as I on I.IterationSourceID = U.IterationSK
WHERE u.StoryPoints = 0 and u.STATE <> 'new' and u.ProjectID = I.ProjectID    and I.Depth <> 0 and I.StartDate <= getdate()
)							
GO
/****** Object:  View [dbo].[VPD_TasksWithoutEstimets]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_TasksWithoutEstimets]
as
(
select t.*from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
where t.OriginalEstimate = 0 and t.state <> 'new' and t.ProjectID = I.ProjectID  and I.Depth <> 0 and I.StartDate <= getdate()
)
GO
/****** Object:  View [dbo].[VPD_UserStoriesNotlinkedToAnyFeature]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW[dbo].[VPD_UserStoriesNotlinkedToAnyFeature]
as
(
    select U.*from userstory as U inner join Iteration as I on I.IterationSourceID = U.IterationSK

    where u.FeatureSourceId is null   and I.ProjectID = u.ProjectID    and I.Depth <> 0 and I.StartDate <= getdate()
	)
GO
/****** Object:  View [dbo].[VQualityOfEstimates_PendingUserStory]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View[dbo].[VQualityOfEstimates_PendingUserStory]
as
Select distinct F.FeatureName,US.* from Userstory as US left Join Feature as f on US.FeatureSourceId = F.FeatureSourceId where US.state = 'New'

GO

/****** Object:  Table [dbo].[BurnRate_QualityOfOutput]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[BurnRate_QualityOfOutput](

    [Assignedto][nchar](100) NULL,

    [CountOfResolvedUserstory] [decimal](5, 2) NULL,
	[CountOfResolvedBugs] [decimal](5, 2) NULL,
	[ActualWork] [varchar] (8000) NULL,
	[QualityofOutput] [decimal](13, 8) NULL,
	[BurnRate/Hour] [decimal](13, 8) NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[ChangeRequest]    Script Date: 5/23/2019 12:56:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[ChangeRequest]
        (

   [ProjectID][int] NULL,

   [IterationSk][int] NULL,

   [IterationName][nvarchar](200) NOT NULL,

  [OriginalEffort] [decimal](10, 2) NULL,
	[ChangeRequestEffort] [decimal](10, 2) NULL,
	[AVG] [decimal](10, 2) NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[ChangeRequest_KPI]    Script Date: 5/23/2019 12:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[ChangeRequest_KPI]
        (

   [ProjectID][int] NOT NULL,

   [Zero] [int] NOT NULL,

   [YellowPand] [int] NOT NULL,

   [GreenPand] [int] NOT NULL,

   [Score] [int] NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryTracking]    Script Date: 5/23/2019 12:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[DeliveryTracking]
        (

   [BurnupID][int] NOT NULL,

   [ProjectID] [int] NULL,
	[IterationID] [int] NULL,
	[Iteration] [nvarchar] (50) NULL,
	[Actual] [decimal](10, 2) NULL,
	[Accumulative] [decimal](10, 2) NULL,
	[Slope] [decimal](10, 2) NULL,
	[Optimistic] [decimal](10, 2) NULL,
	[Pessimistic] [decimal](10, 2) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Scope] [decimal](10, 2) NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryTracking_KPI]    Script Date: 5/23/2019 12:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[DeliveryTracking_KPI]
        (

   [ProjectID][int] NULL,

   [Vopt][decimal](10, 3) NULL,
	[Vpess] [decimal](10, 3) NULL,
	[V] [decimal](10, 3) NULL,
	[Zero] [int] NOT NULL,

    [DeliveryIteration] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[OutComeTestCaseRef]    Script Date: 5/23/2019 12:56:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[OutComeTestCaseRef]
        (

   [outcome][nvarchar](64) NULL,
	[outcomeid] [tinyint] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessDefects]    Script Date: 5/23/2019 12:56:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[ProcessDefects]
        (

   [ID][int] IDENTITY(1,1) NOT NULL,

  [ProjectID] [int] NOT NULL,

  [CountOfTasksWithoutAssignedto] [int] NULL,
	[EffortOfTasksWithoutAssignedto] [int] NULL,
	[CountOFTasksWithoutActivity] [int] NULL,
	[CountOfstillOpenedInClosedIteration] [int] NULL,
	[CountOfClosedUserStoriesWithoutActivetasks] [int] NULL,
	[CountOfUserStoriesWithoutLinkToFeature] [int] NULL,
	[CountOfTasksWithoutOriginalEst] [int] NULL,
	[CountOfTasksWithoutActualWork] [int] NULL,
	[CountOfUserStoriesWithoutStoryPoints] [int] NULL,
	[KPI] [int] NULL,
	[KPI2] [int] NULL,
	[KPIScore] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[QualityOfEstimates]    Script Date: 5/23/2019 12:56:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[QualityOfEstimates]
        (

   [ProjectID][int] NULL,

   [FeatureSourceId][int] NULL,

   [FeatureName][varchar](1000) NULL,
	[UserStorySourceId] [int] NULL,
	[UserStoryName] [varchar] (1000) NULL,
	[CompletedWork] [decimal](10, 2) NULL,
	[OriginalEstimate] [decimal](10, 2) NULL,
	[RemainingWork] [decimal](10, 2) NULL,
	[CurrentEstimate] [decimal](10, 2) NULL,
	[Actual/Target] [decimal](10, 2) NULL,
	[TrimExt] [decimal](10, 2) NULL,
	[Projection] [decimal](10, 2) NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[QualityOfEstimates_KPI]    Script Date: 5/23/2019 12:56:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[QualityOfEstimates_KPI]
        (

   [ProjectID][int] NOT NULL,

   [Zero] [int] NOT NULL,

   [YellowPand] [int] NOT NULL,

   [GreenPand] [int] NOT NULL,

   [Score] [int] NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[TaskVstsOnline]    Script Date: 5/23/2019 12:56:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[TaskVstsOnline]
        (

   [TaskId][int] IDENTITY(1,1) NOT NULL,

  [TaskSourceId] [int] NULL,
	[ProjectId] [int] NULL,
	[IterationSK] [int] NULL,
	[CreationDate] [datetime] NULL,
	[TaskName] [varchar] (1000) NULL,
	[State] [varchar] (255) NULL,
	[IsActive] [int] NULL,
	[AssignedTo] [varchar] (255) NULL,
	[CompletedWork] [decimal](6, 2) NULL,
	[OriginalEstimate] [decimal](6, 2) NULL,
	[RemainingWork] [decimal](6, 2) NULL,
	[Activity] [varchar] (255) NULL,
	[UserStorySourceId] [int] NULL,
 CONSTRAINT[PK_TaskVstsOnline] PRIMARY KEY CLUSTERED
(
   [TaskId] ASC
)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[TestCase]    Script Date: 5/23/2019 12:56:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[TestCase]
        (

   [Id][int] IDENTITY(1,1) NOT NULL,

  [ProjectID] [int] NOT NULL,

  [AreaID] [int] NOT NULL,

  [TestCaseID] [int] NOT NULL,

  [TestCaseRefID] [int] NOT NULL,

  [Priority] [tinyint]
        NOT NULL,

  [CreationDate] [datetime]
        NOT NULL,

  [LastRefTestRunDate] [datetime]
        NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[TestResult]    Script Date: 5/23/2019 12:56:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[TestResult]
        (

   [ID][int] IDENTITY(1,1) NOT NULL,

  [ProjectID] [int] NOT NULL,

  [TestRunID] [int] NOT NULL,

  [TestCaseRefID] [int] NOT NULL,

  [LastUpdated] [datetime]
        NOT NULL,

  [OutCome] [tinyint]
        NOT NULL,

  [State] [tinyint]
        NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[TestSuite]    Script Date: 5/23/2019 12:56:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[TestSuite]
        (

   [ID][int] IDENTITY(1,1) NOT NULL,

  [ProjectID] [int] NOT NULL,

  [SuiteID] [int] NOT NULL,

  [PlanID] [int] NOT NULL,

  [ParentSuiteID] [int] NOT NULL,

  [Title] [nvarchar] (256) NOT NULL,

   [lastupdated] [datetime]
        NOT NULL,

   [Status] [nvarchar] (256) NOT NULL,

    [isDeleted] [bit]
        NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[TestSuiteLink]    Script Date: 5/23/2019 12:56:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[TestSuiteLink]
        (

   [ID][int] IDENTITY(1,1) NOT NULL,

  [ProjectID] [int] NOT NULL,

  [PartitionId] [int] NOT NULL,

  [SuiteId] [int] NOT NULL,

  [SequenceNumber] [int] NOT NULL,

  [TestCaseId] [int] NOT NULL,

  [ChildSuiteId] [int] NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [dbo].[TestSuiteProgressStatus]    Script Date: 5/23/2019 12:56:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[dbo].[TestSuiteProgressStatus]
        (

   [projectName][nvarchar](256) NOT NULL,

  [SuiteID] [int] NOT NULL,

  [Title] [nvarchar] (256) NOT NULL,

   [NumberOfTestCases] [int] NULL,
	[Passed] [int] NULL,
	[Failed] [int] NOT NULL,

    [NotApplicable] [int] NOT NULL,

    [Blocked] [int] NOT NULL,

    [Paused] [int] NOT NULL,

    [In progress] [int] NOT NULL,

    [Active] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[Bug]    Script Date: 5/23/2019 12:56:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[Bug]
        (

   [BugSourceId][int] NULL,

   [ProjectId][int] NULL,

   [IterationSK][int] NULL,

   [CreationDate][datetime] NULL,

   [BugName][varchar](1000) NULL,
	[State] [varchar] (255) NULL,
	[IsActive] [int] NULL,
	[AssignedTo] [varchar] (255) NULL,
	[CompletedWork] [decimal](6, 2) NULL,
	[OriginalEstimate] [decimal](6, 2) NULL,
	[RemainingWork] [decimal](6, 2) NULL,
	[Activity] [varchar] (255) NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[Epic]    Script Date: 5/23/2019 12:56:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[Epic]
        (

   [EpicSourceId][int] NULL,

   [ProjectId][int] NULL,

   [IterationSK][int] NULL,

   [CreationDate][datetime] NULL,

   [EpicName][varchar](1000) NULL,
	[State] [varchar] (255) NULL,
	[IsActive] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[Feature]    Script Date: 5/23/2019 12:56:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[Feature]
        (

   [FeatureSourceId][int] NULL,

   [ProjectId][int] NULL,

   [IterationSK][int] NULL,

   [CreationDate][datetime] NULL,

   [FeatureName][varchar](1000) NULL,
	[State] [varchar] (255) NULL,
	[IsActive] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[Iteration]    Script Date: 5/23/2019 12:56:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[Iteration]
        (

   [IterationSK][int] NULL,

   [ProjectId][int] NOT NULL,

   [IterationName] [nvarchar] (200) NOT NULL,

    [Depth] [int] NULL,
	[LastUpdatedDateTime] [datetime] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Scope] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[Task]    Script Date: 5/23/2019 12:56:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[Task]
        (

   [TaskSourceId][int] NULL,

   [ProjectId][int] NULL,

   [IterationSK][int] NULL,

   [CreationDate][datetime] NULL,

   [TaskName][varchar](1000) NULL,
	[State] [varchar] (255) NULL,
	[IsActive] [int] NULL,
	[AssignedTo] [varchar] (255) NULL,
	[CompletedWork] [decimal](6, 2) NULL,
	[OriginalEstimate] [decimal](6, 2) NULL,
	[RemainingWork] [decimal](6, 2) NULL,
	[Activity] [varchar] (255) NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[TestCase]    Script Date: 5/23/2019 12:56:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[TestCase]
        (

   [ProjectID][int] NOT NULL,

   [AreaID] [int] NOT NULL,

   [TestCaseID] [int] NOT NULL,

   [TestCaseRefID] [int] NOT NULL,

   [Priority] [tinyint]
        NOT NULL,

   [CreationDate] [datetime]
        NOT NULL,

   [LastRefTestRunDate] [datetime]
        NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[TestResult]    Script Date: 5/23/2019 12:56:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[TestResult]
        (

   [ProjectID][int] NOT NULL,

   [TestRunID] [int] NOT NULL,

   [TestCaseRefID] [int] NOT NULL,

   [LastUpdated] [datetime]
        NOT NULL,

   [OutCome] [tinyint]
        NOT NULL,

   [State] [tinyint]
        NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[TestSuite]    Script Date: 5/23/2019 12:56:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[TestSuite]
        (

   [ProjectID][int] NOT NULL,

   [SuiteID] [int] NOT NULL,

   [PlanID] [int] NOT NULL,

   [ParentSuiteID] [int] NOT NULL,

   [Title] [nvarchar] (256) NOT NULL,

    [lastupdated] [datetime]
        NOT NULL,

    [Status] [nvarchar] (256) NOT NULL,
 
     [isDeleted] [bit]
        NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[TestSuiteLink]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[TestSuiteLink]
        (

   [ProjectID][int] NOT NULL,

   [PartitionId] [int] NOT NULL,

   [SuiteId] [int] NOT NULL,

   [SequenceNumber] [int] NOT NULL,

   [TestCaseId] [int] NOT NULL,

   [ChildSuiteId] [int] NOT NULL
) ON[PRIMARY]
GO
/****** Object:  Table [ETL].[UserStory]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE[ETL].[UserStory]
        (

   [UserStorySourceId][int] NULL,

   [ProjectId][int] NULL,

   [IterationSK][int] NULL,

   [CreationDate][datetime] NULL,

   [UserStoryName][varchar](1000) NULL,
	[State] [varchar] (50) NULL,
	[AssignedTo] [varchar] (100) NULL,
	[StoryPoints] [int] NULL,
	[IsActive] [int] NULL
) ON[PRIMARY]
GO
/****** Object:  StoredProcedure [Confg].[RegisterNewonPremProject]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc[Confg].[RegisterNewonPremProject]
        @CollectionName NVARCHAR(300) Null,  
@ReleaseName NVARCHAR(300) Null,  
@ProjectName NVARCHAR(300) Null,  
@AccountID INT,
@ProjectType INT ,
@UserName NVARCHAR(200) Null,
@Password NVARCHAR(200) Null,
@URL NVARCHAR(500) Null,
@AuthType INT NULL,
@WorkspaceId NVARCHAR(250) Null,
@ProjectStatus INT OUTPUT ,
@projectId INT OUTPUT
as  

-- OnPrim
--********
Begin
Declare @ProjectTypeInternal int
Set @ProjectTypeInternal = @ProjectType
IF(@ProjectTypeInternal = 1)
Begin
IF OBJECT_ID('tempdb.dbo.##T', 'U') IS NOT NULL
 DROP TABLE tempdb.dbo.##T;  

Declare @SQL1 NVARCHAR(MAX)
SET @SQL1 = N'  
select*
into tempdb.dbo.##T  
from[10.1.22.94].[Tfs_Warehouse].[dbo].[DimTeamProject] where ProjectPath like ''%'+@CollectionName+'%'+@ReleaseName+'%''  
'  
Exec sp_executesql  @SQL1

Declare @ReleaseSourceId NVARCHAR(MAX)
set @ReleaseSourceId = (select AreaSK from  [10.1.22.94].[Tfs_Warehouse].[dbo].DimArea
where ProjectGUID = (select top 1 convert(nvarchar(max), ProjectNodeGUID) from ##T) )  
--Select isnull(@ReleaseSourceId,3)
-- Case One
--**********
Declare @Status INT
Set @Status = (select Count(1) from project where ProjectName = @ProjectName and ProjectAreaPath = Concat('\',@CollectionName,'\',@ReleaseName ) ) 
IF(@Status = 0 and @ReleaseSourceId is not null)
Begin
INSERT INTO Project
Select
ParentNodeSK as ParentSourceId,
ProjectNodeSK as ProjectSourceId,
@ReleaseSourceId as ReleaseSourceId,
@ProjectName as ProjectName,
Getdate() as CreationDate,
1 as IsActive,
Concat('\',@CollectionName,'\',@ReleaseName )   as ProjectAreaPath ,  
@AccountID as AccountId,
Null as ParentId,
Null as ParentName,
'OnPrim' as ProjectType,
@UserName as UserName,
@Password as Password,
@AuthType as AuthType,
@WorkspaceId as WorkspaceId
from tempdb.dbo.##T  
select @projectID = ProjectId, @ProjectStatus = 1 from Project where ProjectName = @ProjectName--and ProjectAreaPath = @URL
select @projectID as ProjectID, @ProjectStatus as ProjectStatus
Print 'Project Was Added Successfully'
End
-- Case two
-- * *********
else if  (@Status > 0 and @ReleaseSourceId is not null)
 Begin
select  @projectID = 0
select @ProjectStatus = 2--from Project
select @projectID as ProjectID, @ProjectStatus as ProjectStatus
Print 'Project Was Registered Before'
END

else if  (@Status = 0 and @ReleaseSourceId is null)
Begin
select  @projectID = 0  
select @ProjectStatus = 3--from Project
select @projectID as ProjectID , @ProjectStatus as ProjectStatus
Print 'Project Was Not Found'
END
     END
           
               
-- OnLine
--********

 Else IF(@ProjectTypeInternal = 2)
Begin
Declare @Status2 INT
Set @Status2 = (select Count(1) from project where ProjectName = @ProjectName and ProjectAreaPath = @URL  ) 
IF @Status2 = 0
     Begin
INSERT INTO Project
Select
Null as ParentSourceId ,   
Null as ProjectSourceId ,   
Null as ReleaseSourceId ,   
@ProjectName as ProjectName ,   
Getdate() as CreationDate,   
1 as IsActive ,   
@URL   as ProjectAreaPath ,  
@AccountID as AccountId ,  
Null as ParentId ,  
Null as ParentName  ,
'Online' as ProjectType	,
@UserName as UserName ,
@Password as Password ,
@AuthType as AuthType ,
@WorkspaceId as WorkspaceId

select  @projectID = ProjectId,@ProjectStatus = 1 from Project where ProjectName = @ProjectName and ProjectAreaPath = @URL
select @projectID as ProjectID , @ProjectStatus as ProjectStatus
Print 'Project Was Added Successfully'
     END

   
   else -- @Status2 > 0
   Begin
select  @projectID = 0  
select @ProjectStatus = 2--from Project
select @projectID as ProjectID , @ProjectStatus as ProjectStatus
Print 'Project Was Registered Before'
   End
End
END

-- select* from project
GO
/****** Object:  StoredProcedure [dbo].[CROPS_ChangeRequest]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE[dbo].[CROPS_ChangeRequest]
as
;With CTE1(ProjectID , IterationSk , IterationName , OriginalEffort, ChangeRequestEffort)
as
(
SELECT I.ProjectID , I.IterationSourceID ,I.IterationName ,0 as OriginalEffort ,0 as ChangeRequestEffort
FROM Iteration as I left JOIN[UserStory] as US ON I.IterationSourceID = US.IterationSk and I.ProjectId = US.ProjectId
                Left JOIN[Task] as T ON T.UserStorySourceId = US.UserStorySourceId and  T.ProjectId = US.ProjectId
Where I.depth = 1 and US.IsCR = 0 and I.ProjectID = 2
Group BY I.ProjectID , I.IterationSourceID , I.IterationName
) --select* from CTE1
MERGE[dbo].[ChangeRequest]
        AS TARGET
USING CTE1  AS SOURCE
ON(TARGET.[ProjectId] = SOURCE.[ProjectId] AND TARGET.IterationSk = SOURCE.IterationSk)
WHEN MATCHED AND(TARGET.ProjectId = SOURCE.ProjectID AND TARGET.IterationSk = SOURCE.IterationSk)
OR TARGET.ProjectID<> SOURCE.ProjectID
OR TARGET.IterationSk<> SOURCE.IterationSk
OR TARGET.IterationName<> SOURCE.IterationName
 THEN
UPDATE SET
  TARGET.ProjectID = SOURCE.ProjectID
, TARGET.IterationSk = SOURCE.IterationSk
, TARGET.IterationName = SOURCE.IterationName
WHEN NOT MATCHED BY TARGET THEN


INSERT (ProjectID, IterationSk, IterationName, OriginalEffort, ChangeRequestEffort)
VALUES (SOURCE.ProjectID, SOURCE.IterationSk, SOURCE.IterationName,0,0);    

;With CTE2(ProjectID , IterationSk , IterationName , ChangeRequestEffort)
as
(
SELECT I.ProjectID , I.IterationSourceID ,I.IterationName , 
       SUM(T.RemainingWork + T.CompletedWork) as ChangeRequestEffort
FROM Iteration as I INNER JOIN[UserStory] as US ON I.IterationSourceID = US.IterationSk and I.ProjectId = US.ProjectId
                INNER JOIN[Task] as T ON T.UserStorySourceId = US.UserStorySourceId and  T.ProjectId = US.ProjectId
Where US.IsCR = 1
Group BY I.ProjectID , I.IterationSourceID ,I.IterationName
)
Update CR
Set CR.ChangeRequestEffort = CTE2.ChangeRequestEffort
FROM [ChangeRequest] as CR , CTE2
Where CR.ProjectId = CTE2.ProjectID AND CR.IterationSk = CTE2.IterationSk
-- * *****************************************************
; With CTE2(ProjectID , IterationSk , IterationName , OriginalEffort)
as
(
SELECT I.ProjectID , I.IterationSourceID ,I.IterationName , 
       SUM(T.RemainingWork + T.CompletedWork) as OriginalEffort
FROM Iteration as I INNER JOIN[UserStory] as US ON I.IterationSourceID = US.IterationSk and I.ProjectId = US.ProjectId
                INNER JOIN[Task] as T ON T.UserStorySourceId = US.UserStorySourceId and  T.ProjectId = US.ProjectId
Where US.IsCR = 0
Group BY I.ProjectID , I.IterationSourceID ,I.IterationName
)
Update CR
Set CR.OriginalEffort = CTE2.OriginalEffort
FROM [ChangeRequest] as CR , CTE2
Where CR.ProjectId = CTE2.ProjectID AND CR.IterationSk = CTE2.IterationSk
-- * *****************************************************
Update CR
set AVG = (ChangeRequestEffort / (ChangeRequestEffort + OriginalEffort))
From[ChangeRequest] as CR
Where (ChangeRequestEffort<> 0 or OriginalEffort <> 0)

-- AVG ChangeRequest_KPI
--*****************************
; WITH CTE2(ProjectID, Zero, YellowPand, GreenPand, Score)
as
(
SELECT ProjectID,0 as Zero,67 as YellowPand,33 as GreenPand  ,Ceiling((AVG([AVG]))*100) as Score FROM ChangeRequest
GROUP BY ProjectID
 ) --select* from CTE2

MERGE[dbo].[ChangeRequest_KPI]
        AS TARGET
USING CTE2  AS SOURCE
ON(TARGET.[ProjectId] = SOURCE.[ProjectId])
WHEN MATCHED AND(TARGET.ProjectId = SOURCE.ProjectID)
OR TARGET.ProjectID<> SOURCE.ProjectID
OR TARGET.Score<> SOURCE.Score
 THEN
UPDATE SET
  TARGET.ProjectID = SOURCE.ProjectID
, TARGET.Score = SOURCE.Score

WHEN NOT MATCHED BY TARGET THEN


INSERT (ProjectID, Zero, YellowPand, GreenPand, Score)
VALUES (SOURCE.ProjectID, SOURCE.Zero, SOURCE.YellowPand, SOURCE.GreenPand, SOURCE.Score);

        GO
        /****** Object:  StoredProcedure [dbo].[CROPS_DeliveryTracking]    Script Date: 5/23/2019 12:56:55 PM ******/
        SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE[dbo].[CROPS_DeliveryTracking]
        @ProjectID INT
AS  
--begin  
--IF OBJECT_ID('dbo.BurnUp_Info', 'U') IS NOT NULL   
--  TRUNCATE TABLE BurnUp_Info;   
--IF OBJECT_ID('dbo.KPI', 'U') IS NOT NULL   
--  TRUNCATE TABLE dbo.KPI;   
--END  
  
-- BurnUp Table   
--**************  
DECLARE @BurnUp TABLE
(
    BurnupID INT IDENTITY(1,1)   ,  
 ProjectID INT,
 IterationID INT ,  
    Iteration Nvarchar(50)  NULL,  
    Actual decimal (10,2)  NULL,  
 Accumulative decimal (10,2)  NULL,  
 Slope decimal (10,2)  NULL,  
 Optimistic decimal (10,2)  NULL,  
 Pessimistic decimal (10,2)  NULL,  
 StartDate DATETIME NULL ,  
 EndDate DATETIME NULL ,  
 Scope decimal (10,2)  NULL  
);  
  
INSERT INTO @BurnUp
SELECT  projectid,IterationSourceID,IterationName,NUll,NUll,NUll,NUll,NUll,StartDate , EndDate,NUll from Iteration where projectid = @projectid and Depth != 0  
  
-- Actual  
--**************  
;With CTE1(ProjectID, IterationSK, StoryPoints )
as  
(SELECT ProjectID, IterationSK, Sum(StoryPoints)
   FROM userstory
   WHERE[State] = 'Closed' and projectid = @projectid
 GROUP BY ProjectID,IterationSK)  
Update B
set B.Actual = C.StoryPoints
From @BurnUp as B INNER JOIN CTE1 as C
ON B.ProjectID = C.ProjectId and B.IterationID = C.IterationSK

-- Accumulative
--**************  
; WITH CTE2(ProjectID, IterationSK, Accumulative)
as  
(
SELECT distinct ProjectID, IterationSK,
--CASE  
--     WHEN StoryPoints = 0 THEN 0  
   --  ELSE
     CAST(SUM(StoryPoints) OVER (ORDER BY IterationSK) AS DECIMAL(10,2))  
    -- END
     AS Accumulative
     FROM userstory
     Where projectid = @projectid and[State] = 'Closed' 
)  
Update B
set B.Accumulative = C.Accumulative
From @BurnUp as B INNER JOIN CTE2 as C
ON B.ProjectID = C.ProjectId and B.IterationID = C.IterationSK
--Slope
--*********  
--Actual Sum(story points)/number of actual Iterations
DECLARE @ActualNumberOfIterations int
SET @ActualNumberOfIterations = (select COUNT(distinct IterationSK) as ActualNumberOfIterations from UserStory where projectid = @projectid)


DECLARE @SlopeStart decimal (10,3)   
SET @SlopeStart = (select cast(cast(Sum(Storypoints) as decimal(10,2))/@ActualNumberOfIterations as decimal (10,2) ) from UserStory where projectid = @projectid  and State = 'Resolved')  
--select cast(@SlopeStart as decimal(10,2))  
;With CTE3(ProjectID, IterationSK, Slope)
as  
(
  SELECT ProjectID, IterationID as IterationSK, SUM(cast(@SlopeStart as decimal(10,2))) over(order by IterationID )  AS Slope
  FROM @BurnUp
  Where projectid = @projectid   
)  
Update B
set B.Slope = C.Slope
From @BurnUp as B INNER JOIN CTE3 as C
ON B.ProjectID = C.ProjectId and B.IterationID = C.IterationSK
-- Optimistic
--*************  
Declare @OptimisticStart decimal(10,3)
SET @OptimisticStart = (SELECT cast(Scope as decimal(10,2))  / CAST(OptimisticFinalIteration as decimal(10,2)) from ProjectDetails where projectid = @ProjectID )    
-- select @OptimisticStart

; With CTE4(ProjectID, IterationSK, Optimistic)
as  
(
  SELECT ProjectID, IterationID as IterationSK, SUM(cast(@OptimisticStart as decimal(10,2))) over(order by IterationID )  AS Optimistic


  FROM @BurnUp
  Where projectid = @projectid
  --Having Count(IterationID) <=  @OptimisticStart  
)  
Update B
set B.Optimistic = c.Optimistic
From @BurnUp as B INNER JOIN CTE4 as C
ON B.ProjectID = C.ProjectId and B.IterationID = C.IterationSK
WHERE c.Optimistic <= ((select OptimisticFinalIteration from ProjectDetails where projectid = @projectid) * @OptimisticStart)  
--Pessimistic  
--************  
Declare @PessimisticStart decimal (10,3)  
SET @PessimisticStart = (SELECT(cast(Scope as decimal(10, 2)) / CAST(PessimisticFinalIteration as decimal(10, 2))) from ProjectDetails where projectid = @projectid  )  
--select @PessimisticStart
; With CTE5(ProjectID, IterationSK, Pessimistic)
as  
(
  SELECT ProjectID, IterationID as IterationSK, SUM(cast(@PessimisticStart as decimal(10,2))) over(order by IterationID )  AS Pessimistic
  FROM @BurnUp
  Where projectid = @projectid   
)  
Update B
set B.Pessimistic = c.Pessimistic
From @BurnUp as B INNER JOIN CTE5 as C
ON B.ProjectID = C.ProjectId and B.IterationID = C.IterationSK
WHERE c.Pessimistic <= ((select PessimisticFinalIteration from ProjectDetails where projectid = @projectid) * @PessimisticStart)  
  
  
-- Scope  
--*******  
Update B
set B.Scope = I.Scope
From @BurnUp as B INNER JOIN Iteration as I
ON B.ProjectID = I.ProjectId and B.IterationID = I.IterationSourceID
WHERE I.projectid = @projectid
-- Mearge_BurnUP
--**************  
MERGE[dbo].[DeliveryTracking] AS TARGET
USING @BurnUp  AS SOURCE
ON (TARGET.[ProjectId] = SOURCE.[ProjectId] AND TARGET.IterationID = SOURCE.IterationID)
--When records are matched, update       
--the records if there is any change
WHEN MATCHED AND(TARGET.ProjectId = SOURCE.ProjectID AND TARGET.IterationID = SOURCE.IterationID)
OR TARGET.BurnupID<> SOURCE.BurnupID
OR TARGET.Iteration<> SOURCE.Iteration
OR TARGET.Actual<> SOURCE.Actual
OR TARGET.Accumulative<> SOURCE.Accumulative
OR TARGET.Slope<> SOURCE.Slope
OR TARGET.Optimistic<> SOURCE.Optimistic
OR TARGET.Pessimistic<> SOURCE.Pessimistic
OR TARGET.StartDate<> SOURCE.StartDate
OR TARGET.EndDate<> SOURCE.EndDate
OR TARGET.Scope<> SOURCE.Scope
 THEN
UPDATE SET
  TARGET.BurnupID = SOURCE.BurnupID
, TARGET.Iteration = SOURCE.Iteration
, TARGET.Actual = SOURCE.Actual
, TARGET.Accumulative = SOURCE.Accumulative
, TARGET.Slope = SOURCE.Slope
, TARGET.Optimistic = SOURCE.Optimistic
, TARGET.Pessimistic = SOURCE.Pessimistic
, TARGET.StartDate = SOURCE.StartDate
, TARGET.EndDate = SOURCE.EndDate
, TARGET.Scope = SOURCE.Scope


WHEN NOT MATCHED BY TARGET THEN



INSERT (BurnupID, ProjectID, IterationID, Iteration, Actual, Accumulative, Slope, Optimistic, Pessimistic, StartDate, EndDate, Scope)
VALUES (SOURCE.BurnupID, SOURCE.ProjectID, SOURCE.IterationID, SOURCE.Iteration, SOURCE.Actual, SOURCE.Accumulative, SOURCE.Slope, SOURCE.Optimistic, SOURCE.Pessimistic, SOURCE.StartDate, SOURCE.EndDate, SOURCE.Scope);

        Select* from[DeliveryTracking] where ProjectID = @ProjectID
      -- Mearge_KPI  
--**************  
;With CTE6(ProjectID, Vopt, Vpess, V, Zero, DeliveryIteration )
as  
(
SELECT @ProjectID as ProjectID , @OptimisticStart as Vopt , @PessimisticStart as Vpess,@SlopeStart as V, 0 as Zero ,CEILING((cast(Scope as decimal(10,2))/@SlopeStart)) as DeliveryIteration
From ProjectDetails where Projectid = @ProjectID   
)  
MERGE[dbo].[DeliveryTracking_KPI]
        AS TARGET
USING CTE6  AS SOURCE
ON(TARGET.[ProjectId] = SOURCE.[ProjectId])
WHEN MATCHED AND(TARGET.ProjectId = SOURCE.ProjectID)


OR TARGET.Vopt<> SOURCE.Vopt
OR TARGET.Vpess<> SOURCE.Vpess
OR TARGET.V<> SOURCE.V
OR TARGET.Zero<> SOURCE.Zero
OR TARGET.DeliveryIteration<> SOURCE.DeliveryIteration
 THEN
UPDATE SET
  TARGET.Vopt = SOURCE.Vopt
, TARGET.Vpess = SOURCE.Vpess
, TARGET.V = SOURCE.V
, TARGET.Zero = SOURCE.Zero
, TARGET.DeliveryIteration = SOURCE.DeliveryIteration



WHEN NOT MATCHED BY TARGET THEN



INSERT (ProjectID, Vopt, Vpess, V, Zero, DeliveryIteration)
VALUES (SOURCE.ProjectID, SOURCE.Vopt, SOURCE.Vpess, SOURCE.V, SOURCE.Zero, SOURCE.DeliveryIteration);

        GO
        /****** Object:  StoredProcedure [dbo].[CROPS_ProcessDefects]    Script Date: 5/23/2019 12:56:55 PM ******/
        SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC[dbo].[CROPS_ProcessDefects]
        @ProjectID INT
AS   
-- (case1) Count Of Tasks Without Assignee  
--========================================  
BEGIN
Update ProcessDefects
set CountOfTasksWithoutAssignedto =   
(
Select Count(t.TaskId) CountTasksWithoutAssignedto from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
where t.state<> 'new' and t.Assignedto = 'NOT Assigned' and t.ProjectID = @ProjectID   and I.Depth<> 0  
)
where ProjectID = @ProjectID   
/*  
Checker  
========  
Select Count(*) CountTasksWithoutAssignedto from ETL.task where state <> 'new' and Assignedto = 'NOT Assigned' and ProjectID = @ProjectID   and [IterationSK] <> '10952'  
*/  
--############################################################################################################################################################################  
-- (case 2) Count Tasks Without Activity  
--===================================  
Update ProcessDefects
set CountOFTasksWithoutActivity =
(
Select Count(t.TaskId) CountTasksWithoutActivity from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
where t.State<> 'new' and t.Activity is null and t.ProjectID = @ProjectID  and I.Depth<> 0 and I.StartDate <= getdate()
)
where ProjectID = @ProjectID   
/*  
Checker  
=======  
Select Count(t.TaskSourceId) CountTasksWithoutActivity from etl.task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK  
where t.State <> 'new' and t.Activity is null and t.ProjectID = @ProjectID  and I.Depth <> 0  
*/  
--############################################################################################################################################################################  
-- (case 3) Count Of Opened Tasks In Past Iteration  
--===================================================  
Update ProcessDefects
Set CountOfstillOpenedInClosedIteration =
 (
 select count(t.TaskId)   from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
 where t.state<> 'Closed'  and t.ProjectID = @ProjectID and I.StartDate<= getdate() and I.Depth<> 0  
 )
where ProjectID = @ProjectID   
--############################################################################################################################################################################  
-- (case 4) Count Of Closed UserStories Without Active tasks  
--=========================================================  
Update ProcessDefects
Set CountOfClosedUserStoriesWithoutActivetasks =
(
select Count(*) from
                   (
                      select t.* from userstory as U , task as t
       where  u.state = 'Closed' and u.projectid = t.projectid and
              t.UserStorySourceId = u.UserStorySourceId  and t.ProjectID = @ProjectID
       )
as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
where t.state<> 'closed' and I.Depth<> 0  and I.StartDate<= getdate()
)
Where ProjectID = @ProjectID
--############################################################################################################################################################################  
--(case 5) User stories not linked to any feature  
--************************************  
Update ProcessDefects
Set CountOfUserStoriesWithoutLinkToFeature =
(
    select Count(u.UserStoryId) from userstory as U inner join Iteration as I on I.IterationSourceID = U.IterationSK
 where u.FeatureSourceId is null and u.ProjectID = @ProjectID and I.Depth<> 0 and I.StartDate<= getdate()
)
Where ProjectID = @ProjectID
--############################################################################################################################################################################  
--(case 6) Tasks Without Estimets  
--=================================  
Update ProcessDefects
Set CountOfTasksWithoutOriginalEst = (
                                   select Count(t.TaskId) from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
                                   where t.OriginalEstimate = 0 and t.state<> 'new' and t.ProjectID = @ProjectID    and I.Depth<> 0 and I.StartDate<= getdate()
)
Where ProjectID = @ProjectID
--############################################################################################################################################################################  
--(case 7) Missing completed work  
--===================================  
Update ProcessDefects
Set CountOfTasksWithoutActualWork =
                                   (
           select Count(t.TaskId) from task as t inner join Iteration as I on I.IterationSourceID = t.IterationSK
           where t.CompletedWork = 0 and t.state<> 'new' and t.ProjectID = @ProjectID   and I.Depth<> 0 and I.StartDate<= getdate()
           )
Where ProjectID = @ProjectID
--############################################################################################################################################################################  
--(case 8) Missing story points  
-- ==============================  
  
Update ProcessDefects
Set CountOfUserStoriesWithoutStoryPoints =
                                        (
           SELECT Count(u.UserStoryId) FROM userstory as U inner join Iteration as I on I.IterationSourceID = U.IterationSK
           WHERE u.StoryPoints = 0 and u.STATE<> 'new' and u.ProjectID = @ProjectID    and I.Depth<> 0 and I.StartDate<= getdate()
          )
Where ProjectID = @ProjectID
END  
;With CTE(KPIScore)
AS
(
SELECT CountOfTasksWithoutAssignedto as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOFTasksWithoutActivity as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOfstillOpenedInClosedIteration as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOfClosedUserStoriesWithoutActivetasks as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOfUserStoriesWithoutLinkToFeature as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOfTasksWithoutOriginalEst as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOfTasksWithoutActualWork as KPIScore from ProcessDefects where ProjectID = @ProjectID
union ALL
SELECT CountOfUserStoriesWithoutStoryPoints as KPIScore from ProcessDefects where ProjectID = @ProjectID
)
UPDATE ProcessDefects
SET KPIScore = (SELECT Count(*) FROM CTE where KPIScore = 0),
    KPI2 = 1
where ProjectID = @ProjectID


GO
/****** Object:  StoredProcedure [dbo].[CROPS_QualityOfEstimates]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE[dbo].[CROPS_QualityOfEstimates]
as
;With CTE1(ProjectID , FeatureSourceId , FeatureName , UserStorySourceId, UserStoryName , CompletedWork, OriginalEstimate, RemainingWork , CurrentEstimate)
as
(
SELECT F.ProjectID , F.FeatureSourceId ,F.FeatureName , US.UserStorySourceId, US.UserStoryName , 
       SUM(T.CompletedWork) as CompletedWork,SUM(T.OriginalEstimate) as OriginalEstimate,
	   SUM(T.RemainingWork)as RemainingWork ,SUM(T.RemainingWork + T.CompletedWork) as CurrentEstimate
FROM Feature as F INNER JOIN[UserStory] as US ON F.FeatureSourceId = US.FeatureSourceId and F.ProjectId = US.ProjectId
                INNER JOIN[Task] as T ON T.UserStorySourceId = US.UserStorySourceId and  T.ProjectId = US.ProjectId
--WHERE F.ProjectId = @ProjectId and US.ProjectId = @ProjectId and T.ProjectId = @ProjectId
Group BY F.ProjectID , F.FeatureSourceId , F.FeatureName, US.UserStorySourceId, US.UserStoryName
)
MERGE[dbo].[QualityOfEstimates]
        AS TARGET
USING CTE1  AS SOURCE
ON(TARGET.[ProjectId] = SOURCE.[ProjectId] AND TARGET.FeatureSourceId = SOURCE.FeatureSourceId and TARGET.UserStorySourceId = SOURCE.UserStorySourceId)
WHEN MATCHED AND(TARGET.ProjectId = SOURCE.ProjectID AND TARGET.FeatureSourceId = SOURCE.FeatureSourceId and TARGET.UserStorySourceId = SOURCE.UserStorySourceId)
OR TARGET.ProjectID<> SOURCE.ProjectID
OR TARGET.FeatureSourceId<> SOURCE.FeatureSourceId
OR TARGET.FeatureName<> SOURCE.FeatureName
OR TARGET.UserStorySourceId<> SOURCE.UserStorySourceId
OR TARGET.UserStoryName<> SOURCE.UserStoryName
OR TARGET.CompletedWork<> SOURCE.CompletedWork
OR TARGET.OriginalEstimate<> SOURCE.OriginalEstimate
OR TARGET.RemainingWork<> SOURCE.RemainingWork
OR TARGET.CurrentEstimate<> SOURCE.CurrentEstimate
 THEN
UPDATE SET
  TARGET.ProjectID = SOURCE.ProjectID
, TARGET.FeatureSourceId = SOURCE.FeatureSourceId
, TARGET.FeatureName = SOURCE.FeatureName
, TARGET.UserStorySourceId = SOURCE.UserStorySourceId
, TARGET.UserStoryName = SOURCE.UserStoryName
, TARGET.CompletedWork = SOURCE.CompletedWork
, TARGET.OriginalEstimate = SOURCE.OriginalEstimate
, TARGET.RemainingWork = SOURCE.RemainingWork
, TARGET.CurrentEstimate = SOURCE.CurrentEstimate


WHEN NOT MATCHED BY TARGET THEN


INSERT (ProjectID, FeatureSourceId, FeatureName, UserStorySourceId, UserStoryName, CompletedWork, OriginalEstimate, RemainingWork, CurrentEstimate)
VALUES (SOURCE.ProjectID, SOURCE.FeatureSourceId, SOURCE.FeatureName, SOURCE.UserStorySourceId, SOURCE.UserStoryName, SOURCE.CompletedWork, SOURCE.OriginalEstimate,
        SOURCE.RemainingWork, SOURCE.CurrentEstimate);         
-- [Projection]
--**************

Update[QualityOfEstimates]
SET[Actual/Target] = CompletedWork / OriginalEstimate
   ,[TrimExt] = CASE WHEN[Actual/Target] > 2 THEN 2 ELSE[Actual/Target] END
    ,[Projection] = CASE WHEN[TrimExt] > 1 THEN 2-([TrimExt]) ELSE[TrimExt] END

-- AVG QualityOfEstimates_KPI
--*****************************
; WITH CTE2(ProjectID, Zero, YellowPand, GreenPand, Score)
as
(
SELECT ProjectID,0 as Zero,70 as YellowPand,90 as GreenPand  ,Ceiling((AVG([Projection]))*100) as Score FROM QualityOfEstimates
GROUP BY ProjectID
 ) --select* from CTE2

MERGE[dbo].[QualityOfEstimates_KPI]
        AS TARGET
USING CTE2  AS SOURCE
ON(TARGET.[ProjectId] = SOURCE.[ProjectId])
WHEN MATCHED AND(TARGET.ProjectId = SOURCE.ProjectID)
OR TARGET.ProjectID<> SOURCE.ProjectID
OR TARGET.Score<> SOURCE.Score
 THEN
UPDATE SET
  TARGET.ProjectID = SOURCE.ProjectID
, TARGET.Score = SOURCE.Score

WHEN NOT MATCHED BY TARGET THEN


INSERT (ProjectID, Zero, YellowPand, GreenPand, Score)
VALUES (SOURCE.ProjectID, SOURCE.Zero, SOURCE.YellowPand, SOURCE.GreenPand, SOURCE.Score);

        GO
        /****** Object:  StoredProcedure [dbo].[CROPS_UpdateMasterListRelation]    Script Date: 5/23/2019 12:56:55 PM ******/
        SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc[dbo].[CROPS_UpdateMasterListRelation]
        @ProjectID Int
as
BEGIN
Declare @ParentSourceId[INT]
SET     @ParentSourceId  = (SELECT ParentSourceId from Project where ProjectID  = @ProjectID) 

Declare @projectSourceId[INT]
SET @projectSourceId = (SELECT projectSourceId from Project where ProjectID  = @ProjectID)

Declare @ReleaseSourceId[INT]
SET @ReleaseSourceId = (SELECT ReleaseSourceId from Project where ProjectID  = @ProjectID)
END
BEGIN
; With CTE1(ProjectSourceID, FeatureSourceID, UserStorySourceID)
as
(
SELECT
    [DimWorkItemFiltered].TeamProjectSK as ProjectSourceID ,
   -- FLCWI.SourceWorkItemSK AS FeatureSourceSK,
	[DimWorkItemFiltered].system_id AS FeatureSourceID,
	--FLCWI.TargetWorkItemSK AS UserStorySourceSK,
    Children.system_id AS UserStorySourceID
  FROM(select* from [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem]
      WHERE [DimWorkItem].System_RevisedDate = CONVERT(datetime, '9999', 126)

      AND[DimWorkItem].System_IsDeleted = 0

      AND[DimWorkItem].TeamProjectCollectionSK = @ParentSourceId

      AND[DimWorkItem].TeamProjectSK = @projectSourceId

      AND[DimWorkItem].AreaSK = @ReleaseSourceId

      AND[DimWorkItem].System_WorkItemType= 'Feature') AS[DimWorkItemFiltered]
  INNER JOIN[10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem]
        AS FLCWI
    ON FLCWI.SourceWorkItemSK = [DimWorkItemFiltered].WorkItemSK
        AND FLCWI.WorkItemLinkTypeSK = 28

    INNER JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as Children
    ON FLCWI.TargetWorkItemSK = Children.WorkItemSK
    )
	-- Select* from CTE1
   Update US
   set US.FeatureSourceId = CTE1.FeatureSourceID
   From UserStory as US , CTE1
   Where US.UserStorySourceID = CTE1.UserStorySourceID
END
--***************************************************************************************
BEGIN
;With CTE2(ProjectID, UserStorySourceID, TaskOrBugSourceID)
as
(
SELECT
    [DimWorkItemFiltered].TeamProjectSK as ProjectID,
    --FLCWI.SourceWorkItemSK AS UserStorySourceSK,
	[DimWorkItemFiltered].system_id AS UserStorySourceID,
	--FLCWI.TargetWorkItemSK AS TaskOrBugSourceSK,
    Children.system_id AS TaskOrBugSourceID
  FROM(select* from [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem]
      WHERE [DimWorkItem].System_RevisedDate = CONVERT(datetime, '9999', 126)

      AND[DimWorkItem].System_IsDeleted = 0

      AND[DimWorkItem].TeamProjectCollectionSK = @ParentSourceId

      AND[DimWorkItem].TeamProjectSK = @projectSourceId

      AND[DimWorkItem].AreaSK = @ReleaseSourceId

      AND[DimWorkItem].System_WorkItemType= 'User Story') AS[DimWorkItemFiltered]
  INNER JOIN[10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem]
        AS FLCWI
    ON FLCWI.SourceWorkItemSK = [DimWorkItemFiltered].WorkItemSK
        AND FLCWI.WorkItemLinkTypeSK = 28

    INNER JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as Children
    ON FLCWI.TargetWorkItemSK = Children.WorkItemSK
    )
	--Select distinct * from CTE2

    Update T

    set T.UserStorySourceId = CTE2.UserStorySourceId
    From Task as T , CTE2
    Where T.TaskSourceID = CTE2.TaskOrBugSourceID
END
GO
/****** Object:  StoredProcedure [dbo].[--QOE_Feature_UnderDevelopment]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Proc [dbo].[--QOE_Feature_UnderDevelopment]
as
  WITH CTE (Poject_SourceID, FeatureSourceID, UserStorySourceID, TaskSourceID)
  AS (SELECT DISTINCT
    DWI.TeamProjectSK,
    VLCWI1.TargetWorkItemSK AS FeatureSourceID,
    VLCWI2.TargetWorkItemSK AS UserStorySourceID,
    VLCWI3.TargetWorkItemSK AS TaskSourceID
  FROM [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] AS DWI
  INNER JOIN
       [10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem] AS VLCWI1
    ON VLCWI1.SourceWorkItemSK = DWI.WorkItemSK
  RIGHT JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem] AS VLCWI2
    ON VLCWI2.SourceWorkItemSK = VLCWI1.TargetWorkItemSK
  RIGHT JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem] AS VLCWI3
    ON VLCWI3.SourceWorkItemSK = VLCWI2.TargetWorkItemSK
    AND VLCWI2.WorkItemLinkTypeSK = 28
    AND VLCWI3.WorkItemLinkTypeSK = 28
  WHERE DWI.System_RevisedDate = CONVERT(datetime, '9999', 126)
  AND DWI.System_IsDeleted = 0
  AND VLCWI2.WorkItemLinkTypeSK = 28
  AND DWI.TeamProjectSK = 1176) --select* from cte
 SELECT
   2 as ProjectID ,
    F.FeatureName,
    Cte.Poject_SourceID,
    --Cte.FeatureSourceID,
    Cte.FeatureSourceID,
    --Cte2.TaskSourceID,
    SUM(Cte2.CompletedWork) AS CompletedWork,
    SUM(Cte2.OriginalEstimate) AS OriginalEstimate,
    SUM(ISNULL(Cte2.RemainingWork, 0))AS RemainingWork,
     SUM(Cte2.CompletedWork + ISNULL(Cte2.RemainingWork, 0) ) AS CurrentEstimate
   -- GETDATE() AS last_Run
  -- into QOE_Feature
  FROM Cte
  INNER JOIN(SELECT
     1176 AS Poject_SourceID,
    DWI.WorkItemSK AS TaskSourceID,
    CompletedWork AS CompletedWork,
    OriginalEstimate AS OriginalEstimate,
    RemainingWork AS RemainingWork
  FROM [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] AS DWI
  INNER JOIN [Project] AS P
     ON p.ProjectSourceId = DWI.TeamProjectSK
  INNER JOIN [Task] AS T
   ON P.ProjectId = T.ProjectId
    AND T.TaskSourceId = DWI.system_ID
  WHERE  --  DWI.System_State = 'Closed'  and
  DWI.System_RevisedDate = CONVERT(datetime, '9999', 126)
  AND DWI.System_IsDeleted = 0
  AND DWI.TeamProjectSK = 1176
  ) AS Cte2
    ON Cte.Poject_SourceID = Cte2.Poject_SourceID
    AND Cte.TaskSourceID = Cte2.TaskSourceID
  INNER JOIN
  (SELECT distinct 1176 as Poject_SourceID , DWI.WorkItemSK as FeatureSourceID, F.FeatureName
    FROM             [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as DWI
    INNER JOIN[Project] AS P ON p.ProjectSourceId = DWI.TeamProjectSK

    INNER JOIN   [Feature] AS F ON P.projectid = F.ProjectId   AND F.FeatureSourceID = DWI.system_ID
    WHERE DWI.System_RevisedDate = CONVERT(datetime, '9999', 126) and p.ProjectSourceId = 1176) AS F

    ON F.FeatureSourceID = Cte.FeatureSourceID and F.Poject_SourceID = CTE.Poject_SourceID
  GROUP BY  F.FeatureName, Cte.Poject_SourceID, Cte.FeatureSourceID--, GETDATE()
GO
/****** Object:  StoredProcedure [dbo].[--QOE_Userstory_UnderDevelopment]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  Create Proc [dbo].[--QOE_Userstory_UnderDevelopment]
  as
  WITH CTE (Poject_SourceID, FeatureSourceID, UserStorySourceID, TaskSourceID)
  AS (
SELECT DISTINCT
    DWI.TeamProjectSK,
    VLCWI1.TargetWorkItemSK AS FeatureSourceID,
    VLCWI2.TargetWorkItemSK AS UserStorySourceID,
    VLCWI3.TargetWorkItemSK AS TaskSourceID
  FROM [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] AS DWI
  --INNER JOIN [Project] AS P
   -- ON p.ProjectSourceId = DWI.TeamProjectSK--P.ParentSourceId = DWI.TeamProjectCollectionSK and --and DWI.Areask = P.ReleaseSourceId
  RIGHT JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem] AS VLCWI1
    ON VLCWI1.SourceWorkItemSK = DWI.WorkItemSK
  RIGHT JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem] AS VLCWI2
    ON VLCWI2.SourceWorkItemSK = VLCWI1.TargetWorkItemSK
  RIGHT JOIN [10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem] AS VLCWI3
    ON VLCWI3.SourceWorkItemSK = VLCWI2.TargetWorkItemSK
    AND VLCWI2.WorkItemLinkTypeSK = 28
    AND VLCWI3.WorkItemLinkTypeSK = 28
  WHERE DWI.System_RevisedDate = CONVERT(datetime, '9999', 126)
  AND DWI.System_IsDeleted = 0
  AND VLCWI2.WorkItemLinkTypeSK = 28
  AND DWI.TeamProjectSK = 1176)
   SELECT
    2 as ProjectID ,
    U.UserStoryName,
   -- CTE2.projectareapath,
    Cte.Poject_SourceID,
    --Cte.FeatureSourceID,
    Cte.UserStorySourceID,
    --Cte2.TaskSourceID,
    SUM(Cte2.CompletedWork) AS CompletedWork,
    SUM(Cte2.OriginalEstimate) AS OriginalEstimate,
    SUM(Cte2.RemainingWork)AS RemainingWork,
    SUM(Cte2.CompletedWork + ISNULL(Cte2.RemainingWork, 0) ) AS CurrentEstimate
   -- GETDATE() AS last_Run
   into QOE_Userstory
  FROM Cte
  INNER JOIN(SELECT
    --T.projectareapath AS projectareapath,
    1176 AS Poject_SourceID,
    DWI.WorkItemSK AS TaskSourceID,
    CompletedWork AS CompletedWork,
    OriginalEstimate AS OriginalEstimate,
    RemainingWork AS RemainingWork
  FROM [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] AS DWI
  INNER JOIN [Project] AS P
    ON p.ProjectSourceId = DWI.TeamProjectSK
  INNER JOIN [Task] AS T
    ON P.ProjectId = T.ProjectId
    AND T.TaskSourceId = DWI.system_ID
  WHERE  --  DWI.System_State = 'Closed'  and
  DWI.System_RevisedDate = CONVERT(datetime, '9999', 126)
  AND DWI.System_IsDeleted = 0
  AND DWI.TeamProjectSK = 1176
  ) AS Cte2
    ON Cte.Poject_SourceID = Cte2.Poject_SourceID
    AND Cte.TaskSourceID = Cte2.TaskSourceID

     INNER JOIN
  (
   SELECT distinct 1176 as Poject_SourceID , DWI.WorkItemSK as UserStorySourceID, US.UserStoryName
   FROM
                 [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as DWI
    INNER JOIN[Project] AS P
                               ON p.ProjectSourceId = DWI.TeamProjectSK

    INNER JOIN   [UserStory] AS US

                               ON P.projectid = US.ProjectId  AND US.UserStorySourceId = DWI.system_ID

    WHERE DWI.TeamProjectSK   = 1176 and DWI.System_RevisedDate = CONVERT(datetime, '9999', 126)) AS U

    ON U.UserStorySourceID = Cte.UserStorySourceID and U.Poject_SourceID = CTE.Poject_SourceID
  GROUP BY  U.UserStoryName, Cte.Poject_SourceID, Cte.UserStorySourceID



  --select* from [userstory] where projectid = 2

  --select* from project where ProjectId = 2

  --select top 3 * from[10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem]
  --select top 3 * from[10.1.22.94].[Tfs_Warehouse].[dbo].[vFactLinkedCurrentWorkItem]


  --select* from Feature where projectid = 2 FeatureSourceId in (225766,225767,286696)


  -- select* from   QOE_Userstory
GO
/****** Object:  StoredProcedure [ETL].[BugOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Proc[ETL].[BugOnPrimData]
        @ParentSourceId[INT] NULL,
@projectSourceId[INT] NULL ,
@ReleaseSourceId[INT] NULL  
as  
Begin
DELETE FROM ETL.Bug WHERE ProjectID = (SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND @ReleaseSourceId = ReleaseSourceId)
End
BEGIN
Declare @ProjectID INT
Set @ProjectID = 
(
SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND    @ReleaseSourceId = ReleaseSourceId 
)
END
INSERT INTO ETL.Bug
  SELECT
    DWI.System_ID,
    @ProjectID ,
    DWI.IterationSK,
    DWI.System_createdDate,
    DWI.System_title,
    DWI.System_state,
    1,
 DP.Name,
 FCWI.Microsoft_VSTS_Scheduling_CompletedWork,
    FCWI.Microsoft_VSTS_Scheduling_OriginalEstimate,
 FCWI.Microsoft_VSTS_Scheduling_RemainingWork,
    DWI.Microsoft_VSTS_Common_Activity

from [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as DWI left outer JOIN
     [10.1.22.94].[Tfs_Warehouse].[dbo].[FactCurrentWorkItem] as FCWI
ON DWI.WorkItemSK = FCWI.WorkItemSK and DWI.TeamProjectSK = FCWI.TeamProjectSK left outer join
     [10.1.22.94].[Tfs_Warehouse].[dbo].[DimPerson] as DP on DWI.System_AssignedTo__PersonSK= DP.personSK
  WHERE
      DWI.TeamProjectCollectionSK = @ParentSourceId
  AND DWI.TeamProjectSK = @projectSourceId
  AND DWI.AreaSK = @ReleaseSourceId
  AND DWI.System_WorkItemType = 'Bug'
  AND DWI.System_RevisedDate = '9999-01-01 00:00:00.000'
  AND DWI.System_IsDeleted = 0
GO
/****** Object:  StoredProcedure [ETL].[EpicOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc[ETL].[EpicOnPrimData]    --146  ,147,676   
@ParentSourceId[INT] NULL,
@projectSourceId[INT] NULL,
@ReleaseSourceId[INT] NULL
as    
Begin
DELETE FROM ETL.Epic WHERE ProjectID = (SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND @ReleaseSourceId = ReleaseSourceId)
End
Begin
Declare @ProjectID INT
Set @ProjectID = 
(
SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND    @ReleaseSourceId = ReleaseSourceId
)
END
INSERT INTO ETL.Epic
  SELECT
    System_ID,
    @ProjectID,
    IterationSK,
    System_createdDate,
    System_title,
    System_state,
    1    
  FROM[10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem]
  WHERE
      TeamProjectCollectionSK = @ParentSourceId
  AND TeamProjectSK = @projectSourceId
  AND AreaSK = @ReleaseSourceId
  AND System_WorkItemType = 'Epic'
  AND System_RevisedDate = '9999-01-01 00:00:00.000'
  AND System_IsDeleted = 0
GO
/****** Object:  StoredProcedure [ETL].[FeatureOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc[ETL].[FeatureOnPrimData]
@ParentSourceId [INT]  NULL,
@projectSourceId[INT] NULL,
@ReleaseSourceId[INT] NULL    
as    
Begin
DELETE FROM ETL.Feature WHERE ProjectID = (SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND @ReleaseSourceId = ReleaseSourceId)
End
BEGIN
 Declare @ProjectID INT
Set @ProjectID = 
(
SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND    @ReleaseSourceId = ReleaseSourceId 
)
END
INSERT INTO ETL.Feature
  SELECT
    System_ID,
    @ProjectID ,
    IterationSK,
    System_createdDate,
    System_title,
    System_state,
    1    
  FROM[10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem]    as DWI
  WHERE
      DWI.TeamProjectCollectionSK = @ParentSourceId
  AND DWI.TeamProjectSK = @projectSourceId
  AND DWI.AreaSK = @ReleaseSourceId
  AND System_WorkItemType = 'Feature'
  AND System_RevisedDate = '9999-01-01 00:00:00.000'
  AND System_IsDeleted = 0
GO
/****** Object:  StoredProcedure [ETL].[IterationOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc[ETL].[IterationOnPrimData]   -- 146  ,147,676      
@ParentSourceId[INT] NULL,
@projectSourceId[INT] NULL,
@ReleaseSourceId[INT] NULL
as      
Begin
DELETE FROM ETL.Iteration WHERE ProjectID = (SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND @ReleaseSourceId = ReleaseSourceId)
End
Begin
Declare @GUIDs NVARCHAR(1000)
set @GUIDs = (SELECT ProjectNodeGUID FROM[10.1.22.94].[Tfs_Warehouse].[dbo].[DimTeamProject]
        WHERE ProjectNodeSK = @ProjectSourceId AND ParentNodeSK = @ParentSourceId )   

Declare @ProjectID INT
Set @ProjectID = 
(
SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND    @ReleaseSourceId = ReleaseSourceId 
)

END
INSERT INTO ETL.Iteration
SELECT
IterationSK,
@ProjectID,
IterationName,
Depth,
LastUpdatedDateTime,
StartDate,
FinishDate,
NULL
FROM [10.1.22.94].[Tfs_Warehouse].[dbo].[DimIteration] WHERE projectguid = @GUIDs
GO
/****** Object:  StoredProcedure [ETL].[TaskOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Proc[ETL].[TaskOnPrimData]
@ParentSourceId [INT]  NULL,
@projectSourceId[INT] NULL,
@ReleaseSourceId[INT] NULL     
as    
Begin
DELETE FROM ETL.Task WHERE ProjectID = (SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND @ReleaseSourceId = ReleaseSourceId)
End
BEGIN
Declare @ProjectID INT
Set @ProjectID = 
(
SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND    @ReleaseSourceId = ReleaseSourceId 
)
END
INSERT INTO ETL.Task
  SELECT
    DWI.System_ID,
    @ProjectID ,
    DWI.IterationSK,
    DWI.System_createdDate,
    DWI.System_title,
    DWI.System_state,
    1,
 DP.Name,
 FCWI.Microsoft_VSTS_Scheduling_CompletedWork,
    FCWI.Microsoft_VSTS_Scheduling_OriginalEstimate,
 FCWI.Microsoft_VSTS_Scheduling_RemainingWork,
 DWI.Microsoft_VSTS_Common_Activity

from [10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as DWI left outer JOIN
     [10.1.22.94].[Tfs_Warehouse].[dbo].[FactCurrentWorkItem] as FCWI
ON DWI.WorkItemSK = FCWI.WorkItemSK and DWI.TeamProjectSK = FCWI.TeamProjectSK left outer join
     [10.1.22.94].[Tfs_Warehouse].[dbo].[DimPerson] as DP on DWI.System_AssignedTo__PersonSK= DP.personSK
  WHERE
        DWI.TeamProjectCollectionSK = @ParentSourceId
  AND DWI.TeamProjectSK = @projectSourceId
  AND DWI.AreaSK = @ReleaseSourceId
  AND DWI.System_WorkItemType = 'Task'
  AND DWI.System_RevisedDate = '9999-01-01 00:00:00.000'
  AND DWI.System_IsDeleted = 0
GO
/****** Object:  StoredProcedure [ETL].[TestManagerOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Exec ML.[ETL_TestManagerOnPrimData]  '3','TFS_BDCollection'


CREATE PROC [ETL].[TestManagerOnPrimData]
@ProjectID nvarchar(50),
@DBCollection nvarchar(50)

AS
-- Truncate Test Manager Master list
BEGIN
TRUNCATE table[ETL].[TestSuite]

        TRUNCATE table[ETL].[TestSuiteLink]

        TRUNCATE table[ETL].[TestCase]

        TRUNCATE table[ETL].[TestResult]
        END
-- Pull Data from Collection
--***************************
BEGIN
  DECLARE @SQL nvarchar(max)
  SET @SQL = '
Insert INTO[ETL].[TestSuite]
        SELECT '+@ProjectID+' as ProjectID,SuiteID,PlanID,ParentSuiteID,Title,lastupdated,Status,isDeleted
        FROM[10.1.22.94].'+@DBCollection+'.[dbo].[tbl_Suite]
        With(Nolock)


BEGIN
Insert INTO[ETL].[TestSuiteLink]
        SELECT  ' + @ProjectID + ' as ProjectID,* 
FROM[10.1.22.94].' + @DBCollection + '.[dbo].[tbl_SuiteEntry]
        With(Nolock)
END

BEGIN
; WITH CTE(AreaID, TestCaseID, TestCaseRefID, Priority, CreationDate, LastRefTestRunDate )
as (
SELECT TC1.AreaID,TC1.TestCaseID,TC1.TestCaseRefID,TC1.Priority,TC1.CreationDate,TC1.LastRefTestRunDate
FROM[10.1.22.94].' + @DBCollection + '.[TestResult].[tbl_TestCaseReference] AS TC1 With(Nolock) INNER JOIN(
SELECT AreaID, TestCaseID, Priority, max(TestCaseRefID) as TestCaseRefID, Max(lastRefTestRunDate) as lastRefTestRunDate
FROM [10.1.22.94].' + @DBCollection + '.[TestResult].[tbl_TestCaseReference] With(Nolock)
GROUP BY AreaID, TestCaseID, Priority
) as TC2
ON  TC1.AreaID = TC2.AreaID and TC1.TestCaseID = TC2.TestCaseID and TC1.TestCaseRefID = TC2.TestCaseRefID   
) 
Insert INTO[ETL].[TestCase]
        SELECT  ' + @ProjectID + ' as ProjectID ,*  
FROM CTE
END

BEGIN
; WITH CTE(TestRunID, TestCaseRefID, LastUpdated, OutCome, State )
as (
SELECT TR1.TestRunID,TR1.TestCaseRefID,TR1.LastUpdated,TR1.OutCome,TR1.State
FROM[10.1.22.94].' + @DBCollection + '.[TestResult].[tbl_TestResult] AS TR1 With(Nolock) INNER JOIN(
SELECT TestCaseRefID, max(TestRunID) as TestRunID
FROM [10.1.22.94].' + @DBCollection + '.[TestResult].[tbl_TestResult]  With(Nolock)
GROUP BY TestCaseRefID
) AS TR2
ON TR1.TestCaseRefID = TR2.TestCaseRefID and TR1.TestRunID = TR2.TestRunID--where TC1.testcaseid = 12352
)
Insert INTO[ETL].[TestResult]
        SELECT  ' + @ProjectID + ' as ProjectID ,*  
FROM CTE
END
'
--Print @SQL
 EXEC sp_sqlexec @SQL
 END
GO
/****** Object:  StoredProcedure [ETL].[UserStoryOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Proc[ETL].[UserStoryOnPrimData]    
-- Exec[ETL].[UserStoryOnPrimData]  146  , 676  
@ParentSourceId[INT] NULL,
@projectSourceId[INT] NULL ,
@ReleaseSourceId[INT] NULL   
as   
Begin
DELETE FROM ETL.UserStory WHERE ProjectID = (SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND @ReleaseSourceId = ReleaseSourceId)
End
BEGIN
Declare @ProjectID INT
Set @ProjectID = 
(
SELECT projectID FROM Project WHERE  ParentSourceId = @ParentSourceId AND ProjectSourceId = @projectSourceId AND    @ReleaseSourceId = ReleaseSourceId 
)
END
INSERT INTO ETL.UserStory
  SELECT
    DWI.System_ID,
    @ProjectID ,
    DWI.IterationSK,
    DWI.System_createdDate,
    DWI.System_title,
    DWI.System_state,
    DP.Name,
    isnull(FCWI.Microsoft_VSTS_Scheduling_StoryPoints,0) ,    
    1    
from[10.1.22.94].[Tfs_Warehouse].[dbo].[DimWorkItem] as DWI left outer JOIN
[10.1.22.94].[Tfs_Warehouse].[dbo].[FactCurrentWorkItem] as FCWI
ON DWI.WorkItemSK = FCWI.WorkItemSK and DWI.TeamProjectSK = FCWI.TeamProjectSK left outer join
[10.1.22.94].[Tfs_Warehouse].[dbo].[DimPerson] as DP on DWI.System_AssignedTo__PersonSK= DP.personSK
WHERE
         DWI.TeamProjectCollectionSK = @ParentSourceId
  AND DWI.TeamProjectSK = @projectSourceId
  AND DWI.AreaSK = @ReleaseSourceId
  AND DWI.System_WorkItemType = 'User Story'    
  AND DWI.System_RevisedDate = '9999-01-01 00:00:00.000'    
  AND DWI.System_IsDeleted = 0
GO
/****** Object:  StoredProcedure [FN].[Get_BurnRate_QualityOfOutput]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc[FN].[Get_BurnRate_QualityOfOutput]
as
begin
select Assignedto, cast(isnull(count(userstorysourceid),0) as decimal (5,2)) as CountOfResolvedUserstory
  into #t1 from UserStory
where state in ('Closed','Resolved')
group by assignedto
End
begin
select Assignedto,cast(isnull(count(Bugsourceid),0) as decimal (5,2)) CountOfResolvedBugs
 into #t2 
from Bug
where state in ('Closed','Resolved')
group by assignedto
end
begin
select Assignedto,cast(isnull(Sum([CompletedWork]),0) as decimal (5,2)) ActualWork
Into #t3 
from Task
where state in ('Closed','Resolved')
group by assignedto
end
begin

select #t1.Assignedto,#t1.CountOfResolvedUserstory,#t2.CountOfResolvedBugs,REPLACE (ActualWork, 0.00, 0.01) as ActualWork
Into #t4
from #t1  left outer join #t2 on #t1.Assignedto = #t2.Assignedto
left outer join #t3 on #t2.Assignedto = #t3.Assignedto 
where ActualWork is not null or CountOfResolvedBugs is not null
end
begin
create table #Temp
(
  Assignedto Nvarchar(100),
  CountOfResolvedUserstory int,
  CountOfResolvedBugs int,
  ActualWork decimal (5,2),
  QualityofOutput decimal (5,2),
  BurnRate_Hour decimal (5,2)
)
insert into #Temp
select
Assignedto,
CountOfResolvedUserstory,
CountOfResolvedBugs,
ActualWork,
 CountOfResolvedBugs/CountOfResolvedUserstory as 'QualityofOutput' ,
 CountOfResolvedUserstory/ActualWork as 'BurnRate/Hour'
from #t4
end
select* from #Temp
--drop table #t1
--drop table #t2
--drop table #t3
--drop table #t4

GO
/****** Object:  StoredProcedure [FN].[TestSuiteProgressStatus]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--select distinct outcome,outcomeid
--Into OutComeTestCaseRef
--from[10.1.22.94].[Tfs_Warehouse].dbo.DimTestResult
--where outcome is not null or outcomeid is not null

-- truncate table[dbo].[Tb_TestSuiteProgressStatus]
        CREATE PROC[FN].[TestSuiteProgressStatus]
-- EXEC TestSuiteProgressStatus  21111 ,'2018-07-01'
@PTS INT,
@TestDataAfter Datetime
as
--DECLARE @PTS INT 
--SET @PTS = 21111
Begin
;WITH CTE1(SuiteID, Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY  TS.SuiteID, Title, Status
 )
 SELECT TSL.SuiteId, Count(TC.TestCaseID) as TestCaseID, Count(TR.OutCome) as Passed
 Into   #t1
 -- drop table #t1
 FROM[TestSuiteLink] AS TSL INNER JOIN[TestCase] AS TC ON TC.TestCaseID = TSL.TestCaseId
 INNER JOIN [TestResult]  AS TR ON TC.TestCaseRefID = TR.TestCaseRefID
 WHERE TSL.SuiteId in (SELECT SuiteID FROM CTE1) and Outcome = 2  and TC.LastRefTestRunDate >= @TestDataAfter and  TR.LastUpdated >=  @TestDataAfter
 GROUP BY TSL.SuiteId
 End
 --#########################################################################################
 Begin
--  DECLARE @PTS INT
--SET @PTS = 21111
; WITH CTE1(SuiteID    , Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY  TS.SuiteID, Title, Status
 )
 SELECT TSL.SuiteId, Count(TC.TestCaseID) as TestCaseID, Count(TR.OutCome) as Failed
 Into #t2
 -- drop table #t2
 FROM[TestSuiteLink] AS TSL INNER JOIN[TestCase] AS TC ON TC.TestCaseID = TSL.TestCaseId
 INNER JOIN [TestResult]  AS TR ON TC.TestCaseRefID = TR.TestCaseRefID
 WHERE TSL.SuiteId in (SELECT SuiteID FROM CTE1) and Outcome = 3 and TC.LastRefTestRunDate >= @TestDataAfter and  TR.LastUpdated >=  @TestDataAfter
 GROUP BY TSL.SuiteId
 End
  --#########################################################################################
--DECLARE @PTS INT
--SET @PTS = 21111
Begin
; WITH CTE1(SuiteID    , Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY  TS.SuiteID, Title, Status
 )
 SELECT TSL.SuiteId, Count(TC.TestCaseID) as TestCaseID, Count(TR.OutCome) as NotApplicable
 Into   #t3
 -- drop table #t3
 FROM[TestSuiteLink] AS TSL INNER JOIN[TestCase] AS TC ON TC.TestCaseID = TSL.TestCaseId
 INNER JOIN [TestResult]  AS TR ON TC.TestCaseRefID = TR.TestCaseRefID
 WHERE TSL.SuiteId in (SELECT SuiteID FROM CTE1) and Outcome = 11 and TC.LastRefTestRunDate >= @TestDataAfter and  TR.LastUpdated >=  @TestDataAfter
 GROUP BY TSL.SuiteId
End
   --#########################################################################################
--DECLARE @PTS INT
--SET @PTS = 21111
Begin
; WITH CTE1(SuiteID    , Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY  TS.SuiteID, Title, Status
 )
 SELECT TSL.SuiteId, Count(TC.TestCaseID) as TestCaseID, Count(TR.OutCome) as   Blocked
 Into   #t4
 -- drop table #t4
 FROM[TestSuiteLink] AS TSL INNER JOIN[TestCase] AS TC ON TC.TestCaseID = TSL.TestCaseId
 INNER JOIN [TestResult]  AS TR ON TC.TestCaseRefID = TR.TestCaseRefID
 WHERE TSL.SuiteId in (SELECT SuiteID FROM CTE1) and Outcome = 7 and TC.LastRefTestRunDate >= @TestDataAfter and  TR.LastUpdated >=  @TestDataAfter
 GROUP BY TSL.SuiteId
 End
   --#########################################################################################
--DECLARE @PTS INT
--SET @PTS = 21111
Begin
; WITH CTE1(SuiteID    , Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY  TS.SuiteID, Title, Status
 )
 SELECT TSL.SuiteId, Count(TC.TestCaseID) as TestCaseID, Count(TR.OutCome) as Paused
 Into   #t5
 -- drop table #t5
 FROM[TestSuiteLink] AS TSL INNER JOIN[TestCase] AS TC ON TC.TestCaseID = TSL.TestCaseId
 INNER JOIN [TestResult]  AS TR ON TC.TestCaseRefID = TR.TestCaseRefID
 WHERE TSL.SuiteId in (SELECT SuiteID FROM CTE1) and Outcome = 12 and TC.LastRefTestRunDate >= @TestDataAfter and  TR.LastUpdated >=  @TestDataAfter
 GROUP BY TSL.SuiteId
End
    --#########################################################################################
--DECLARE @PTS INT
--SET @PTS = 21111
Begin
; WITH CTE1(SuiteID    , Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY  TS.SuiteID, Title, Status
 )
 SELECT TSL.SuiteId, Count(TC.TestCaseID) as TestCaseID, Count(TR.OutCome) as 'In progress'  
 Into   #t6
 -- drop table #t6
 FROM[TestSuiteLink] AS TSL INNER JOIN[TestCase] AS TC ON TC.TestCaseID = TSL.TestCaseId
 INNER JOIN [TestResult]  AS TR ON TC.TestCaseRefID = TR.TestCaseRefID
 WHERE TSL.SuiteId in (SELECT SuiteID FROM CTE1) and Outcome = 1 and TC.LastRefTestRunDate >= @TestDataAfter and  TR.LastUpdated >=  @TestDataAfter
 GROUP BY TSL.SuiteId
 End
  --#########################################################################################
--  DECLARE @PTS INT
--SET @PTS = 21111
Begin
; WITH CTE1(ProjectID, SuiteID  , Title  , Status, NumberOfTestCases)
AS
(
SELECT TS.ProjectID, TS.SuiteID, Title, Status, count(TSL.ChildSuiteId) AS NumberOfTestCases
-- Into #t4
FROM [TestSuite] AS TS INNER JOIN [TestSuiteLink] as TSL ON  TS.SuiteID = TSL.SuiteId
WHERE ParentSuiteID = @PTS and isDeleted !=1
 Group BY TS.ProjectID, TS.SuiteID, Title, Status
 )
Insert Into[dbo].TestSuiteProgressStatus
select P.ProjectName,CTE1.SuiteID,CTE1.Title,CTE1.NumberOfTestCases,ISNULL(Passed,0) as Passed ,ISNULL(Failed,0) as Failed,ISNULL(NotApplicable,0) as NotApplicable ,
       ISNULL(Blocked,0) as Blocked,ISNULL(Paused,0) as Paused, ISNULL([In progress],0) as 'In progress' 
	  ,(CTE1.NumberOfTestCases - (ISNULL(Passed,0)+ISNULL(Failed,0)+ISNULL(NotApplicable,0) +
       ISNULL(Blocked,0)+ISNULL(Paused,0)+ ISNULL([In progress],0) ) )as Active    
--into Tb_TestSuiteProgressStatus
from CTE1 left Join #t1 on CTE1.SuiteID = #t1.SuiteId left Join #t2 
                        on CTE1.SuiteID = #t2.SuiteId left Join #t3 
						on CTE1.SuiteID = #t3.SuiteId left Join #t4 
						on CTE1.SuiteID = #t4.SuiteId left Join #t5 
						on CTE1.SuiteID = #t5.SuiteId left Join #t6
						on CTE1.SuiteID = #t6.SuiteId left Join Project as P
						On CTE1.ProjectID = P.ProjectId
End
            Begin
drop table #t1
  drop table #t2
   drop table #t3
    drop table #t4
	drop table #t5
			

            End

select * from[dbo].TestSuiteProgressStatus
GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfBugsOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*    
      
Step 2     : From TaskEtl to the Task      
Object     : StoredProcedure [ML].[ADDLastUpdatedListOfTBugs]      
Script Date: 7/10/2018 11:25:46 PM      
Dev        : Islam Naggar        
    
*/
CREATE PROC [ML].[ADDLastUpdatedListOfBugsOnPrimData]
--@ProjectSourceId[INT] NULL,
--@ReleaseSourceId[INT] null      
as      
BEGIN
--DECLARE @ProjectID INT
--SET @ProjectID = (Select ProjectID from project where ProjectSourceId= @ProjectSourceId and ReleaseSourceId = @ReleaseSourceId)
-- Part 2    
--********    
--MERGE SQL statement -     
--Synchronize the target table with    
--refreshed data from source table    
--***********************************    
MERGE[dbo].[Bug]
        AS TARGET
USING[Etl].[Bug]
        AS SOURCE
ON(TARGET.[BugSourceId] = SOURCE.[BugSourceId] AND TARGET.ProjectId = SOURCE.ProjectID)
--When records are matched, update     
--the records if there is any change
WHEN MATCHED AND TARGET.ProjectId = SOURCE.ProjectID AND
   TARGET.IterationSK<> SOURCE.IterationSK
OR TARGET.CreationDate<> SOURCE.CreationDate
OR TARGET.[BugName] <> SOURCE.[BugName]
OR TARGET.[State] <> SOURCE.[State]
OR TARGET.[AssignedTo] <> SOURCE.[AssignedTo]
OR TARGET.IsActive<> SOURCE.[IsActive]
OR TARGET.[CompletedWork] <> SOURCE.[CompletedWork]
OR TARGET.[OriginalEstimate] <> SOURCE.[OriginalEstimate]
OR TARGET.[RemainingWork] <> SOURCE.[RemainingWork]
OR TARGET.[Activity] <> SOURCE.[Activity]
 THEN
UPDATE SET
  TARGET.IterationSK = SOURCE.IterationSK
, TARGET.CreationDate = SOURCE.CreationDate
, TARGET.[BugName] = SOURCE.[BugName]
, TARGET.[State] = SOURCE.[State]
, TARGET.[AssignedTo] = SOURCE.[AssignedTo]
, TARGET.IsActive = SOURCE.[IsActive]
, TARGET.[CompletedWork] = SOURCE.[CompletedWork]
, TARGET.[OriginalEstimate] = SOURCE.[OriginalEstimate]
, TARGET.[RemainingWork] = SOURCE.[RemainingWork]
, TARGET.[Activity] = SOURCE.[Activity]

--When no records are matched, insert
--the incoming records from source
--table to target table


WHEN NOT MATCHED BY TARGET THEN


INSERT ([BugSourceId], [ProjectId], [IterationSK],[CreationDate],[BugName],[State],[AssignedTo],[IsActive],[CompletedWork],[OriginalEstimate],[RemainingWork],[Activity])
VALUES (SOURCE.[BugSourceId], SOURCE.ProjectID, SOURCE.[IterationSK], SOURCE.[CreationDate], SOURCE.[BugName], SOURCE.[State], SOURCE.[AssignedTo], SOURCE.[IsActive], SOURCE.[CompletedWork], SOURCE.[OriginalEstimate], SOURCE.[RemainingWork], SOURCE.[Activity]);

        END
           BEGIN
   Update[Bug]
   SET AssignedTo = 'NOT Assigned'      
   Where AssignedTo is null      
   END
   Begin
   Update[Bug]
   Set
    CompletedWork    = 0    
   where
     CompletedWork     is NULL
   END
     Begin
   Update[Bug]
   Set
   OriginalEstimate = 0
   where
     OriginalEstimate  is NULL
   END
     Begin
   Update[Bug]
   Set
   RemainingWork = 0
   where
     RemainingWork    is NULL
   END
GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfEpicsOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*      
Step 2     : From EpicEtl to the Epic      
Object     : StoredProcedure [ML].[ADDLastUpdatedListOfEpics]      
Script Date: 7/10/2018 11:25:46 PM      
Dev        : Islam Naggar        
*/
CREATE PROC[ML].[ADDLastUpdatedListOfEpicsOnPrimData]      
--@ProjectSourceId[INT] NULL,
--@ReleaseSourceId[INT] null          
as          
BEGIN         
--DECLARE @ProjectID INT        
--SET @ProjectID = (Select ProjectID from project where ProjectSourceId=@ProjectSourceId and ReleaseSourceId =@ReleaseSourceId )        
-- Part 2        
--********        
--MERGE SQL statement -         
--Synchronize the target table with        
--refreshed data from source table        
--***********************************        
MERGE[dbo].[Epic]
        AS TARGET
USING[ETL].[Epic]
        AS SOURCE
ON(TARGET.EpicSourceId = SOURCE.EpicSourceId and TARGET.ProjectId = SOURCE.ProjectID)
--When records are matched, update
WHEN MATCHED AND(TARGET.IterationSK<> SOURCE.IterationSK and TARGET.ProjectId = SOURCE.ProjectID)
OR TARGET.CreationDate<> SOURCE.CreationDate
OR TARGET.EpicName<> SOURCE.EpicName
OR TARGET.State<> SOURCE.State
OR TARGET.IsActive<> SOURCE.IsActive
 THEN
UPDATE SET
  TARGET.IterationSK = SOURCE.IterationSK
, TARGET.CreationDate = SOURCE.CreationDate
, TARGET.EpicName = SOURCE.EpicName
, TARGET.State = SOURCE.State
, TARGET.IsActive = SOURCE.IsActive

--When no records are matched, insert
--the incoming records from source
--table to target table


WHEN NOT MATCHED BY TARGET THEN


INSERT (EpicSourceId, [ProjectId], [IterationSK],[CreationDate], EpicName,[State],[IsActive])
VALUES (SOURCE.EpicSourceId, SOURCE.ProjectID, SOURCE.[IterationSK], SOURCE.[CreationDate], SOURCE.EpicName, SOURCE.[State], SOURCE.[IsActive]);
        END
        SELECT @@ROWCOUNT; 
GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfFeaturesOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*      
Step 2     : From FeatureEtl to the Feature      
Object     : StoredProcedure [ML].[ADDLastUpdatedListOfFeatures]      
Script Date: 7/10/2018 11:25:46 PM      
Dev        : Islam Naggar        
*/
CREATE PROC[ML].[ADDLastUpdatedListOfFeaturesOnPrimData]      
--@ProjectSourceId[INT] NULL,
--@ReleaseSourceId[INT] null        
as        
BEGIN       
--DECLARE @ProjectID INT      
--SET @ProjectID = (Select ProjectID from project where ProjectSourceId=@ProjectSourceId and ReleaseSourceId =@ReleaseSourceId )      
-- Part 2      
--********      
--MERGE SQL statement -       
--Synchronize the target table with      
--refreshed data from source table      
--***********************************      
MERGE[dbo].[Feature]
        AS TARGET
USING[ETL].[Feature]
        AS SOURCE
ON(TARGET.FeatureSourceId = SOURCE.FeatureSourceId AND TARGET.ProjectId = SOURCE.ProjectID)
--When records are matched, update       
--the records if there is any change  FeatureId FeatureSourceId ProjectId IterationSK CreationDate FeatureName State IsActive
WHEN MATCHED AND TARGET.IterationSK<> SOURCE.IterationSK AND TARGET.ProjectId = SOURCE.ProjectID
OR TARGET.CreationDate<> SOURCE.CreationDate
OR TARGET.FeatureName<> SOURCE.FeatureName
OR TARGET.State<> SOURCE.State
OR TARGET.IsActive<> SOURCE.IsActive
 THEN
UPDATE SET
  TARGET.IterationSK = SOURCE.IterationSK
, TARGET.CreationDate = SOURCE.CreationDate
, TARGET.FeatureName = SOURCE.FeatureName
, TARGET.State = SOURCE.State
, TARGET.IsActive = SOURCE.IsActive

--When no records are matched, insert
--the incoming records from source
--table to target table


WHEN NOT MATCHED BY TARGET THEN


INSERT (FeatureSourceId, [ProjectId], [IterationSK],[CreationDate], FeatureName,[State],[IsActive])
VALUES (SOURCE.FeatureSourceId, SOURCE.ProjectID, SOURCE.[IterationSK], SOURCE.[CreationDate], SOURCE.FeatureName, SOURCE.[State], SOURCE.[IsActive]);
        END
        SELECT @@ROWCOUNT;
GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfIterationOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*        
Step 2     : From ETL.Iteration to the Iteration        
Object     : StoredProcedure [ML].[ADDLastUpdatedListOfIterations]        
Script Date: 09/09/2018 11:25:46 PM        
Dev        : Islam Naggar          
*/
CREATE PROC[ML].[ADDLastUpdatedListOfIterationOnPrimData]  
--@ParentSourceId[INT] null ,        
--@ProjectSourceId[INT] NULL      
         
as           
BEGIN         
--DECLARE @ProjectID INT        
--SET @ProjectID = (Select ProjectID from project where CollectionSourceId = @ParentSourceId and @ProjectSourceId = @ProjectSourceId  )        
-- Part 2        
--********        
--MERGE SQL statement -         
--Synchronize the target table with        
--refreshed data from source table        
--***********************************        
MERGE[dbo].[Iteration]
        AS TARGET
USING[ETL].[Iteration]
        AS SOURCE
ON(TARGET.IterationSourceId = SOURCE.IterationSK AND TARGET.ProjectId = SOURCE.ProjectId)
--When records are matched, update         
--the records if there is any change  IterationId IterationSourceId ProjectId IterationSK CreationDate IterationName State IsActive
WHEN MATCHED AND TARGET.ProjectId = SOURCE.ProjectId AND
   TARGET.IterationName<> SOURCE.IterationName
OR TARGET.Depth<> SOURCE.Depth
OR TARGET.LastUpdatedDateTime<> SOURCE. LastUpdatedDateTime
OR TARGET.StartDate<> SOURCE.StartDate
OR TARGET.EndDate<> SOURCE.EndDate

 THEN
UPDATE SET
  TARGET.IterationName = SOURCE.IterationName
, TARGET.Depth = SOURCE.Depth
, TARGET.LastUpdatedDateTime = SOURCE.LastUpdatedDateTime
, TARGET.StartDate = SOURCE.StartDate
, TARGET.EndDate = SOURCE.EndDate

--When no records are matched, insert
--the incoming records from source
--table to target table


WHEN NOT MATCHED BY TARGET THEN


INSERT (IterationSourceId, [ProjectId], IterationName, Depth, LastUpdatedDateTime, StartDate, EndDate)
VALUES (SOURCE.IterationSK, ProjectId, SOURCE.IterationName, SOURCE.Depth, SOURCE.LastUpdatedDateTime, SOURCE.StartDate, SOURCE.EndDate);
        END
        SELECT @@ROWCOUNT; 
GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfTasksOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*    
      
Step 2     : From TaskEtl to the Task      
Object     : StoredProcedure [ML].[ADDLastUpdatedListOfTasks]      
Script Date: 7/10/2018 11:25:46 PM      
Dev        : Islam Naggar        
    
*/
CREATE PROC[ML].[ADDLastUpdatedListOfTasksOnPrimData]       
--@ProjectSourceId[INT] NULL,
--@ReleaseSourceId[INT] null      
as      
BEGIN     
--DECLARE @ProjectID INT    
--SET @ProjectID = (Select ProjectID from project where ProjectSourceId=@ProjectSourceId and ReleaseSourceId =@ReleaseSourceId )    
-- Part 2    
--********    
--MERGE SQL statement -     
--Synchronize the target table with    
--refreshed data from source table    
--***********************************    
MERGE[dbo].[Task]
        AS TARGET
USING[ETL].[Task]
        AS SOURCE
ON(TARGET.TaskSourceId = SOURCE.TaskSourceId AND TARGET.ProjectId = SOURCE.ProjectID)
--When records are matched, update     
--the records if there is any change
WHEN MATCHED AND TARGET.ProjectId = SOURCE.ProjectID AND
   TARGET.IterationSK<> SOURCE.IterationSK
OR TARGET.CreationDate<> SOURCE.CreationDate
OR TARGET.[TaskName] <> SOURCE.[TaskName]
OR TARGET.[State] <> SOURCE.[State]
OR TARGET.[AssignedTo] <> SOURCE.[AssignedTo]
OR TARGET.IsActive<> SOURCE.[IsActive]
OR TARGET.[CompletedWork] <> SOURCE.[CompletedWork]
OR TARGET.[OriginalEstimate] <> SOURCE.[OriginalEstimate]
OR TARGET.[RemainingWork] <> SOURCE.[RemainingWork]
OR TARGET.[Activity] <> SOURCE.[Activity]
 THEN
UPDATE SET
  TARGET.IterationSK = SOURCE.IterationSK
, TARGET.CreationDate = SOURCE.CreationDate
, TARGET.[TaskName] = SOURCE.[TaskName]
, TARGET.[State] = SOURCE.[State]
, TARGET.[AssignedTo] = SOURCE.[AssignedTo]
, TARGET.IsActive = SOURCE.[IsActive]
, TARGET.[CompletedWork] = SOURCE.[CompletedWork]
, TARGET.[OriginalEstimate] = SOURCE.[OriginalEstimate]
, TARGET.[RemainingWork] = SOURCE.[RemainingWork]
, TARGET.[Activity] = SOURCE.[Activity]

--When no records are matched, insert
--the incoming records from source
--table to target table


WHEN NOT MATCHED BY TARGET THEN


INSERT ([TaskSourceId], [ProjectId], [IterationSK],[CreationDate],[TaskName],[State],[AssignedTo],[IsActive],[CompletedWork],[OriginalEstimate],[RemainingWork],[Activity])
VALUES (SOURCE.[TaskSourceId], SOURCE.ProjectID, SOURCE.[IterationSK], SOURCE.[CreationDate], SOURCE.[TaskName], SOURCE.[State], SOURCE.[AssignedTo], SOURCE.[IsActive], SOURCE.[CompletedWork], SOURCE.[OriginalEstimate], SOURCE.[RemainingWork], SOURCE.[Activity]);

        END
           BEGIN
   Update Task
   SET AssignedTo = 'NOT Assigned'
   Where AssignedTo is null      
   END
   Begin
   Update Task
   Set
    CompletedWork = 0
   where
     CompletedWork     is NULL
   END
     Begin
   Update Task
   Set
   OriginalEstimate = 0    
   where
     OriginalEstimate  is NULL
   END
     Begin
   Update Task
   Set
   RemainingWork    = 0    
   where
     RemainingWork    is NULL
   END
   Begin    
   ;    
   With Cte(ProjectID , name , Role)
  as    
  (
   Select A.ProjectID, A.name, AR.Role
   from[Assignee] as A inner join[AssigneeRoles] as AR
 on A.roleCode = AR.[RoleCode]    
  )    
    
  Update T
  Set T.Activity = C.Role
  from Task as t inner Join CTE as C
  ON T.AssignedTo = C.name and T.ProjectId = C.ProjectID
  where Activity is null    
  END


GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfTestManagerOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [ML].[ADDLastUpdatedListOfTestManagerOnPrimData] @ProjectID nvarchar(50) -- Exec ML.ETL_DeleteInsertTestManagerdata 5
AS
BEGIN
  truncate table [dbo].TestSuiteProgressStatus
  DELETE FROM[dbo].[TestCase]
  WHERE ProjectID = @ProjectID
  DELETE FROM [dbo].[TestSuiteLink]
  WHERE ProjectID = @ProjectID
  DELETE FROM [dbo].[TestResult]
  WHERE ProjectID = @ProjectID
  DELETE FROM [dbo].[TestSuite]
  WHERE ProjectID = @ProjectID
END
  BEGIN
    INSERT INTO [dbo].[TestCase]
      SELECT
        *
      FROM [ETL].[TestCase]
      WHERE ProjectID = @ProjectID
    INSERT INTO [dbo].[TestSuiteLink]
      SELECT
        *
      FROM [ETL].[TestSuiteLink]
      WHERE ProjectID = @ProjectID
    INSERT INTO [dbo].[TestResult]
      SELECT
        *
      FROM [ETL].[TestResult]
      WHERE ProjectID = @ProjectID
    INSERT INTO [dbo].[TestSuite]
      SELECT
        *
      FROM [ETL].[TestSuite]
      WHERE ProjectID = @ProjectID
  END
GO
/****** Object:  StoredProcedure [ML].[ADDLastUpdatedListOfUserStorysOnPrimData]    Script Date: 5/23/2019 12:56:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [ML].[ADDLastUpdatedListOfUserStorysOnPrimData]
--@ProjectSourceId[INT] NULL,
--@ReleaseSourceId[INT] null      
as      
BEGIN
--DECLARE @ProjectID INT
--SET @ProjectID = (Select ProjectID from project where ProjectSourceId= @ProjectSourceId and ReleaseSourceId = @ReleaseSourceId)
-- Part 2    
--********    
--MERGE SQL statement -     
--Synchronize the target table with    
--refreshed data from source table    
--***********************************    
MERGE[dbo].[UserStory]
        AS TARGET
USING[ETL].[UserStory]
        AS SOURCE
ON(TARGET.UserStorySourceId = SOURCE.UserStorySourceId  AND TARGET.ProjectId = SOURCE.ProjectID)
--When records are matched, update     
--the records if there is any change
WHEN MATCHED AND TARGET.IterationSK<> SOURCE.IterationSK AND TARGET.ProjectId = SOURCE.ProjectID
OR TARGET.CreationDate<> SOURCE.CreationDate
OR TARGET.UserStoryName<> SOURCE.UserStoryName
OR TARGET.State<> SOURCE.State
OR TARGET.AssignedTo<> SOURCE.AssignedTo
OR TARGET.StoryPoints<> SOURCE.StoryPoints
OR TARGET.IsActive<> SOURCE.IsActive
 THEN
UPDATE SET
  TARGET.CreationDate = SOURCE.CreationDate
, TARGET.UserStoryName = SOURCE.UserStoryName
, TARGET.State = SOURCE.State
, TARGET.AssignedTo = SOURCE.AssignedTo
, TARGET.StoryPoints = SOURCE.StoryPoints
, TARGET.IsActive = SOURCE.IsActive

--When no records are matched, insert
--the incoming records from source
--table to target table


WHEN NOT MATCHED BY TARGET THEN


INSERT (UserStorySourceId, [ProjectId], [IterationSK],[CreationDate],[UserStoryName],[State],[AssignedTo],[StoryPoints],[IsActive])
VALUES (SOURCE.UserStorySourceId, SOURCE.ProjectID, SOURCE.[IterationSK], SOURCE.[CreationDate], SOURCE.[UserStoryName], SOURCE.[State], SOURCE.[AssignedTo], SOURCE.[StoryPoints], SOURCE.[IsActive]);
        END
         BEGIN
   Update Userstory
   SET AssignedTo = 'NOT Assigned'
   Where AssignedTo is null      
   END
SELECT @@ROWCOUNT; 
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
