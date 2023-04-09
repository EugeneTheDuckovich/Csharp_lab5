using dotnet_lab5_src.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace dotnet_lab5_src
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new LibraryViewModel();            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox is null) return;

            textBox.ScrollToEnd();
        }
    }
}
