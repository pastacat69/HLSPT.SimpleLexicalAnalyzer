using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HLSPT.SimpleLexicalAnalyzer.Reader
{
    public class FileWorker
    {
        public async Task<string> ReadFileAsync(string path = null)
        {
            var bytes = await File.ReadAllBytesAsync(path);
            return Encoding.Default.GetString(bytes);     
        }

        public async Task CreateFileAsync(string data,string destinationPath = null)
        {
            await using var fs = new FileStream(destinationPath, FileMode.OpenOrCreate);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            await fs.WriteAsync(dataBytes, 0, dataBytes.Length);
        }
    }
}
