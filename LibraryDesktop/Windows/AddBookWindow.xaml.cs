using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using LibraryDesktop.Classes;

namespace LibraryDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
        }

        #region Сохранение данных в файл

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateAllFields())
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = System.IO.Path.Combine(baseDirectory, "book.txt");
                Book newBook = new Book
                {
                    Title = TitleTextBox.Text,
                    Author = AuthorTextBox.Text,
                    Genre = GenreTextBox.Text,
                    TotalCopies = int.Parse(TotalCopiesTextBox.Text),
                    AvailableCopies = int.Parse(AvailableCopiesTextBox.Text),
                };
                SaveDataFile(newBook, filePath);
                MessageBox.Show("Данные успешно сохранены!", "Успешное сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Данные не сохранились, проверьте правильность введения!", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveDataFile(Book book, string filePath)
        {
            using (StreamWriter writerBook = new StreamWriter(filePath, true))
            {
                writerBook.WriteLine($"{book.Title}|{book.Author}|{book.Genre}|{book.TotalCopies}|{book.AvailableCopies}");
            }
        }

        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region Проверка данных
        private bool ValidateAllFields()
        {
            var requiredFields = new[] { TitleTextBox, AuthorTextBox, GenreTextBox, TotalCopiesTextBox, AvailableCopiesTextBox};
            var numberFields = new[] {TotalCopiesTextBox, AvailableCopiesTextBox};
            bool isValid = true;

            foreach (var field in requiredFields)
            {
                if (string.IsNullOrEmpty(field.Text))
                {
                    MarkAsError(field, "Обязательно для заполнения");
                    isValid = false;
                }
                else
                {
                    ClearError(field);
                }
            }

            foreach (var field in numberFields)
            {
                if (!int.TryParse(field.Text, out _) || string.IsNullOrWhiteSpace(field.Text))
                {
                    MarkAsError(field, "Введите целое число");
                    isValid = false;
                }
                else
                {
                    ClearError(field);
                }

                if(int.Parse(field.Text)<0)
                {
                    MarkAsError(field, "Введите положительное число");
                    isValid=false;
                }
                else
                {
                    ClearError(field);
                }
            }
            if (int.Parse(AvailableCopiesTextBox.Text) > int.Parse(TotalCopiesTextBox.Text))
            {
                MarkAsError(AvailableCopiesTextBox, "Значение доступных книг не может быть больше общего количества");
                isValid = false;
            }
            return isValid;
        }

        private void MarkAsError (TextBox field, string message)
        {
            field.BorderBrush = Brushes.Red;
            field.ToolTip = message;
        }

        private void ClearError (TextBox field)
        {
            field.ToolTip = null;
            field.BorderBrush = Brushes.Black;
        }

        #endregion
    }
}
