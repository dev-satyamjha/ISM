using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NetMonitor.ViewModels;
using NetMonitor.Models;

namespace NetMonitor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HideToTray_Click(object sender, RoutedEventArgs e) => this.Hide();

        private void OpenProStoreLink(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://apps.microsoft.com/detail/9ncl3jrpkhtw",
                    UseShellExecute = true
                });
            }
            catch { }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer? scrollViewer = sender as ScrollViewer;

            if (scrollViewer == null && sender is DependencyObject depObj)
                scrollViewer = FindVisualChild<ScrollViewer>(depObj);

            if (scrollViewer != null)
            {
                var vm   = this.DataContext as MainViewModel;
                double m = vm?.Settings?.ScrollSpeedMultiplier ?? 0.5;
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta * m));
                e.Handled = true;
            }
        }

        private static T? FindVisualChild<T>(DependencyObject? parent) where T : DependencyObject
        {
            if (parent == null) return null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t) return t;
                var found = FindVisualChild<T>(child);
                if (found != null) return found;
            }
            return null;
        }
    }
}
