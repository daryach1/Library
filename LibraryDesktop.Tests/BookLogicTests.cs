using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryDesktop.Classes;
using System;
using System.IO;
using System.Collections.Generic;

namespace LibraryDesktopTests
{
    [TestClass]
    public class BookLogicTests
    {
        #region Тесты класса Book

        [TestMethod]
        public void Book_Properties_ShouldBeSetAndRetrievedCorrectly()
        {
            // Arrange
            var book = new Book
            {
                Title = "1984",
                Author = "Джордж Оруэлл",
                Genre = "Антиутопия",
                TotalCopies = 10,
                AvailableCopies = 5
            };

            // Act & Assert
            Assert.AreEqual("1984", book.Title);
            Assert.AreEqual("Джордж Оруэлл", book.Author);
            Assert.AreEqual("Антиутопия", book.Genre);
            Assert.AreEqual(10, book.TotalCopies);
            Assert.AreEqual(5, book.AvailableCopies);
        }

        #endregion

        #region Тесты логики валидации (аналог AddBookWindow.ValidateAllFields)

        // Выносим логику валидации в отдельный метод для чистого тестирования без зависимости от TextBox
        private bool ValidateBookData(string title, string author, string genre, string totalStr, string availableStr, out string errorMessage)
        {
            errorMessage = "";
            var requiredFields = new[] { title, author, genre };

            foreach (var field in requiredFields)
            {
                if (string.IsNullOrEmpty(field))
                {
                    errorMessage = "Обязательно для заполнения";
                    return false;
                }
            }

            if (!int.TryParse(totalStr, out int total) || string.IsNullOrWhiteSpace(totalStr))
            {
                errorMessage = "Введите целое число";
                return false;
            }

            if (!int.TryParse(availableStr, out int available) || string.IsNullOrWhiteSpace(availableStr))
            {
                errorMessage = "Введите целое число";
                return false;
            }

            if (total < 0)
            {
                errorMessage = "Введите положительное число";
                return false;
            }

            if (available < 0)
            {
                errorMessage = "Введите положительное число";
                return false;
            }

            if (available > total)
            {
                errorMessage = "Значение доступных книг не может быть больше общего количества";
                return false;
            }

            return true;
        }

        [DataTestMethod]
        [DataRow("Название", "Автор", "Жанр", "10", "5", true, "")] // Успешная валидация
        [DataRow("Название", "Автор", "Жанр", "10", "15", false, "Значение доступных книг не может быть больше общего количества")]
        [DataRow("Название", "Автор", "Жанр", "-5", "2", false, "Введите положительное число")]
        [DataRow("Название", "Автор", "Жанр", "10", "-2", false, "Введите положительное число")]
        [DataRow("", "Автор", "Жанр", "10", "5", false, "Обязательно для заполнения")]
        [DataRow("Название", "Автор", "Жанр", "abc", "5", false, "Введите целое число")]
        [DataRow("Название", "Автор", "Жанр", "10", "abc", false, "Введите целое число")]
        public void ValidateBookData_ShouldReturnExpectedResult(
            string title, string author, string genre, string totalStr, string availableStr, bool expectedIsValid, string expectedError)
        {
            // Act
            bool isValid = ValidateBookData(title, author, genre, totalStr, availableStr, out string errorMessage);

            // Assert
            Assert.AreEqual(expectedIsValid, isValid);
            Assert.AreEqual(expectedError, errorMessage);
        }

        #endregion

        #region Тесты работы с файлами (аналог ListBooksPage.LoadDataFile и AddBookWindow.SaveDataFile)

        [TestMethod]
        public void SaveAndLoadBook_ShouldPersistDataCorrectly()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "test_book_mstest.txt");
            if (File.Exists(tempFilePath)) File.Delete(tempFilePath);

            var bookToSave = new Book
            {
                Title = "Мастер и Маргарита",
                Author = "Михаил Булгаков",
                Genre = "Роман",
                TotalCopies = 20,
                AvailableCopies = 12
            };

            try
            {
                // Act 1: Сохранение (аналог SaveDataFile)
                using (StreamWriter writer = new StreamWriter(tempFilePath, true))
                {
                    writer.WriteLine($"{bookToSave.Title}|{bookToSave.Author}|{bookToSave.Genre}|{bookToSave.TotalCopies}|{bookToSave.AvailableCopies}");
                }

                // Act 2: Чтение (аналог LoadDataFile)
                var loadedBooks = new List<Book>();
                int totalBook = 0;
                int totalAvailableBook = 0;

                using (StreamReader reader = new StreamReader(tempFilePath))
                {
                    string lineRead;
                    while ((lineRead = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(lineRead)) continue;

                        string[] parts = lineRead.Split('|');
                        if (parts.Length == 5)
                        {
                            var book = new Book
                            {
                                Title = parts[0],
                                Author = parts[1],
                                Genre = parts[2],
                                TotalCopies = int.Parse(parts[3]),
                                AvailableCopies = int.Parse(parts[4])
                            };
                            loadedBooks.Add(book);
                            totalBook += book.TotalCopies;
                            totalAvailableBook += book.AvailableCopies;
                        }
                    }
                }

                // Assert
                Assert.AreEqual(1, loadedBooks.Count);
                Assert.AreEqual("Мастер и Маргарита", loadedBooks[0].Title);
                Assert.AreEqual(20, totalBook);
                Assert.AreEqual(12, totalAvailableBook);
            }
            finally
            {
                // Cleanup: удаление временного файла
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        [TestMethod]
        public void LoadDataFile_ShouldSkipInvalidLines()
        {
            // Arrange
            string tempFilePath = Path.Combine(Path.GetTempPath(), "test_invalid_book.txt");
            File.WriteAllLines(tempFilePath, new[]
            {
                "Valid|Author|Genre|10|5",
                "InvalidLineWithMissingParts|Author", // Некорректная строка (длина != 5)
                "Valid2|Author2|Genre2|20|10"
            });

            var loadedBooks = new List<Book>();

            try
            {
                // Act
                using (StreamReader reader = new StreamReader(tempFilePath))
                {
                    string lineRead;
                    while ((lineRead = reader.ReadLine()) != null)
                    {
                        string[] parts = lineRead.Split('|');
                        if (parts.Length == 5) // Логика из ListBooksPage.xaml.cs
                        {
                            loadedBooks.Add(new Book { Title = parts[0] });
                        }
                    }
                }

                // Assert: Должны загрузиться только 2 корректные книги, некорректная игнорируется
                Assert.AreEqual(2, loadedBooks.Count);
                Assert.AreEqual("Valid", loadedBooks[0].Title);
                Assert.AreEqual("Valid2", loadedBooks[1].Title);
            }
            finally
            {
                if (File.Exists(tempFilePath)) File.Delete(tempFilePath);
            }
        }

        #endregion
    }
}
