using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListBooksPage.xaml
    /// </summary>
    public partial class ListBooksPage : Page
    {
        public ListBooksPage()
        {
            InitializeComponent();
        }

        private void BooksListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0)
            {
                double totalWidth = e.NewSize.Width;

                TitleColumn.Width = totalWidth * 0.3;
                GenreColumn.Width = totalWidth * 0.2;
                AuthorColumn.Width = totalWidth * 0.2;
                TotalCopiesColumn.Width = totalWidth * 0.15;
                AvailableCopiesColumn.Width = totalWidth * 0.15;
                
            }
        }
    }
}
