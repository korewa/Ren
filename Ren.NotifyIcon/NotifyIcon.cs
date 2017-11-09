﻿using Ren.NotifyIcon.Helpers;
using Ren.NotifyIcon.Interop;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;

namespace Ren.NotifyIcon
{
    public class NotifyIcon : FrameworkElement, IDisposable
    {
        #region Constants

        private const string Category = nameof(NotifyIcon);

        #endregion Constants

        #region Fields

        private NotifyIconData _data;

        private bool _exists;

        private NativeWindow _window = new NativeWindow(NotifyIconHelpers.IsInDesignMode);

        #endregion Fields

        #region Properties

        private Icon _icon;

        public Icon Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                _data.hIcon = value != null ? _icon.Handle : IntPtr.Zero;
                NotifyIconHelpers.SetIconData(ref _data, NotifyIconMessage.Modify, NotifyIconFlags.Icon);
            }
        }

        #endregion Properties

        #region Dependency Properties

        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register(nameof(IconSource),
            typeof(ImageSource),
            typeof(NotifyIcon),
            new FrameworkPropertyMetadata(IconSourcePropertyChanged));

        private static void IconSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (NotifyIcon)d;
            obj.OnIconSourcePropertyChanged(e);
        }

        private void OnIconSourcePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!NotifyIconHelpers.IsInDesignMode)
                Icon = NotifyIconHelpers.GetIconFromImageSource((ImageSource)e.NewValue);
        }

        [Category(Category)]
        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        #endregion Dependency Properties

        #region Constructor

        public NotifyIcon()
        {
            _data = NotifyIconHelpers.GetDefaultNotifyIconData(_window.Handle);

            _window.MouseButtonEventReceived += OnMouseButtonEventReceived;

            NotifyIconHelpers.SetIconData(ref _data, NotifyIconMessage.Add, NotifyIconFlags.Message | NotifyIconFlags.Icon | NotifyIconFlags.Tip);

            _exists = true;
        }

        #endregion Constructor

        #region Methods

        private void OnMouseButtonEventReceived(MouseButton mouseButton)
        {
            if (!Equals(mouseButton, MouseButton.RightButton))
                return;

            var position = new NativePoint();
            Native.GetPhysicalCursorPos(ref position);

            ShowContextMenu(position);
        }

        // rewrite
        private void ShowContextMenu(NativePoint position)
        {
            if (ContextMenu != null)
            {
                ContextMenu.Placement = PlacementMode.AbsolutePoint;
                ContextMenu.HorizontalOffset = position.X;
                ContextMenu.VerticalOffset = position.Y;
                ContextMenu.IsOpen = true;

                var handle = IntPtr.Zero;

                var source = (HwndSource)PresentationSource.FromVisual(ContextMenu);
                if (source != null)
                {
                    handle = source.Handle;
                }

                Native.SetForegroundWindow(handle);
            }
        }

        private void RemoveNotifyIcon()
        {
            if (!_exists)
                return;

            NotifyIconHelpers.SetIconData(ref _data, NotifyIconMessage.Delete, NotifyIconFlags.Message);

            _window.MouseButtonEventReceived -= OnMouseButtonEventReceived;

            _exists = false;
        }

        #endregion Methods

        #region IDisposable

        public bool IsDispose { get; private set; } = false;

        ~NotifyIcon()
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
            RemoveNotifyIcon();
            _window.Dispose();
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