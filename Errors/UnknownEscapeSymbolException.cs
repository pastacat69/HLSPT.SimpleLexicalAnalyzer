using System;

namespace HLSPT.SimpleLexicalAnalyzer.Errors
{
    public class UnknownEscapeSymbolException: Exception
    {
        public UnknownEscapeSymbolException(string message):base(message) { }
    }
}