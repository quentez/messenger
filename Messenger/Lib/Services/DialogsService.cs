using System.Windows.Forms;
using Messenger.Lib.Services.Interop;
using Window = System.Windows.Window;

namespace Messenger.Lib.Services
{
    class DialogsService : IDialogsService
    {
        public DialogsService()
        {
            // Theme the messagebox to match the OS style.
            Application.EnableVisualStyles();
        }

        public bool AskUser(Window window, string message, string caption)
        {
            // Create interop window object.
            var window32 = new Wpf32Window(window);
            
            // Display a WinForms MessageBox.
            return MessageBox.Show(window32, message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}