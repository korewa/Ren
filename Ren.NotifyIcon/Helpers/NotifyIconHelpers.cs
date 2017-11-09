using Ren.NotifyIcon.Interop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Ren.NotifyIcon.Helpers
{
    public static class NotifyIconHelpers
    {
        public static bool SetIconData(ref NotifyIconData data, NotifyIconMessage message, NotifyIconFlags flags)
        {
            data.uFlags = flags;
            return Native.Shell_NotifyIcon(message, ref data);
        }

        public static Icon GetIconFromImageSource(ImageSource imageSource)
        {
            if (imageSource == null)
                return default(Icon);

            var info = Application.GetResourceStream(new Uri($"{imageSource}"));

            if (info == null)
                throw new ArgumentNullException($"{imageSource}: couldn't be resolved.");

            return new Icon(info.Stream);
        }

        public static NotifyIconData GetDefaultNotifyIconData(IntPtr handle)
        {
            var data = new NotifyIconData();

            data.cbSize = (uint)Marshal.SizeOf(data);
            data.hWnd = handle;
            data.hIcon = IntPtr.Zero;
            data.dwState = NotifyIconState.Hidden;
            data.dwStateMask = NotifyIconState.Hidden;
            data.uCallbackMessage = 0x0400;
            data.uFlags = NotifyIconFlags.Message | NotifyIconFlags.Icon | NotifyIconFlags.Tip;

            return data;
        }

        public static bool IsInDesignMode => (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
    }
}