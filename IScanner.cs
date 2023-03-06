using System.Collections.Generic;
using HLSPT.SimpleLexicalAnalyzer;

namespace HLSPT.Api.Services.Interfaces
{
    public interface IScanner
    {
        IEnumerable<Token> Scan(string rawString);
    }
}