using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using Messenger.Lib.Infrastructure;

namespace Messenger.Lib.Services
{
    class InjectScriptService : IInjectScriptService
    {
        public InjectScriptService()
        {
            this.initializeLock = new AsyncLock();
        }
        
        private readonly AsyncLock initializeLock;
        private string customScript;

        public Task InitializeAsync()
        {
            return this.GetScriptAsync();
        }

        public async Task<string> GetScriptAsync()
        {
            using (await this.initializeLock.LockAsync())
            {
                // If we already built the script, return it directly.
                if (!string.IsNullOrWhiteSpace(this.customScript))
                    return this.customScript;

                // Otherwise, build it from our resources.
                using (var cssStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Messenger.css")).Stream)
                using (var jsStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Messenger.js")).Stream)
                using (var cssReader = new StreamReader(cssStream))
                using (var jsRead = new StreamReader(jsStream))
                {
                    // Read the files.
                    var cssContent = await cssReader.ReadToEndAsync();
                    var jsContent = await jsRead.ReadToEndAsync();

                    // Combine them and return.
                    this.customScript = $"window.messengerCSS = \"{HttpUtility.JavaScriptStringEncode(cssContent)}\";\n {jsContent}";
                    return this.customScript;
                }
            }
        }
    }
}