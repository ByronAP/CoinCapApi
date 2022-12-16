using System;

namespace CoinCapApi.Exceptions
{
    /// <summary>
    /// An exception because I am dumb and don't know WFT it is.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    public class UnknownException : Exception
    {
        public UnknownException(string message) : base(message)
        {
        }
    }
}
