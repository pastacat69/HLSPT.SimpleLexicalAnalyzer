using HLSPT.Api.Services.Interfaces;
using HLSPT.SimpleLexicalAnalyzer.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLSPT.SimpleLexicalAnalyzer
{
    public class Tokanizer : IScanner

    {
        private IList<Token> Tokens { get; set; }
        private Token CurrentToken { get; set; }
        private IDictionary<string, TokenType> KeyWords { get; set; }

        public Tokanizer()
        {
            Tokens = new List<Token>();
            CurrentToken = new Token();
            KeyWords = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.IF},
            {"for", TokenType.FOR},
            {"do", TokenType.DO},
            {"while", TokenType.WHILE},
            {"var", TokenType.Var},
            {"let", TokenType.Let},
            {"false", TokenType.FALSE},
            {"true", TokenType.TRUE},
            {"else", TokenType.ELSE},
            {"null", TokenType.NULL},
            {"const", TokenType.Const}
        };

        }

        public IEnumerable<Token> Scan(string rawString)
        {
            foreach (char chr in rawString)
            {
                if (Lexema.IsEscpecialCharacter(chr))
                {
                    if (CurrentToken.Type != TokenType.STRING_ESCAPE)
                    {
                        RefreshToken();
                    }

                    switch (chr)
                    {
                        case '\n':
                            CurrentToken.Add('\n');
                            CurrentToken.Type = TokenType.STRING_ESCAPE;
                            break;
                        case '\r':
                            CurrentToken.Add('\r');
                            CurrentToken.Type = TokenType.STRING_ESCAPE;
                            break;
                        case '\\':
                            CurrentToken.Add('\\');
                            CurrentToken.Type = TokenType.STRING_ESCAPE;
                            break;
                        case '\t':
                            CurrentToken.Add('\t');
                            CurrentToken.Type = TokenType.STRING_ESCAPE;
                            break;
                        default:
                            throw new UnknownEscapeSymbolException(
                                $"Cannot parse '{chr}' escape character on line {CurrentToken.LineNumber} at {CurrentToken.StartOffset} position");
                    }
                }
                else if (Lexema.IsWhiteSpace(chr))
                {
                    if (CurrentToken.Type == TokenType.String)
                    {
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        RefreshToken();
                    }
                }
                else if (Lexema.IsNumber(chr))
                {
                    if (CurrentToken.Type == TokenType.WHITE_SPACE
                        || CurrentToken.Type == TokenType.Assign || CurrentToken.Type == TokenType.EqOp || CurrentToken.Type == TokenType.STRING_ESCAPE)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.Number;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }

                else if (Lexema.IsGreaterThenOperator(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.Gt;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsLessThenOperator(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.Lt;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsRightBracket(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.RB;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsLeftBracket(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.LB;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsRightParenthese(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.RP;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsLeftParenthese(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.LP;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsPlusOperator(chr))
                {
                    if (CurrentToken.Type == TokenType.AddOp)
                    {
                        CurrentToken.Type = TokenType.IncOp;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.AddOp;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsSubstractOperator(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.DivOp;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsMultOperator(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.MultOp;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsMinusOperator(chr))
                {
                    if (CurrentToken.Type == TokenType.MinOp)
                    {
                        CurrentToken.Type = TokenType.DecOp;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.MinOp;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsAssingOperator(chr))
                {
                    if (CurrentToken.Type == TokenType.Gt)
                    {
                        CurrentToken.Type = TokenType.Gte;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else if (CurrentToken.Type == TokenType.Lt)
                    {
                        CurrentToken.Type = TokenType.Lte;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }

                    else if (CurrentToken.Type == TokenType.Assign)
                    {
                        CurrentToken.Type = TokenType.EqOp;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else if (CurrentToken.Type == TokenType.Not)
                    {
                        CurrentToken.Type = TokenType.NotEqOp;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                    else if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.Assign;
                        CurrentToken.Add(chr);
                    }

                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsDot(chr))
                {
                    if (CurrentToken.Type == TokenType.WHITE_SPACE || CurrentToken.Type == TokenType.Number)
                    {
                        CurrentToken.Type = TokenType.Number;
                        CurrentToken.Add(chr);
                    }
                    else if (CurrentToken.Type == TokenType.String)
                    {
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.DotOperator;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                }
                else if (Lexema.IsDoubleQuotation(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.String;
                        CurrentToken.Add(chr);
                    }
                    else if (CurrentToken.Type == TokenType.String)
                    {
                        RefreshToken();
                    }
                }
                else if (Lexema.IsUnderscore(chr))
                {

                    if (CurrentToken.Type == TokenType.WHITE_SPACE)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.IDENTIFIER;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsExclamationMark(chr))
                {
                    if (CurrentToken.Type != TokenType.String)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.Not;
                        CurrentToken.Add(chr);
                    }
                    else
                    {
                        CurrentToken.Add(chr);
                    }
                }
                else if (Lexema.IsSemiColon(chr))
                {
                    if (CurrentToken.Type == TokenType.String)
                    {
                        CurrentToken.Add(chr);
                    }
                    else if (CurrentToken.Type != TokenType.WHITE_SPACE)
                    {
                        RefreshToken();
                        CurrentToken.Type = TokenType.Sc;
                        CurrentToken.Add(chr);
                        RefreshToken();
                    }
                }
                else if (Lexema.IsCarriageReturnSymbol(chr))
                {
                    RefreshToken();
                    ++CurrentToken.LineNumber;
                }
                else if (CurrentToken.Type == TokenType.WHITE_SPACE || CurrentToken.Type == TokenType.Number || CurrentToken.Type == TokenType.STRING_ESCAPE)
                {
                    RefreshToken();
                    CurrentToken.Type = TokenType.IDENTIFIER;
                    CurrentToken.Add(chr);
                }
                else
                {
                    CurrentToken.Add(chr);
                }
            }

            RefreshToken();
            return Tokens;
        }


        private void RefreshToken()
        {
            if (CurrentToken.Type != TokenType.WHITE_SPACE)
            {
                TokenType? possibleKeyWordToken = GetKeyWord(CurrentToken.GetTokenizedText());

                if (possibleKeyWordToken != null)
                {
                    CurrentToken.Type = possibleKeyWordToken.GetValueOrDefault();
                }

                Tokens.Add(CurrentToken);

                CurrentToken = new Token();
            }

        }

        public string PrettifyTokens()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var token in Tokens)
            {
                if (token.Type != TokenType.STRING_ESCAPE)
                {
                    sb.Append(token.Type.ToString());
                    sb.Append(" ");
                }
                else
                {
                    sb.Append("\r\n");
                }

            }

            return sb.ToString();
        }

        public void PrintTokens()
        {
            foreach (var token in Tokens)
            {
                Console.Write($" {token.Type.ToString()} ");
            }
        }

        private TokenType? GetKeyWord(string key)
        {
            if (KeyWords.ContainsKey(key))
            {
                return KeyWords[key];
            }

            return null;
        }


        public static class Lexema
        {

            public static bool IsMinusOperator(char op)
            {
                return op == '-';
            }


            public static bool IsPlusOperator(char op)
            {
                return op == '+';
            }

            public static bool IsDivOperator(char op)
            {
                return op == '/';
            }

            public static bool IsMultOperator(char op)
            {
                return op == '*';
            }

            public static bool IsWhiteSpace(char escape)
            {
                return (escape == '\t' || escape == ' ');
            }

            public static bool IsCarriageReturnSymbol(char cr)
            {
                return cr == '\r' || cr == '\n';
            }

            public static bool IsEscpecialCharacter(char es)
            {
                return es == '\r' || es == '\n' || es == '\t';
            }
            public static bool IsSingleQuotation(char quotation)
            {
                return quotation == '\'';
            }

            public static bool IsDoubleQuotation(char doubleQuotation)
            {
                return doubleQuotation == '\"';
            }

            public static bool IsNumber(char number)
            {
                return (number >= '0' && number <= '9');
            }

            public static bool IsLeftParenthese(char parenthese)
            {
                return parenthese == '(';
            }

            public static bool IsRightParenthese(char parenthese)
            {
                return parenthese == ')';
            }

            public static bool IsLeftBracket(char bracket)
            {
                return bracket == '{';
            }

            public static bool IsRightBracket(char bracket)
            {
                return bracket == '}';
            }

            public static bool IsAssingOperator(char equal)
            {

                return equal == '=';
            }

            public static bool IsSubstractOperator(char sub)
            {
                return sub == '/';
            }

            public static bool IsDot(char dot)
            {
                return dot == '.';
            }

            public static bool IsGreaterThenOperator(char _operator)
            {
                return _operator == '>';
            }

            public static bool IsLessThenOperator(char _operator)
            {
                return _operator == '<';
            }

            public static bool IsUnderscore(char underscore)
            {
                return underscore == '_';
            }

            public static bool IsSemiColon(char semicolon)
            {
                return semicolon == ';';
            }

            public static bool IsExclamationMark(char exclamation)
            {
                return exclamation == '!';
            }
        }
    }
}
