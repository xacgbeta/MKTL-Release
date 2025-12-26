using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MKTL.WPF.Views.Components
{
    public partial class TitleBar : System.Windows.Controls.UserControl

    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(TitleBar));
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

        public static readonly DependencyProperty LeftContentProperty = DependencyProperty.Register("LeftContent", typeof(object), typeof(TitleBar));
        public object LeftContent { get => GetValue(LeftContentProperty); set => SetValue(LeftContentProperty, value); }

        private void OnMouseDown(object sender, MouseButtonEventArgs e) { if(e.ChangedButton == MouseButton.Left) Window.GetWindow(this)?.DragMove(); }
        private void Minimize_Click(object sender, RoutedEventArgs e) { if (Window.GetWindow(this) is Window w) w.WindowState = WindowState.Minimized; }
        private void Close_Click(object sender, RoutedEventArgs e) { Window.GetWindow(this)?.Close(); }
    }
}