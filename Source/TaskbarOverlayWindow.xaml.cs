using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using NetMonitor.ViewModels;

namespace NetMonitor
{
    public partial class TaskbarOverlayWindow : Window
    {
        private readonly MainViewModel _viewModel;
        private DispatcherTimer? _positionTimer;
        private bool _isFirstPositioning = true;
        private bool _isDragging = false;
        private Point _clickStart;
        private HwndSource? _hwndSource;
        private IntPtr _myHwnd = IntPtr.Zero;
        private uint _wmTaskbarCreated = 0;

        private IntPtr _winEventHook = IntPtr.Zero;
        private WinEventDelegate? _winEventProc;

        private const uint SWP_NOSIZE         = 0x0001;
        private const uint SWP_NOMOVE         = 0x0002;
        private const uint SWP_NOZORDER       = 0x0004;
        private const uint SWP_NOACTIVATE     = 0x0010;
        private const uint SWP_NOSENDCHANGING = 0x0400;
        private const int WM_WINDOWPOSCHANGING = 0x0046;

        private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        private const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;
        private const uint WINEVENT_OUTOFCONTEXT = 0x0000;
        private const uint WINEVENT_SKIPOWNPROCESS = 0x0002;

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        private delegate void WinEventDelegate(
            IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x, y, cx, cy;
            public uint flags;
        }

