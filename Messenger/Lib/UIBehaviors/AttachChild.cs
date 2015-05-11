using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Interactivity;

namespace Messenger.Lib.UIBehaviors
{
    /// <summary>
    /// XAML Behavior to bind the Child property of a WindowsFormsHost.
    /// </summary>
    public class AttachChild : Behavior<WindowsFormsHost>
    {
        /// <summary>
        /// When the behavior is first attached, set the Child a first time.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.UpdateChild();
        }

        /// <summary>
        /// Update the Child of the attached WindowsFormsHost using the provided Control.
        /// </summary>
        private void UpdateChild()
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            this.AssociatedObject.Child = this.Control;
        }

        /// <summary>
        /// The Control to attach.
        /// </summary>
        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
            "Control", typeof (System.Windows.Forms.Control), typeof (AttachChild), new PropertyMetadata(default(Control), (s, e) => ((AttachChild)s).UpdateChild()));

        /// <summary>
        /// Gets or Sets the Control to attach.
        /// </summary>
        public System.Windows.Forms.Control Control
        {
            get { return (System.Windows.Forms.Control)this.GetValue(ControlProperty); }
            set { this.SetValue(ControlProperty, value); }
        }
    }
}