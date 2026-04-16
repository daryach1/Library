using LibraryDesktop.Classes;
using LibraryDesktop.Windows;
using System;
using System.Collections.ObjectModel;
using System.IO;
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
            LoadDataFile();
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

        #region Работа с данными из файла
        private void UpdateDataButton_Click(object sender, RoutedEventArgs e)
        {
            books.Clear();
            LoadDataFile();
        }

        private void LoadDataFile()
        {
            int totalBook = 0;
            int totalAvailableBook = 0;

            try
            {
                if(!File.Exists(filePath))
                {
                    MessageBox.Show("Файл не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                books.Clear();

                using (StreamReader bookReader = new StreamReader(filePath))
                {
                    string lineRead;

                    while ((lineRead = bookReader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(lineRead))
                            continue;
                        string[] parts = lineRead.Split('|');
                        
                        if (parts.Length == 5)
                        {
                            Book bookData = new Book
                            {
                                Title = parts[0],
                                Author = parts[1],
                                Genre = parts[2],
                                TotalCopies = int.Parse(parts[3]),
                                AvailableCopies = int.Parse(parts[4])
                            };
                            books.Add(bookData);
                            totalBook += int.Parse(parts[3]);
                            totalAvailableBook += int.Parse(parts[4]);
                        }
                        else
                        {
                            MessageBox.Show($"Обнаружена строка с неккоретным форматом:\n{lineRead}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                TotalBookLabel.Content = totalBook.ToString();
                TotalAvailableBookLabel.Content = totalAvailableBook.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: \n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
