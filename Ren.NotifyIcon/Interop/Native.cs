using System;
using System.Runtime.InteropServices;

namespace Ren.NotifyIcon.Interop
{
    internal static class Native
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
            uint dwExStyle,
            [MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
            uint dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(
            IntPtr hWnd,
            uint uMsg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyWindow(
            IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetCursorPos(ref NativePoint lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern ushort RegisterClassEx(
            [In] ref WindowClassEx lpwcx);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool Shell_NotifyIcon(
            NotifyIconMessage message,
            [In] ref NotifyIconData data);
    }
}