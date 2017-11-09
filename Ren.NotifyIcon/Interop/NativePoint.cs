using System.Runtime.InteropServices;

namespace Ren.NotifyIcon.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NativePoint
    {
        public int X;
        public int Y;
    }
}