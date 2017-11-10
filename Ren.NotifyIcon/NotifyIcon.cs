using Ren.NotifyIcon.Helpers;
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
            get => _icon;
            set
            {
                _icon = value;
                _data.hIcon = value != null ? _icon.Handle : IntPtr.Zero;
                NotifyIconHelpers.SetIconData(ref _data, NotifyIconMessage.Modify, NotifyIconFlags.Icon);
            }
        }

        private string _tip;

        public string Tip
        {
            get => _tip;
            set
            {
                _tip = value;
                _data.szTip = value ?? null;
                NotifyIconHelpers.SetIconData(ref _data, NotifyIconMessage.Modify, NotifyIconFlags.Tip);
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
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }

        public static readonly DependencyProperty ContextMenuOpenMouseButtonProperty = DependencyProperty.Register(nameof(ContextMenuOpenMouseButton),
            typeof(NotifyIconMouseButton),
            typeof(NotifyIcon),
            new FrameworkPropertyMetadata(NotifyIconMouseButton.RightButton));

        [Category(Category)]
        public NotifyIconMouseButton ContextMenuOpenMouseButton
        {
            get => (NotifyIconMouseButton)GetValue(ContextMenuOpenMouseButtonProperty);
            set => SetValue(ContextMenuOpenMouseButtonProperty, value);
        }

        public static readonly DependencyProperty NotifyIconToolTipProperty = DependencyProperty.Register(nameof(NotifyIconToolTip),
            typeof(string),
            typeof(NotifyIcon),
            new FrameworkPropertyMetadata(NotifyIconToolTipPropertyChanged));

        private static void NotifyIconToolTipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (NotifyIcon)d;
            obj.OnNotifyIconToolTipPropertyChanged(e);
        }

        private void OnNotifyIconToolTipPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!NotifyIconHelpers.IsInDesignMode)
                Tip = (string)e.NewValue;
        }

        [Category(Category)]
        public string NotifyIconToolTip
        {
            get => (string)GetValue(NotifyIconToolTipProperty);
            set => SetValue(NotifyIconToolTipProperty, value);
        }

        #endregion Dependency Properties

        #region Routed Events

        public static readonly RoutedEvent NotifyIconLeftButtonMouseDownEvent = EventManager.RegisterRoutedEvent(nameof(NotifyIconLeftButtonMouseDown),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(NotifyIcon));

        public event RoutedEventHandler NotifyIconLeftButtonMouseDown
        {
            add { AddHandler(NotifyIconLeftButtonMouseDownEvent, value); }
            remove { RemoveHandler(NotifyIconLeftButtonMouseDownEvent, value); }
        }

        public static readonly RoutedEvent NotifyIconRightButtonMouseDownEvent = EventManager.RegisterRoutedEvent(nameof(NotifyIconRightButtonMouseDown),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(NotifyIcon));

        public event RoutedEventHandler NotifyIconRightButtonMouseDown
        {
            add { AddHandler(NotifyIconRightButtonMouseDownEvent, value); }
            remove { RemoveHandler(NotifyIconRightButtonMouseDownEvent, value); }
        }

        #endregion Routed Events

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

        private void OnMouseButtonEventReceived(NotifyIconMouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case NotifyIconMouseButton.LeftButton:
                    NotifyIconHelpers.RaiseEvent(this, NotifyIconLeftButtonMouseDownEvent);
                    break;

                case NotifyIconMouseButton.RightButton:
                    NotifyIconHelpers.RaiseEvent(this, NotifyIconRightButtonMouseDownEvent);
                    break;

                default:
                    break;
            }

            if (!Equals(ContextMenuOpenMouseButton, mouseButton))
                return;

            var position = new NativePoint();
            Native.GetCursorPos(ref position);

            ShowContextMenu(position);
        }

        private void ShowContextMenu(NativePoint position)
        {
            if (ContextMenu == null)
                return;

            ContextMenu.Placement = PlacementMode.AbsolutePoint;
            ContextMenu.HorizontalOffset = position.X;
            ContextMenu.VerticalOffset = position.Y;
            ContextMenu.IsOpen = true;

            var source = (HwndSource)PresentationSource.FromVisual(ContextMenu);
            if (source == null)
                return;

            Native.SetForegroundWindow(source.Handle);
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