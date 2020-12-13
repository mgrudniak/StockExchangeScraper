CREATE TABLE [dbo].[Companies](
	[ISIN] [char](12) PRIMARY KEY,
	[Name] [nvarchar](50) NULL,
	[SectorID] [int] NULL,
	[IndustryID] [int] NULL,
	[EquityTypeID] [int] NULL,
	[ExchangeID] [int] NULL,
	[EmployeesNumber] [int] NULL,
	[Url] [varchar](200) NULL
)
GO

CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](30) NULL
)
GO

CREATE TABLE [dbo].[EquityTypes](
	[EquityTypeID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](30) NULL
)
GO

CREATE TABLE [dbo].[ExchangeHolidays](
	[ExchangeHolidayID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ExchangeID] [int] NOT NULL,
	[Date] [date] NOT NULL
)
GO

CREATE TABLE [dbo].[Exchanges](
	[ExchangeID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](100) NOT NULL,
	[CountryID] [int] NOT NULL
)
GO

CREATE TABLE [dbo].[Industries](
	[IndustryID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](50) NULL
)
GO

CREATE TABLE [dbo].[Sectors](
	[SectorID] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [varchar](25) NULL
)
GO

CREATE TABLE [dbo].[StockQuotes](
	[StockQuoteID] [int] IDENTITY(1,1) PRIMARY KEY,
	[CompanyISIN] [char](12) NOT NULL,
	[Date] [date] NOT NULL,
	[Open] [float] NULL,
	[Close] [float] NULL,
	[Min] [float] NULL,
	[Max] [float] NULL,
	[Volume] [float] NULL
)
GO
