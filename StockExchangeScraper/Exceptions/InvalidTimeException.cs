using System;

namespace StockExchangeScraper
{
	[Serializable]
	public class InvalidTimeException : Exception
	{
		public InvalidTimeException() :base("The app tries to get data during a trading session.") { }
		public InvalidTimeException(string message) : base(message) { }
		public InvalidTimeException(string message, Exception inner) : base(message, inner) { }
		protected InvalidTimeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
