using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Messenger.Lib.UIBehaviors
{
    /// <summary>
    /// XAML Behavior to close a window through a binded flag.
    /// </summary>
    public class CloseWindow : Behavior<Window>
    {
        #region Behavior boilerplate.

        /// <summary>
        /// When the behavior is first attached, register for events on the window.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Closing += this.AssociatedObjectOnClosing;
            this.AssociatedObject.Closed += this.AssociatedObjectOnClosed;
        }

        /// <summary>
        /// Unregister events on the window when we get dettached.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Closing -= this.AssociatedObjectOnClosing;
            this.AssociatedObject.Closed -= this.AssociatedObjectOnClosed;
        }

        #endregion

        #region Event handlers.

        private void AssociatedObjectOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            // If we don't have a BeforeCloseCommand, or if the IsOpen flag is set 
            // to false, close right away.
            if (this.BeforeCloseCommand == null
               || !this.IsOpen)
            {
                return;
            }

            // Otherwise, cancel the event and call the command.
            cancelEventArgs.Cancel = true;
            this.BeforeCloseCommand.Execute(this.AssociatedObject);
        }

        private void AssociatedObjectOnClosed(object sender, EventArgs eventArgs)
        {
            // If we have a command to call, call it now.
            this.ClosedCommand?.Execute(this.AssociatedObject);
        }

        private void IsOpenPropertyChanged()
        {
            // If we don't have an associated object, or if the IsOpen flag is true, nothing to do.
            if (this.AssociatedObject == null || this.IsOpen)
            {
                return;
            }

            // Otherwise, close the window.
            this.AssociatedObject.Close();
        }

        #endregion

        #region Dependency properties.

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            "IsOpen", typeof (bool), typeof (CloseWindow), new PropertyMetadata(default(bool), (s, e) => ((CloseWindow)s).IsOpenPropertyChanged()));

        public bool IsOpen
        {
            get { return (bool) this.GetValue(IsOpenProperty); }
            set { this.SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty BeforeCloseCommandProperty = DependencyProperty.Register(
            "BeforeCloseCommand", typeof (ICommand), typeof (CloseWindow), new PropertyMetadata(default(ICommand)));

        public ICommand BeforeCloseCommand
        {
            get { return (ICommand) this.GetValue(BeforeCloseCommandProperty); }
            set { this.SetValue(BeforeCloseCommandProperty, value); }
        }

        public static readonly DependencyProperty ClosedCommandProperty = DependencyProperty.Register(
            "ClosedCommand", typeof (ICommand), typeof (CloseWindow), new PropertyMetadata(default(ICommand)));

        public ICommand ClosedCommand
        {
            get { return (ICommand) this.GetValue(ClosedCommandProperty); }
            set { this.SetValue(ClosedCommandProperty, value); }
        }

        #endregion
    }
}