using LibraryDesktop.Classes;
using LibraryDesktop.Windows;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace LibraryDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListBooksPage.xaml
    /// </summary>
    public partial class ListBooksPage : Page
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string filePath = System.IO.Path.Combine(baseDirectory, "book.txt");
        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        public ListBooksPage()
        {
            InitializeComponent();
            BooksListView.ItemsSource = books;
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

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBook = new AddBookWindow();
            addBook.Show();
        }

        private void UpdateDataButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
