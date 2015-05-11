using System;
using System.Deployment.Application;
using System.IO;

namespace Messenger.Lib.Services
{
    internal class AppConstants : IAppConstants
    {
        public AppConstants()
        {
            // Set the UserAgent and if needed, add the deployment version to it.
            this.UserAgentAppName = "MessengerForWindows";
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                this.UserAgentAppName += '/' + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
        }

        public string AppPath => AppDomain.CurrentDomain.BaseDirectory;

        public string DownloadsPath => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory, Environment.SpecialFolderOption.Create);

        public string DownloadsTempPath
        {
            get
            {
                var tempDirectoryPath = Path.Combine(Path.GetTempPath(), "MessengerForWindows");
                Directory.CreateDirectory(tempDirectoryPath);
                return tempDirectoryPath;
            }
        }

        public string BrowserCachePath
        {
            get
            {
                var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MessengerForWindows", "BrowserCache");
                Directory.CreateDirectory(appDataPath);
                return appDataPath;
            }
        }

        public string SignInPath => "/login";
        public string AppUrl => "https://www.messenger.com";
        public string SignInUrl => "https://www.facebook.com/login/messenger_dot_com_iframe";
        public string ExternalAllowedUrl => "https://www.facebook.com";

        public string UserAgent => $"Mozilla/5.0 (Windows NT {this.UserAgentOsVersion}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36 {this.UserAgentAppName}";
        public string UserAgentAppName { get; }
        public string UserAgentOsVersion => $"{Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}";
    }
}