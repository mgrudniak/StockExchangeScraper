CREATE PROCEDURE [dbo].[InsertCompany]
	@Isin char(12),
	@Name varchar(50),
	@Sector varchar(25), 
	@Industry varchar(50), 
	@EquityType varchar(30), 
	@Exchange varchar(100), 
	@EmployeesNumber int,
	@Url varchar(200)
AS
BEGIN
	DECLARE @SectorID int, @IndustryID int, @EquityTypeID int, @ExchangeID int

	SELECT TOP 1 @SectorID=SectorID
	FROM Sectors
	WHERE Name=@Sector

	SELECT TOP 1 @IndustryID=IndustryID
	FROM Industries
	WHERE Name=@Industry

	SELECT TOP 1 @EquityTypeID=EquityTypeID
	FROM EquityTypes
	WHERE Name=@EquityType

	SELECT TOP 1 @ExchangeID=ExchangeID
	FROM Exchanges
	WHERE Name=@Exchange

	IF NOT EXISTS (SELECT *
				   FROM Companies
				   WHERE ISIN=@Isin)
		INSERT INTO Companies (
			ISIN,
			Name,
			SectorID,
			IndustryID,
			EquityTypeID,
			ExchangeID,
			EmployeesNumber,
			Url)
		VALUES (
			@Isin,
			@Name,
			@SectorID,
			@IndustryID,
			@EquityTypeID,
			@ExchangeID,
			@EmployeesNumber,
			@Url)
END
GO
