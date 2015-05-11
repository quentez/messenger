using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Messenger.ViewModel
{
    public class NotificationItemViewModel : ViewModelBase
    {
        #region Constructor.

        public NotificationItemViewModel()
        {
            this.isPlaying = true;
            this.ClickCommand = new RelayCommand(() =>
            {
                // Trigger the corresponding action.
                this.ClickAction?.Invoke();

                // Remove this notification.
                this.IsPlaying = false;
            });
            this.LifecycleCompletedCommand = new RelayCommand(() => this.IsPlaying = false);
        }

        private bool isPlaying;

        #endregion

        #region Public properties.

        public string Title { get; set; }

        public string Description { get; set; }

        public Action ClickAction { get; set; }

        public int VisualIndex { get; set; }

        public bool IsPlaying
        {
            get { return this.isPlaying; }
            private set { this.Set(() => this.IsPlaying, ref this.isPlaying, value); }
        }

        #endregion

        #region

        public RelayCommand ClickCommand { get; }
        public RelayCommand LifecycleCompletedCommand { get; }

        #endregion
    }
}