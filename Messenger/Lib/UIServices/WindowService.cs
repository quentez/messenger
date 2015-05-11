using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Messenger.Lib.Infrastructure;
using Messenger.Lib.Services;

namespace Messenger.Lib.UIServices
{
    class WindowService : IWindowService
    {
        #region Constructor & Private variables.

        public WindowService(IDispatcherService dispatcherService)
        {
            Ensure.Argument.IsNotNull(dispatcherService, "dispatcherService");
            this.dispatcherService = dispatcherService;
        }

        private readonly IDispatcherService dispatcherService;

        #endregion

        #region Interface implementation.

        public void ActivateMainWindow()
        {
            // Activate the main window on the UI thread.
            this.dispatcherService.RunOnMainThead(() =>
            {
                var mainWindow = Application.Current.MainWindow;

                // If needed, un-minimize the window first.
                // This uses a hack because of a WPF issue that prevented me from restoring properly.
                var hwnd = ((HwndSource)PresentationSource.FromVisual(mainWindow)).Handle;
                ShowWindow(hwnd, ShowWindowCommands.Restore);

                // Then activate it.
                mainWindow.Activate();
            });
        }

        #endregion

        #region Marshalling.

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        private enum ShowWindowCommands : int
        {
            /// <summary>
            /// Activates and displays the window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position. 
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
        }

        #endregion
    }
}