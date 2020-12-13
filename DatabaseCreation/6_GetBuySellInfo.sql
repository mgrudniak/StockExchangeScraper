CREATE PROCEDURE [dbo].[GetBuySellInfo]
AS
BEGIN
	DECLARE @Table TABLE (CompanyISIN char(12), Date date, 
						  RSI float, STOCH float, STOCHRSI float, MACD float, MACDsignal float, ADX float, MINUS_DI float, PLUS_DI float, WILLR float,
						  CCI float, ATR float, HighLow float, HighLowSignal float, ULTOSC float, ROC float, BullBear float, Bull float, Bear float,
						  SMA5 float, SMA10 float, SMA20 float, SMA50 float, SMA100 float, SMA200 float,
						  EMA5 float, EMA10 float, EMA20 float, EMA50 float, EMA100 float, EMA200 float, 
						  ClosePrice float, PriceChange float)
	INSERT INTO @Table
	EXEC TechnicalAnalysis 

	SELECT	CompanyISIN, Date, 
			CASE 
				WHEN RSI<=30 THEN 'Oversold'
				WHEN RSI>30 AND RSI<=45 THEN 'Sell'
				WHEN RSI>45 AND RSI<=55 THEN 'Neutral'
				WHEN RSI>55 AND RSI<=70 THEN 'Buy'
				WHEN RSI>70 THEN 'Overbought'
			END AS RSI, 
			CASE 
				WHEN STOCH<=20 THEN 'Oversold'
				WHEN STOCH>20 AND STOCH<=45 THEN 'Sell'
				WHEN STOCH>45 AND STOCH<=55 THEN 'Neutral'
				WHEN STOCH>55 AND STOCH<=80 THEN 'Buy'
				WHEN STOCH>80 THEN 'Overbought'
			END AS STOCH, 
			CASE 
				WHEN STOCHRSI<=25 THEN 'Oversold'
				WHEN STOCHRSI>25 AND STOCHRSI<=45 THEN 'Sell'
				WHEN STOCHRSI>45 AND STOCHRSI<=55 THEN 'Neutral'
				WHEN STOCHRSI>55 AND STOCHRSI<=75 THEN 'Buy'
				WHEN STOCHRSI>75 THEN 'Overbought'
			END AS STOCHRSI, 
			CASE 
				WHEN MACD<MACDsignal AND LAG(MACD-MACDsignal) OVER(PARTITION BY CompanyISIN ORDER BY Date)>0 THEN 'Sell'
				WHEN MACD>MACDsignal AND LAG(MACD-MACDsignal) OVER(PARTITION BY CompanyISIN ORDER BY Date)<0 THEN 'Buy'
				WHEN MACD IS NULL OR MACDsignal IS NULL THEN NULL
				ELSE 'Neutral'
			END AS MACD, 
			CASE 
				WHEN ADX<=25 THEN 'No Trend'
				WHEN ADX>25 AND ADX<=50 AND PLUS_DI>MINUS_DI THEN 'Medium Uptrend'
				WHEN ADX>50 AND ADX<=75 AND PLUS_DI>MINUS_DI THEN 'Strong Uptrend'
				WHEN ADX>75 AND ADX<=100 AND PLUS_DI>MINUS_DI THEN 'Very Strong Uptrend'

				WHEN ADX>25 AND ADX<=50 AND PLUS_DI<MINUS_DI THEN 'Medium Downtrend'
				WHEN ADX>50 AND ADX<=75 AND PLUS_DI<MINUS_DI THEN 'Strong Downtrend'
				WHEN ADX>75 AND ADX<=100 AND PLUS_DI<MINUS_DI THEN 'Very Strong Downtrend'
			END AS ADX,
			CASE
				WHEN PLUS_DI>MINUS_DI THEN 'Buy'
				WHEN PLUS_DI<MINUS_DI THEN 'Sell'
			END AS DMI,
			CASE 
				WHEN WILLR>=-20 THEN 'Overbought'
				WHEN WILLR<-20 AND WILLR>=-45 THEN 'Buy'
				WHEN WILLR<-45 AND WILLR>=-55 THEN 'Neutral'
				WHEN WILLR<-55 AND WILLR>=-80 THEN 'Sell'
				WHEN WILLR<-80 THEN 'Oversold'
			END AS WILLR,
			CASE 
				WHEN CCI<=-100 THEN 'Strong Sell'
				WHEN CCI>-100 AND CCI<=-50 THEN 'Sell'
				WHEN CCI>-50 AND CCI<=50 THEN 'Neutral'
				WHEN CCI>50 AND CCI<=100 THEN 'Buy'
				WHEN CCI>100 THEN 'Strong Buy'
			END AS CCI,
			CASE 
				WHEN HighLow<=30 AND HighLow<HighLowSignal THEN 'StrongSell'
				WHEN HighLow>30 AND HighLow<=50 AND HighLow<HighLowSignal THEN 'Sell'
				WHEN HighLow>50 AND HighLow<=70 AND HighLow>HighLowSignal THEN 'Buy'
				WHEN HighLow>70 AND HighLow>HighLowSignal THEN 'Strong Buy'
				WHEN HighLow IS NULL OR HighLowSignal IS NULL THEN NULL
				ELSE 'Neutral'
			END AS HighLow, 
			CASE 
				WHEN ULTOSC<=30 THEN 'Oversold'
				WHEN ULTOSC>30 AND ULTOSC<=45 THEN 'Sell'
				WHEN ULTOSC>45 AND ULTOSC<=55 THEN 'Neutral'
				WHEN ULTOSC>55 AND ULTOSC<=70 THEN 'Buy'
				WHEN ULTOSC>70 THEN 'Overbought'
			END AS ULTOSC,
			CASE 
				WHEN Bull>0 AND Bear>0 AND LAG(Bear) OVER(PARTITION BY CompanyISIN ORDER BY Date)<0 THEN 'Buy'
				WHEN Bull<0 AND Bear<0 AND LAG(Bull) OVER(PARTITION BY CompanyISIN ORDER BY Date)>0 THEN 'Sell'
				WHEN Bull IS NULL OR Bear IS NULL THEN NULL
				ELSE 'Neutral'
			END AS BullBear,
			CASE
				WHEN SMA5<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS SMA5,
			CASE
				WHEN SMA10<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS SMA10,
			CASE
				WHEN SMA20<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS SMA20,
			CASE
				WHEN SMA50<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS SMA50,
			CASE
				WHEN SMA100<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS SMA100,
			CASE
				WHEN SMA200<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS SMA200,
			CASE
				WHEN EMA5<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS EMA5,
			CASE
				WHEN EMA10<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS EMA10,
			CASE
				WHEN EMA20<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS EMA20,
			CASE
				WHEN EMA50<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS EMA50,
			CASE
				WHEN EMA100<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS EMA100,
			CASE
				WHEN EMA200<ClosePrice THEN 'Buy'
				ELSE 'Sell'
			END AS EMA200
	FROM @Table
END