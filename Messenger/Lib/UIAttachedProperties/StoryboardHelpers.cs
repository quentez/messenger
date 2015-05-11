using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Messenger.Lib.UIAttachedProperties
{
    // This is a hack to bind a command on a storyboard's completed event.
    public static class StoryboardHelpers
    {
        public static readonly DependencyProperty TargetStoryboardProperty = DependencyProperty.RegisterAttached(
            "TargetStoryboard", typeof (Storyboard), typeof (StoryboardHelpers), new PropertyMetadata(default(Storyboard), Element_StoryboardChanged));

        public static void SetTargetStoryboard(DependencyObject element, Storyboard value)
        {
            element.SetValue(TargetStoryboardProperty, value);
        }

        public static Storyboard GetTargetStoryboard(DependencyObject element)
        {
            return (Storyboard)element.GetValue(TargetStoryboardProperty);
        }

        public static readonly DependencyProperty StoryboardCompletedProperty = DependencyProperty.RegisterAttached(
            "StoryboardCompleted", typeof (ICommand), typeof (StoryboardHelpers), new PropertyMetadata(default(ICommand)));

        public static void SetStoryboardCompleted(DependencyObject element, ICommand value)
        {
            element.SetValue(StoryboardCompletedProperty, value);
        }

        public static ICommand GetStoryboardCompleted(DependencyObject element)
        {
            return (ICommand)element.GetValue(StoryboardCompletedProperty);
        }

        private static void Element_StoryboardChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            // Register our event handler on the new storyboard.
            ((Storyboard)e.NewValue).Completed += (s2, e2) =>
            {
                // Try and retrieve the corresponding command.
                var elementCommand = element.GetValue(StoryboardCompletedProperty) as ICommand;
                if (elementCommand == null || !elementCommand.CanExecute(null))
                {
                    return;
                }

                // Execute the target command.
                elementCommand.Execute(null);
            };

        }
    }
}