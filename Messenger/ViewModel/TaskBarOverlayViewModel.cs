using GalaSoft.MvvmLight;

namespace Messenger.ViewModel
{
    public class TaskBarOverlayViewModel : ViewModelBase
    {
        private uint count;
        private bool isCountVisible;

        public uint Count
        {
            get { return this.count; }
            set
            {
                this.Set(() => this.Count, ref this.count, value);
                this.RaisePropertyChanged(() => this.DisplayCount);
            }
        }

        public string DisplayCount => this.count > 99 ? "99" : this.count.ToString();

        public bool IsCountVisible
        {
            get { return this.isCountVisible; }
            set { this.Set(() => this.IsCountVisible, ref this.isCountVisible, value); }
        }
    }
}