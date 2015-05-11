using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CefSharp;
using Messenger.Lib.Infrastructure;

namespace Messenger.Lib.Services
{
    class CefConfigureService : ICefConfigureService
    {
        #region Constructor & Private variables.

        public CefConfigureService(IAppConstants appConstants)
        {
            Ensure.Argument.IsNotNull(appConstants, "appConstants");
            this.appConstants = appConstants;
        }

        private readonly IAppConstants appConstants;

        #endregion

        #region Interface implementation.

        public void Configure()
        {
            // Create the settings that will be applied to all our CEF browsers.
            var settings = new CefSettings
            {
                RemoteDebuggingPort = 8088,
                LogSeverity = LogSeverity.Verbose,
                CachePath = this.appConstants.BrowserCachePath,
                UserAgent = this.appConstants.UserAgent,
                Locale = this.FindLocale()
            };
            //settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
            //settings.CefCommandLineArgs.Add("renderer-startup-dialog", "renderer-startup-dialog");
            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1");
            settings.CefCommandLineArgs.Add("enable-media-stream", "enable-media-stream");

            // Try to initialize the CEF component.
            if (!Cef.Initialize(settings))
            {
                throw new Exception("Unable to Initialize Cef.");
            }
        }

        public void Shutdown()
        {
            Cef.Shutdown();
        }

        #endregion

        #region Private methods.

        private string FindLocale()
        {
            // Find the current culture and the available locales.
            var culture = CultureInfo.CurrentUICulture;
            var locales = Directory.GetFiles(Path.Combine(this.appConstants.AppPath, "locales"))
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();

            // Check if we have an exact match for our culture.
            var exactLocale = locales.FirstOrDefault(l => string.Equals(l, culture.Name, StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(exactLocale))
            {
                return exactLocale;
            }

            // Otherwise, check if we have a language match for our culture.
            var languageLocale = locales.FirstOrDefault(l => l.StartsWith(culture.TwoLetterISOLanguageName, StringComparison.InvariantCultureIgnoreCase));
            if (!string.IsNullOrWhiteSpace(languageLocale))
            {
                return languageLocale;
            }

            // Finally, if nothing was found, default to english.
            return "en-US";
        }

        #endregion
    }
}