using System.Windows;
using System.Windows.Controls;
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
            this.overlayControl = new TaskBarOverlay {DataContext = this.overlayViewModel};
            this.overlayContainer = new Viewbox {Child = this.overlayControl};
        }

        private readonly IDispatcherService dispatcherService;
        private ImageSource taskBarOverlay;

        private readonly Viewbox overlayContainer;
        private readonly TaskBarOverlay overlayControl;
        private readonly TaskBarOverlayViewModel overlayViewModel;

        #endregion

        public void UpdateBadgeOverlay(int newUnreadCount)
        {
            // Run this on the UI thread.
            this.dispatcherService.RunOnMainThead(() =>
            {
                // If this is the same value, nothing to do.
                if (newUnreadCount == this.overlayViewModel.Count)
                {
                    return;
                }
                // If it's 0, clear the overlay.
                if (newUnreadCount == 0)
                {
                    this.overlayViewModel.Count = 0;
                    this.TaskBarOverlay = null;
                    return;
                }

                // Otherwise, update it.
                this.overlayViewModel.Count = (uint)newUnreadCount;
                this.overlayViewModel.IsCountVisible = true;

                // Let the UI system render and bind our control.
                this.overlayContainer.Measure(new Size(this.overlayControl.Width, this.overlayControl.Height));
                this.overlayContainer.Arrange(new Rect(0, 0, this.overlayControl.Width, this.overlayControl.Height));
                this.overlayContainer.UpdateLayout();

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