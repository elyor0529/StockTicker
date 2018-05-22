using System;

namespace StockTicker.Lib.Common.Exceptions
{
    public class StockException : Exception
    {
        /// <summary>
        /// init base for empty throwing 
        /// </summary>
        public StockException() : base()
        {

        }

        /// <summary>
        /// send message to base ctor
        /// </summary>
        /// <param name="message">Exp message</param>
        public StockException(string message) : base(message)
        {

        }

        /// <summary>
        /// Send message include with inner excetion
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public StockException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
