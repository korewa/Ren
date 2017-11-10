using System;
using System.Runtime.InteropServices;

namespace Ren.NotifyIcon.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NotifyIconData
    {
        public uint cbSize;

        public IntPtr hWnd;

        public uint uID;

        public NotifyIconFlags uFlags;

        public uint uCallbackMessage;

        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;

        public NotifyIconState dwState;

        public NotifyIconState dwStateMask;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
        public string szInfo;

        public uint uTimeout;

        public uint uVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 48)]
        public string szInfoTitle;

        public NotifyIconInfoFlags dwInfoFlags;

        public Guid guidItem;

        public IntPtr hBalloonIcon;
    }
}