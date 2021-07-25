USE [master]
GO

/****** Object:  Database [TemplateDB]    Script Date: 18/04/2021 12:32:26 ******/
CREATE DATABASE [TemplateDB]
GO


USE [TemplateDB]
GO

/****** Object:  Table [dbo].[Sales]    Script Date: 18/04/2021 12:39:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE TemplateDB.dbo.SAMPLE_TABLE (
	ID int NOT NULL IDENTITY(1,1),
	TESTE_PROPERTY varchar(50) NOT NULL,
	ACTIVE bit NOT NULL,
	CONSTRAINT PK_SAMPLE_TABLE PRIMARY KEY (id)
);