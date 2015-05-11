using System;
using System.Deployment.Application;
using System.IO;
using System.Windows;

namespace Messenger.Lib.Services
{
    internal class AppConstants : IAppConstants
    {
        public AppConstants()
        {
            // Set the UserAgent and if needed, add the deployment version to it.
            this.UserAgentAppName = "MessengerDotnet";
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
                var tempDirectoryPath = Path.Combine(Path.GetTempPath(), "FrontApp");
                Directory.CreateDirectory(tempDirectoryPath);
                return tempDirectoryPath;
            }
        }

        public string BrowserCachePath
        {
            get
            {
                var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Front", "BrowserCache");
                Directory.CreateDirectory(appDataPath);
                return appDataPath;
            }
        }

        public string SignInPath => "/login";
        public string AppUrl => "https://www.messenger.com";
        public string SignInUrl => "https://www.facebook.com/login/messenger_dot_com_iframe";

        public string UserAgent => $"Mozilla/5.0 (Windows NT {this.UserAgentOsVersion}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36 {this.UserAgentAppName}";
        public string UserAgentAppName { get; }
        public string UserAgentOsVersion => $"{Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}";
        public Size PopupSettingsSize => new Size(700, 600);
        public Size PopupCardsSize => new Size(800, 600);
        public Size PopupCardsMinSize => new Size(700, 550);
        public Size PopupComposeSize => new Size(700, 600);
        public Size PopupComposeMinSize => new Size(600, 550);
        public Size PopupAddRestoreInboxSize => new Size(600, 500);
        public Size PopupGenericSize => new Size(700, 600);
        public string PopupBootDataMethodName => "window.popupapi.getBootStr();";
        public string PopupLastDraftMethodName => "window.popupapi.getLastDraftStr();";
        public string IsDraftSavedMethodName => "window.jsapi.draftSaved();";
        public string OpenMethodName => "window.jsapi.open(\"{0}\");";
        public string OpenDraftMethodName => "window.popupapi.openDraft({0}, false);";
        public string OpenSearchMethodName => "window.jsapi.search(\"{0}\");";
        public string OpenContextMenuMethodName => "window.openContextMenu({0}, {1}, {2}, \"{3}\", {4});";
        public string ComposerDidCloseMethodName => "window.jsapi.composerDidClose({0});";
        public string DownloadDidProgressMethodName => "window.jsapi.downloadDidProgress(\"{0}\", {1}, \"{2}\");";
        public string DownloadDidCompleteMethodName => "window.jsapi.downloadDidComplete(\"{0}\", \"{1}\", \"{2}\");";
        public string DownloadDidFailMethodName => "window.jsapi.downloadDidFail(\"{0}\", \"{1}\");";
    }
}