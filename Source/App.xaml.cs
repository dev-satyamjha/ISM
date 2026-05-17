using System;
using System.Drawing;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using NetMonitor.ViewModels;
using NetMonitor.Services;

namespace NetMonitor
{
    public partial class App : Application
    {
        private TaskbarIcon? _taskbarIcon;
        private MainViewModel? _viewModel;

        private void EnsureSingleInstance()
        {
            string currentProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            int currentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            var processes = System.Diagnostics.Process.GetProcessesByName(currentProcessName);
            foreach (var process in processes)
            {
                if (process.Id != currentProcessId)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(1000);
                    }
                    catch { }
                }
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            EnsureSingleInstance();

            DispatcherUnhandledException += (s, args) => 
            {
                System.IO.File.WriteAllText("crash.log", "Unhandled Exception: " + args.Exception.ToString());
                args.Handled = true;
            };

            try
            {
                _viewModel = new MainViewModel();
                
                _taskbarIcon = new TaskbarIcon();
                
                try
                {
                    _taskbarIcon.Icon = TrayIconBuilder.BuildIconWithText("0", _viewModel.Settings.TaskbarFontFamily, _viewModel.Settings.TaskbarFontColor);
                }
                catch (Exception ex)
                {
                    System.IO.File.WriteAllText("crash_tray.log", "Tray Icon Error: " + ex.ToString());
                }

                _taskbarIcon.ToolTipText = "ISM+: Initializing...";
                
                _taskbarIcon.ContextMenu = (System.Windows.Controls.ContextMenu)FindResource("SysTrayMenu");
                _taskbarIcon.DataContext = _viewModel;
                
                _taskbarIcon.TrayLeftMouseDown += (s, ev) => 
                {
                    if (MainWindow != null)
                    {
                        MainWindow.Show();
                        if (MainWindow.WindowState == WindowState.Minimized)
                        {
                            MainWindow.WindowState = WindowState.Normal;
                        }
                        MainWindow.Activate();
                    }
                };

                _viewModel.PropertyChanged += ViewModel_PropertyChanged;

                ApplyTheme(_viewModel.Settings.Theme);
                
                MainWindow = new MainWindow
                {
                    DataContext = _viewModel
                };
                
                MainWindow.Show();
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("crash_startup.log", "Startup Error: " + ex.ToString());
            }
        }


        public void ApplyTheme(NetMonitor.Models.ThemeMode theme)
        {
            if (theme == NetMonitor.Models.ThemeMode.Light)
            {
                Resources["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF"));
                Resources["TextBrush"] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#333333"));
                Resources["CardBrush"] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFF5E6"));
            }
            else
            {
                Resources["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1E1E1E"));
                Resources["TextBrush"] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFF"));
                Resources["CardBrush"] = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2D2D30"));
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_viewModel == null || _taskbarIcon == null) return;

            if (e.PropertyName == nameof(MainViewModel.TaskbarText))
            {
                Dispatcher.Invoke(() =>
                {
                    _taskbarIcon.Icon?.Dispose(); 
                    _taskbarIcon.Icon = TrayIconBuilder.BuildIconWithText(_viewModel.TaskbarText, _viewModel.Settings.TaskbarFontFamily, _viewModel.Settings.TaskbarFontColor);
                    
                    _taskbarIcon.ToolTipText = 
                        $"Speed: D: {_viewModel.CurrentDownloadSpeed} | U: {_viewModel.CurrentUploadSpeed}\\n" +
                        $"Usage: {_viewModel.DisplayedUsage}";
                });
            }
            else if (e.PropertyName == nameof(MainViewModel.SelectedTheme))
            {
                ApplyTheme(_viewModel.SelectedTheme);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _taskbarIcon?.Dispose();
        }
    }
}
