using System;

namespace HLSPT.SimpleLexicalAnalyzer.Errors
{
    public class OperatorException:Exception
    {
        public OperatorException(string message):base(message){}
    }
}