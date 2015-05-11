using System;
using System.Diagnostics;
using System.IO;

namespace Messenger.Lib.Services
{
    class ExternalProcessService : IExternalProcessService
    {
        public void OpenUrl(string url)
        {
            // Validate that we were given a valid URL.
            Uri validUri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out validUri)
                || (validUri.Scheme != Uri.UriSchemeHttp && validUri.Scheme != Uri.UriSchemeHttps && validUri.Scheme != Uri.UriSchemeMailto))
            {
                return;
            }

            // Open the provided URL using the system-associated app.
            Process.Start(validUri.AbsoluteUri);
        }

        public void OpenFile(string filename)
        {
            // Check if a file does exist at the specified location.
            if (!File.Exists(filename))
            {
                return;
            }

            // Open the file.
            Process.Start(filename);
        }
    }
}