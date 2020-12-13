CREATE PROCEDURE [dbo].[InsertExchangeHoliday]
	@ExchangeName varchar(100),
	@Date date
AS
BEGIN
	DECLARE @ExchangeID int

	SELECT @ExchangeID=ExchangeID
	FROM Exchanges
	WHERE Name=@ExchangeName

	INSERT INTO ExchangeHolidays (
		ExchangeID,
		Date
	)
	VALUES (
		 @ExchangeID,
		 @Date)
END
GO