        public TaskbarOverlayWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            _positionTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) };
            _positionTimer.Tick += (s, e) =>
            {
                if (_viewModel.IsTaskbarLocked) UpdatePositionToTray();
                AssertTopmost();
            };
            _positionTimer.Start();
        }

        private void AssertTopmost()
        {
            if (_myHwnd == IntPtr.Zero) return;
            SetWindowPos(_myHwnd, HWND_TOPMOST, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOSENDCHANGING);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _myHwnd = new WindowInteropHelper(this).Handle;

            int exStyle = GetWindowLong(_myHwnd, GWL_EXSTYLE);
            SetWindowLong(_myHwnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE);

            _hwndSource = HwndSource.FromHwnd(_myHwnd);
            _hwndSource.AddHook(WndProc);

            _wmTaskbarCreated = RegisterWindowMessage("TaskbarCreated");

            _winEventProc = new WinEventDelegate(OnWinEvent);
            _winEventHook = SetWinEventHook(
                EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_MINIMIZEEND,
                IntPtr.Zero, _winEventProc, 0, 0,
                WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);

            AssertTopmost();

            if (_viewModel.Settings.TaskbarTop >= 0 && _viewModel.Settings.TaskbarLeft >= 0)
            {
                Top  = _viewModel.Settings.TaskbarTop;
                Left = _viewModel.Settings.TaskbarLeft;
                _isFirstPositioning = false;
            }

            LocationChanged += (s, ev) => SaveLocation();
            SizeChanged     += (s, ev) => SaveLocation();
        }

        private void OnWinEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                AssertTopmost();
                var t = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
                t.Tick += (s, e) => { t.Stop(); AssertTopmost(); };
                t.Start();
            }), DispatcherPriority.Send);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_WINDOWPOSCHANGING)
            {
                object? boxed = Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                if (boxed != null)
                {
                    WINDOWPOS pos = (WINDOWPOS)boxed;
                    if ((pos.flags & SWP_NOZORDER) == 0)
                    {
                        pos.hwndInsertAfter = HWND_TOPMOST;
                        Marshal.StructureToPtr(pos, lParam, false);
                    }
                }
            }

            if (_wmTaskbarCreated != 0 && (uint)msg == _wmTaskbarCreated)
            {
                _isFirstPositioning = true;
                Dispatcher.BeginInvoke(new Action(UpdatePositionToTray), DispatcherPriority.Normal);
            }

            return IntPtr.Zero;
        }

        private void UpdatePositionToTray()
        {
            try
            {
                RECT taskbarRect = GetTaskbarRect();
                if (taskbarRect.right <= 0) return;

                RECT anchorRect = GetClockRect();
                if (anchorRect.right <= 0) anchorRect = GetTrayNotifyRect();
                if (anchorRect.right <= 0) return;

                double dpiScale = GetDpiScale();

                double taskTop    = taskbarRect.top    / dpiScale;
                double taskBottom = taskbarRect.bottom / dpiScale;
                double anchorLeft = anchorRect.left    / dpiScale;
                double tbHeight   = taskBottom - taskTop;

                if (_isFirstPositioning && tbHeight > 0 && tbHeight < 200)
                {
                    Height = tbHeight;
                    _isFirstPositioning = false;
                }

                double newLeft = anchorLeft - ActualWidth - 4;
                double newTop  = taskTop + (tbHeight - ActualHeight) / 2.0;

                if (Math.Abs(Left - newLeft) > 0.5 || Math.Abs(Top - newTop) > 0.5)
                {
                    Left = newLeft;
                    Top  = newTop;
                }
            }
            catch { }
        }

        private double GetDpiScale()
        {
            try
            {
                var source = PresentationSource.FromVisual(this);
                if (source?.CompositionTarget != null)
                    return source.CompositionTarget.TransformToDevice.M11;
            }
            catch { }
            return 1.0;
        }

        protected override void OnClosed(EventArgs e)
        {
            _positionTimer?.Stop();
            _hwndSource?.RemoveHook(WndProc);
            if (_winEventHook != IntPtr.Zero) { UnhookWinEvent(_winEventHook); _winEventHook = IntPtr.Zero; }
            base.OnClosed(e);
        }

        private void SaveLocation()
        {
            if (!_viewModel.IsTaskbarLocked && Top >= 0 && Left >= 0)
            {
                _viewModel.Settings.TaskbarTop  = Top;
                _viewModel.Settings.TaskbarLeft = Left;
                NetMonitor.Models.SettingsManager.SaveSettings(_viewModel.Settings);
            }
        }

        private void Content_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { _isDragging = false; _clickStart = e.GetPosition(this); e.Handled = false; }
        private void Content_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { _isDragging = false; }
        private void Content_MouseMove(object sender, MouseEventArgs e) { if (e.LeftButton==MouseButtonState.Pressed && !_viewModel.IsTaskbarLocked) { Point p=e.GetPosition(this); if (Math.Abs(p.X-_clickStart.X)>4||Math.Abs(p.Y-_clickStart.Y)>4) { _isDragging=true; DragMove(); } } }
        private void Resize_DragDelta(object sender, DragDeltaEventArgs e) { if (_viewModel.IsTaskbarLocked) return; if (sender is Thumb t) { if (t.Name=="LeftResize"){double w=Width-e.HorizontalChange;if(w>MinWidth){Width=w;Left+=e.HorizontalChange;}} else if(t.Name=="RightResize"){double w=Width+e.HorizontalChange;if(w>MinWidth)Width=w;} else if(t.Name=="TopResize"){double h=Height-e.VerticalChange;if(h>MinHeight){Height=h;Top+=e.VerticalChange;}} else if(t.Name=="BottomResize"){double h=Height+e.VerticalChange;if(h>MinHeight)Height=h;} } }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr h, int i);
        [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr h, int i, int v);
        [DllImport("user32.dll")] private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndAfter, int x, int y, int cx, int cy, uint flags);
        [DllImport("user32.dll")] private static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll")] private static extern IntPtr FindWindow(string cls, string? name);
        [DllImport("user32.dll")] private static extern IntPtr FindWindowEx(IntPtr parent, IntPtr child, string cls, string? name);
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr hwnd, out RECT r);
        [DllImport("user32.dll")] private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        [DllImport("user32.dll")] private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int left, top, right, bottom; }

        private RECT GetClockRect()
        {
            IntPtr tb = FindWindow("Shell_TrayWnd", null); if (tb == IntPtr.Zero) return new RECT();
            IntPtr tn = FindWindowEx(tb, IntPtr.Zero, "TrayNotifyWnd", null); if (tn == IntPtr.Zero) return new RECT();
            IntPtr cl = FindWindowEx(tn, IntPtr.Zero, "TrayClockWClass", null); if (cl == IntPtr.Zero) return new RECT();
            GetWindowRect(cl, out RECT r); return r;
        }
        private RECT GetTrayNotifyRect()
        {
            IntPtr tb = FindWindow("Shell_TrayWnd", null); if (tb == IntPtr.Zero) return new RECT();
            IntPtr tn = FindWindowEx(tb, IntPtr.Zero, "TrayNotifyWnd", null); if (tn == IntPtr.Zero) return new RECT();
            GetWindowRect(tn, out RECT r); return r;
        }
        private RECT GetTaskbarRect()
        {
            IntPtr tb = FindWindow("Shell_TrayWnd", null); if (tb == IntPtr.Zero) return new RECT();
            GetWindowRect(tb, out RECT r); return r;
        }
    }
}
