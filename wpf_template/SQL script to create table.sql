
/*
	This script creates one table. Application is going to use it to store background images.
	Open it in Mangement Studio and execute. Make sure you selected correct database!!
*/


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_fact_Background_images](
	[Image id] [int] IDENTITY(1,1) NOT NULL,
	[Background image] [image] NULL,
	[Background image name] [nvarchar](100) NULL DEFAULT (N''),
	[Background image extension] [nvarchar](100) NULL DEFAULT (N''),
	[Created date] [datetime2](2) NULL DEFAULT (getdate()),
 CONSTRAINT [PK_tbl_fact_SWP_Background_images] PRIMARY KEY CLUSTERED 
(
	[Image id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]




