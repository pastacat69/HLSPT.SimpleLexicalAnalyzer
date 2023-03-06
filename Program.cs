using HLSPT.SimpleLexicalAnalyzer.Reader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HLSPT.SimpleLexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
           var sc = new FileWorker();
           var source = sc.ReadFileAsync($"{AppDomain.CurrentDomain.BaseDirectory}\\src\\sourcecode.txt").GetAwaiter().GetResult();
           var tk = new Tokanizer();
           tk.Scan(source);
           sc.CreateFileAsync( tk.PrettifyTokens(), $"{AppDomain.CurrentDomain.BaseDirectory}\\src\\tokanized.txt").GetAwaiter().GetResult();
           tk.PrintTokens();
        }
    }
}
