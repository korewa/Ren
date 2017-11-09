using System;

namespace Ren.NotifyIcon.Interop
{
    [Flags]
    public enum NotifyIconFlags
    {
        Message = 0x01,
        Icon = 0x02,
        Tip = 0x04,
        State = 0x08,
        Info = 0x10,
        Guid = 0x20,
        Realtime = 0x40,
        Showtip = 0x80
    }
}