USE [jupiter_CRM]
GO
/****** Object:  Table [dbo].[CRM_AttendanceLog]    Script Date: 29/04/2016 12:26:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CRM_AttendanceLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CRM_AttendancePersonTypeID] [int] NOT NULL,
	[CRM_CRM_AttendanceLogGroupID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CRM_AttendanceLogGroup]    Script Date: 29/04/2016 12:26:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CRM_AttendanceLogGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AddedTimeStamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CRM_AttendancePersonType]    Script Date: 29/04/2016 12:26:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CRM_AttendancePersonType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsArchived] [bit] NOT NULL,
	[OrderNo] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CRM_AttendanceLog]  WITH CHECK ADD FOREIGN KEY([CRM_AttendancePersonTypeID])
REFERENCES [dbo].[CRM_AttendancePersonType] ([ID])
GO
ALTER TABLE [dbo].[CRM_AttendanceLog]  WITH CHECK ADD FOREIGN KEY([CRM_CRM_AttendanceLogGroupID])
REFERENCES [dbo].[CRM_AttendanceLogGroup] ([ID])
GO
EXEC sys.sp_addextendedproperty @name=N'Description', @value=N'Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CRM_AttendancePersonType', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CRM_AttendancePersonType', @level2type=N'COLUMN',@level2name=N'Name'
GO
