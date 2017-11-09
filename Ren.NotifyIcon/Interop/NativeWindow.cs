using System;
using System.Runtime.InteropServices;

namespace Ren.NotifyIcon.Interop
{
    public class NativeWindow : IDisposable
    {
        #region Fields

        private WindowProc _messageReceiver;

        #endregion Fields

        #region Properties

        public IntPtr Handle { get; set; }

        public string ClassName { get; set; }

        #endregion Properties

        #region Events

        public event Action<MouseButton> MouseButtonEventReceived;

        #endregion Events

        #region Constructor

        public NativeWindow(bool isDesignMode)
        {
            if (isDesignMode)
                return;

            ClassName = $"{nameof(NotifyIcon)}-{DateTime.Now.Ticks}";
            _messageReceiver = OnMessageReceived;

            var windowClass = new WindowClassEx() { cbSize = (uint)Marshal.SizeOf(typeof(WindowClassEx)), lpszClassName = ClassName, lpfnWndProc = _messageReceiver };

            Native.RegisterClassEx(ref windowClass);

            Handle = Native.CreateWindowEx(0, ClassName, string.Empty, 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        #endregion Constructor

        #region Methods

        //public static NativeWindow GetEmptyNativeWindow()
        //{
        //    return new NativeWindow(true) { Handle = IntPtr.Zero };
        //}

        private IntPtr OnMessageReceived(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            ProcessReceivedMessage(uMsg, wParam, lParam);
            return Native.DefWindowProc(hWnd, uMsg, wParam, lParam);
        }

        private void ProcessReceivedMessage(uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            if (uMsg != 0x0400)
                return;

            switch (lParam.ToInt32())
            {
                case (int)MouseButton.LeftButton:
                    MouseButtonEventReceived(MouseButton.LeftButton);
                    break;

                case (int)MouseButton.RightButton:
                    MouseButtonEventReceived(MouseButton.RightButton);
                    break;

                default:
                    break;
            }
        }

        #endregion Methods

        #region IDisposable

        public bool IsDispose { get; private set; } = false;

        ~NativeWindow()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDispose)
                return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.
            if (Handle == IntPtr.Zero)
            {
                Native.DestroyWindow(Handle);
                Handle = IntPtr.Zero;
            }

            IsDispose = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }
}