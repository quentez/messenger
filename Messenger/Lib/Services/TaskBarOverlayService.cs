using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using Messenger.Controls;
using Messenger.Lib.Infrastructure;
using Messenger.ViewModel;

namespace Messenger.Lib.Services
{
    class TaskBarOverlayService : ObservableObject, ITaskBarOverlayService
    {
        #region Constructor & Private fields.

        public TaskBarOverlayService(IDispatcherService dispatcherService)
        {
            Ensure.Argument.IsNotNull(dispatcherService, "dispatcherService");
            this.dispatcherService = dispatcherService;

            // Create the control we'll be using to render the overlay,
            // along with its ViewModel.
            this.overlayViewModel = new TaskBarOverlayViewModel();
            this.overlayControl = new TaskBarOverlay { DataContext = this.overlayViewModel };
        }

        private readonly IDispatcherService dispatcherService;
        private ImageSource taskBarOverlay;

        private readonly TaskBarOverlay overlayControl;
        private readonly TaskBarOverlayViewModel overlayViewModel;

        #endregion

        public void UpdateBadgeOverlay(string newUnreadCount)
        {
            // Run this on the UI thread.
            this.dispatcherService.RunOnMainThead(() =>
            { 
                // If the provided string is null or empty, clear the overlay.
                if (string.IsNullOrWhiteSpace(newUnreadCount))
                {
                    this.TaskBarOverlay = null;
                    return;
                }
             
                // Try to parse the new unread count.
                uint newUnreadCountValue;
                if (uint.TryParse(newUnreadCount, out newUnreadCountValue))
                {
                    // If this is the same value, nothing to do.
                    if (newUnreadCountValue == this.overlayViewModel.Count)
                    {
                        return;
                    }
                    // If it's 0, clear the overlay.
                    if (newUnreadCountValue == 0)
                    {
                        this.TaskBarOverlay = null;
                        return;
                    }

                    // Otherwise, update it.
                    this.overlayViewModel.Count = newUnreadCountValue;
                    this.overlayViewModel.IsCountVisible = true;
                }
                // Otherwise, display a simple dot.
                else if (newUnreadCount.Length == 1)
                {
                    this.overlayViewModel.IsCountVisible = false;
                }
                
                // Let the UI system render and bind our control.
                this.overlayControl.Measure(new Size(this.overlayControl.Width, this.overlayControl.Height));
                this.overlayControl.Arrange(new Rect(0, 0, this.overlayControl.Width, this.overlayControl.Height));
                this.dispatcherService.RunAllRenderTasks();

                var renderBitmap = new RenderTargetBitmap((int)this.overlayControl.Width, (int)this.overlayControl.Height, 96, 96, PixelFormats.Default);
                renderBitmap.Render(this.overlayControl);
                this.TaskBarOverlay = renderBitmap;
            });
        }

        #region Observable properties.

        public ImageSource TaskBarOverlay
        {
            get { return this.taskBarOverlay; }
            set { this.Set(() => this.TaskBarOverlay, ref this.taskBarOverlay, value); }
        }

        #endregion
    }
}