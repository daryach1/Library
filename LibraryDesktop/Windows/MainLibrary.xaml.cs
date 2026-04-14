using System.Windows;
using LibraryDesktop.Classes;
using LibraryDesktop.Pages;

namespace LibraryDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainLibrary : Window
    {
        public MainLibrary()
        {
            InitializeComponent();
            FrameManager.mainFrame = MainFrame;
            FrameManager.mainFrame.Navigate(new ListBooksPage());

        }
    }
}
