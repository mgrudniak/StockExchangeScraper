ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [C1] FOREIGN KEY([SectorID])
REFERENCES [dbo].[Sectors] ([SectorID])
GO

ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [C2] FOREIGN KEY([IndustryID])
REFERENCES [dbo].[Industries] ([IndustryID])
GO

ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [C3] FOREIGN KEY([EquityTypeID])
REFERENCES [dbo].[EquityTypes] ([EquityTypeID])
GO

ALTER TABLE [dbo].[Companies]  WITH CHECK ADD  CONSTRAINT [C4] FOREIGN KEY([ExchangeID])
REFERENCES [dbo].[Exchanges] ([ExchangeID])
GO

ALTER TABLE [dbo].[ExchangeHolidays]  WITH CHECK ADD  CONSTRAINT [C5] FOREIGN KEY([ExchangeID])
REFERENCES [dbo].[Exchanges] ([ExchangeID])
GO

ALTER TABLE [dbo].[Exchanges]  WITH CHECK ADD  CONSTRAINT [C6] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([CountryID])
GO

ALTER TABLE [dbo].[StockQuotes]  WITH CHECK ADD  CONSTRAINT [C7] FOREIGN KEY([CompanyISIN])
REFERENCES [dbo].[Companies] ([ISIN])
GO

ALTER TABLE [dbo].[StockQuotes]  WITH CHECK ADD  CONSTRAINT [CHK_StockQuote] CHECK  (([Open]>(0) AND [Close]>(0) AND [Min]>(0) AND [Max]>(0) AND [Volume]>=(0)))
GO
