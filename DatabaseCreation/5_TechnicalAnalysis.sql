CREATE PROCEDURE [dbo].[TechnicalAnalysis]
AS
BEGIN
		EXEC sp_execute_external_script
			@language = N'Python',
			@script = N'
import pandas as pd
import talib

InputDataSet = InputDataSet.sort_values(by=["CompanyISIN", "Date"])
InputDataSet = InputDataSet.groupby(["CompanyISIN", "Date"]).first()

def STOCHRSI(c, timeperiod=14, fastk_period=5, fastd_period=3):
    rsi = talib.RSI(c, timeperiod)
    return talib.STOCH(rsi, rsi, rsi, timeperiod, fastk_period, fastd_period)

dfs = []
for date, new_df in InputDataSet.groupby(level=0):
    new_df.reset_index(inplace=True)
    
    new_df["RSI"] = round(talib.RSI(new_df.LastValue, timeperiod=14), 3)
    new_df["STOCH"] = round(talib.STOCH(new_df.MaxValue, new_df.MinValue, new_df.LastValue, fastk_period=9, slowk_period=6, slowd_period=3)[0], 3)
    new_df["STOCHRSI"] = round(STOCHRSI(new_df.LastValue, timeperiod=14, fastk_period=1, fastd_period=6)[0], 3)

    macd = talib.MACD(new_df.LastValue)
    new_df["MACD"] = round(macd[0], 3)
    new_df["MACDsignal"] = round(macd[1], 3)

    new_df["ADX"] = round(talib.ADX(new_df.MaxValue, new_df.MinValue, new_df.LastValue, timeperiod=14), 3)
    new_df["MINUS_DI"] = round(talib.MINUS_DI(new_df.MaxValue, new_df.MinValue, new_df.LastValue, timeperiod=14), 3)
    new_df["PLUS_DI"] = round(talib.PLUS_DI(new_df.MaxValue, new_df.MinValue, new_df.LastValue, timeperiod=14), 3)
    new_df["WILLR"] = round(talib.WILLR(new_df.MaxValue, new_df.MinValue, new_df.LastValue, timeperiod=14), 3)
    new_df["CCI"] = round(talib.CCI(new_df.MaxValue, new_df.MinValue, new_df.LastValue, timeperiod=14), 3)
    new_df["ATR"] = round(talib.ATR(new_df.MaxValue, new_df.MinValue, new_df.LastValue, timeperiod=14), 3)
    
    lows = new_df.MinValue.rolling(52).min()
    highs = new_df.MaxValue.rolling(52).max()
    recordHighPercent = (highs) / (lows + highs) * 100
    new_df["HighLow"] = round(recordHighPercent.rolling(10).mean(), 3)
    new_df["HighLowSignal"] = round(recordHighPercent.rolling(20).mean(), 3)
    
    new_df["ULTOSC"] = round(talib.ULTOSC(new_df.MaxValue, new_df.MinValue, new_df.LastValue), 3)
    new_df["ROC"] = round(talib.ROC(new_df.LastValue, timeperiod=14), 3)
    
    ema = talib.EMA(new_df.LastValue, timeperiod=13)
    bull = new_df.MaxValue - ema
    bear = new_df.MinValue - ema
    new_df["BullBear"] = round(bull + bear, 3)
    new_df["Bull"] = round(bull, 3)
    new_df["Bear"] = round(bear, 3)
    
    new_df["SMA5"] = round(talib.SMA(new_df.LastValue, 5), 3)
    new_df["SMA10"] = round(talib.SMA(new_df.LastValue, 10), 3)
    new_df["SMA20"] = round(talib.SMA(new_df.LastValue, 20), 3)
    new_df["SMA50"] = round(talib.SMA(new_df.LastValue, 50), 3)
    new_df["SMA100"] = round(talib.SMA(new_df.LastValue, 100), 3)
    new_df["SMA200"] = round(talib.SMA(new_df.LastValue, 200), 3)
    
    new_df["EMA5"] = round(talib.EMA(new_df.LastValue, 5), 3)
    new_df["EMA10"] = round(talib.EMA(new_df.LastValue, 10), 3)
    new_df["EMA20"] = round(talib.EMA(new_df.LastValue, 20), 3)
    new_df["EMA50"] = round(talib.EMA(new_df.LastValue, 50), 3)
    new_df["EMA100"] = round(talib.EMA(new_df.LastValue, 100), 3)
    new_df["EMA200"] = round(talib.EMA(new_df.LastValue, 200), 3)
    
    new_df["PriceChange"] = round((new_df.LastValue/new_df.LastValue.shift(1)*100) - 100, 2)
    
    new_df = new_df[["CompanyISIN", "Date", 
                     "RSI", "STOCH", "STOCHRSI", "MACD", "MACDsignal", "ADX", "MINUS_DI", "PLUS_DI", "WILLR", 
                     "CCI", "ATR", "HighLow", "HighLowSignal", "ULTOSC", "ROC", "BullBear", "Bull", "Bear", 
                     "SMA5", "SMA10", "SMA20", "SMA50", "SMA100", "SMA200",  
                     "EMA5", "EMA10", "EMA20", "EMA50", "EMA100", "EMA200", 
                     "LastValue", "PriceChange"]]
    
    dfs.append(new_df)

OutputDataSet = pd.concat(dfs)
	',
		@input_data_1 = N'
SELECT CompanyISIN, Date, LastValue, MinValue, MaxValue
FROM StockQuotes
ORDER BY CompanyISIN, Date'
		WITH RESULT SETS((CompanyISIN char(12), Date date,
						  RSI float, STOCH float, STOCHRSI float, MACD float, MACDsignal float, ADX float, MINUS_DI float, PLUS_DI float, WILLR float,
						  CCI float, ATR float, HighLow float, HighLowSignal float, ULTOSC float, ROC float, BullBear float, Bull float, Bear float, 
						  SMA5 float, SMA10 float, SMA20 float, SMA50 float, SMA100 float, SMA200 float,
						  EMA5 float, EMA10 float, EMA20 float, EMA50 float, EMA100 float, EMA200 float, 
						  ClosePrice float, PriceChange float))
END