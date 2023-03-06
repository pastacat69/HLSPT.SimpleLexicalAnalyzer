using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HLSPT.SimpleLexicalAnalyzer
{
    public class Token
    {
        private StringBuilder TokenText { get; set; }
        public  TokenType Type { get; set; }
        public int  StartOffset { get; set; }
        public int EndOffset { get; set; }
        public int LineNumber { get; set; } = 0;
        public Token()
        {
            TokenText = new StringBuilder();
            Type = TokenType.WHITE_SPACE;
        }

        public void Add(char symbol) => TokenText.Append(symbol);

        public void Add(string str) => TokenText.Append(str);
        
        public void Clean() => TokenText.Clear();
        public string GetTokenizedText() => TokenText.ToString();

    }
}
