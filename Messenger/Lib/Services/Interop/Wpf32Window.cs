using System;
using System.Windows;
using System.Windows.Interop;
using Messenger.Lib.Infrastructure;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace Messenger.Lib.Services.Interop
{
    public class Wpf32Window : IWin32Window
    {
        public Wpf32Window(Window baseWindow)
        {
            Ensure.Argument.IsNotNull(baseWindow, "baseWindow");
            this.Handle = new WindowInteropHelper(baseWindow).Handle;
        }

        public IntPtr Handle { get; }
    }
}