namespace Messenger.Lib.Services
{
    public interface IAppConstants
    {
        string AppPath { get; }
        string DownloadsPath { get; }
        string DownloadsTempPath { get; }
        string BrowserCachePath { get; }
        string SignInPath { get; }
        string AppUrl { get; }
        string SignInUrl { get; }
        string ExternalAllowedUrl { get; }
        string UserAgent { get; }
    }
}