using System.Windows;

namespace MKTL.WPF.Views.Components
{
    public partial class CustomMessageBox : Window
    {
        public string Message { get; set; }

        public CustomMessageBox(string title, string message, bool showCancel = false)
        {
            InitializeComponent();
            Title = title;
            Message = message;
            DataContext = this;
            if (!showCancel) BtnCancel.Visibility = Visibility.Collapsed;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) { DialogResult = true; Close(); }
        private void BtnCancel_Click(object sender, RoutedEventArgs e) { DialogResult = false; Close(); }

        public static bool Show(string title, string message, bool showCancel = false)
        {
            var msg = new CustomMessageBox(title, message, showCancel);
            return msg.ShowDialog() ?? false;
        }
    }
}