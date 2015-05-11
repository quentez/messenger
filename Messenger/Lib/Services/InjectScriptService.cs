using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Messenger.Lib.Services
{
    class InjectScriptService : IInjectScriptService
    {
        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetScriptAsync()
        {
            using (var cssFile = File.OpenText("Resources/Messenger.css"))
            using (var jsFile = File.OpenText("Resources/Messenger.js"))
            {
                // Read the files.
                var cssContent = await cssFile.ReadToEndAsync();
                var jsContent = await jsFile.ReadToEndAsync();

                // Combine them and return.
                return $"window.messengerCSS = \"{HttpUtility.JavaScriptStringEncode(cssContent)}\";\n {jsContent}";
            }
        }
    }
}