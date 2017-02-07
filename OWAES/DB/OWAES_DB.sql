GO
/****** Object:  Table [dbo].[Task]    Script Date: 02/07/2017 17:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Task](
	[TaskID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](50) NULL,
	[TaskRunDateTime] [datetime] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PersonLinkedIDDetail]    Script Date: 02/07/2017 17:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PersonLinkedIDDetail](
	[PersonLinkedIDNo] [varchar](20) NOT NULL,
	[PersonIDNo] [varchar](20) NOT NULL,
	[CreatedDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MESSAGE]    Script Date: 02/07/2017 17:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MESSAGE](
	[System] [int] IDENTITY(1,1) NOT NULL,
	[BailWeightage] [int] NOT NULL,
	[BailTypeID] [varchar](10) NULL,
	[BailTypeName] [varchar](66) NULL,
	[MsgContent] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_MESSAGE] PRIMARY KEY CLUSTERED 
(
	[System] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LOG]    Script Date: 02/07/2017 17:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LOG](
	[LogID] [uniqueidentifier] NOT NULL,
	[LogDateTime] [datetime] NULL,
	[PersonIDNo] [varchar](30) NULL,
 CONSTRAINT [PK_LOG] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CASETYPE]    Script Date: 02/07/2017 17:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CASETYPE](
	[CaseTypeID] [varchar](10) NOT NULL,
	[CaseTypeDescription] [varchar](200) NULL,
	[CaseTypeCode] [varchar](15) NULL,
 CONSTRAINT [PK_CASETYPE] PRIMARY KEY CLUSTERED 
(
	[CaseTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WARRANT]    Script Date: 02/07/2017 17:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WARRANT](
	[SystemID] [int] IDENTITY(1,1) NOT NULL,
	[WarrantNo] [varchar](30) NOT NULL,
	[SummonsNo] [varchar](500) NULL,
	[PersonIDNo] [varchar](20) NOT NULL,
	[WarrantStatusID] [varchar](1) NOT NULL,
	[BailTypeID] [varchar](10) NOT NULL,
	[BailAmount] [int] NOT NULL,
	[BailWeightage] [int] NOT NULL,
	[WarrantCaseTypeID] [varchar](10) NOT NULL,
	[WarrantIsPassportImpounded] [bit] NULL,
	[NoOfSurety] [smallint] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_WARRANT] PRIMARY KEY CLUSTERED 
(
	[SystemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comma Seperated Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WARRANT', @level2type=N'COLUMN',@level2name=N'SummonsNo'
GO
/****** Object:  View [dbo].[vwWarrantInfo]    Script Date: 02/07/2017 17:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwWarrantInfo]
AS

SELECT 
A.WarrantNo, 
A.PersonIDNo,
A.SummonsNo, 
A.CaseTypeDescription, 
M.MsgContent 

FROM (
SELECT     dbo.WARRANT.WarrantNo, dbo.WARRANT.SummonsNo, dbo.WARRANT.PersonIDNo, dbo.WARRANT.WarrantStatusID, dbo.WARRANT.BailTypeID, 
                      dbo.WARRANT.BailAmount, dbo.WARRANT.BailWeightage, dbo.WARRANT.WarrantCaseTypeID, dbo.WARRANT.WarrantIsPassportImpounded, 
                      dbo.WARRANT.NoOfSurety, dbo.WARRANT.CreatedDate, dbo.WARRANT.ModifiedDate, dbo.CASETYPE.CaseTypeDescription,
                      MAX(BailWeightage) over (Partition by PersonIDNo) MaxBailWeightage
FROM         dbo.WARRANT LEFT JOIN
             dbo.CASETYPE ON dbo.WARRANT.WarrantCaseTypeID = dbo.CASETYPE.CaseTypeID
) A              
INNER JOIN dbo.MESSAGE M on A.MaxBailWeightage = M.BailWeightage
WHERE WarrantStatusID = 'A'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[33] 4[28] 2[17] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "WARRANT"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CASETYPE"
            Begin Extent = 
               Top = 6
               Left = 310
               Bottom = 110
               Right = 500
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwWarrantInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwWarrantInfo'
GO
/****** Object:  View [dbo].[vwLastSuccessfulUpdate]    Script Date: 02/07/2017 17:17:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE view [dbo].[vwLastSuccessfulUpdate]
as 
select top 1 ISNULL(TaskRunDateTime,getdate()) LastSuccessfulUpdate from Task  
where [Status] = 'Success'
order by TaskRunDateTime desc
GO
/****** Object:  Default [DF_LOG_LogID]    Script Date: 02/07/2017 17:17:47 ******/
ALTER TABLE [dbo].[LOG] ADD  CONSTRAINT [DF_LOG_LogID]  DEFAULT (newid()) FOR [LogID]
GO
