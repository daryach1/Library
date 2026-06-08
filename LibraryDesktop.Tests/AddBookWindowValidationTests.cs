using LibraryDesktop.Classes;
using LibraryDesktop.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Windows.Controls;

namespace LibraryDesktop.Tests
{
    [TestClass]
    public class AddBookWindowValidationTests
    {
        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(obj, value);
            }
            else
            {
                Assert.Fail($"Поле {fieldName} не найдено в классе {obj.GetType().Name}");
            }
        }

        private object InvokePrivateMethod(object obj, string methodName)
        {
            var method = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method?.Invoke(obj, null);
        }

        [TestMethod]
        [STAThread]
        public void ValidateAllFields_WithValidData_ReturnsTrue()
        {
            var window = new AddBookWindow();
            SetPrivateField(window, "TitleTextBox", new TextBox { Text = "Название книги" });
            SetPrivateField(window, "AuthorTextBox", new TextBox { Text = "Автор" });
            SetPrivateField(window, "GenreTextBox", new TextBox { Text = "Жанр" });
            SetPrivateField(window, "TotalCopiesTextBox", new TextBox { Text = "10" });
            SetPrivateField(window, "AvailableCopiesTextBox", new TextBox { Text = "5" });

            bool isValid = (bool)InvokePrivateMethod(window, "ValidateAllFields");

            Assert.IsTrue(isValid, "Валидация должна пройти успешно при корректных данных");
        }

        [TestMethod]
        [STAThread]
        public void ValidateAllFields_WithAvailableGreaterThanTotal_ReturnsFalse()
        {
            var window = new AddBookWindow();
            SetPrivateField(window, "TitleTextBox", new TextBox { Text = "Название" });
            SetPrivateField(window, "AuthorTextBox", new TextBox { Text = "Автор" });
            SetPrivateField(window, "GenreTextBox", new TextBox { Text = "Жанр" });
            SetPrivateField(window, "TotalCopiesTextBox", new TextBox { Text = "5" });
            SetPrivateField(window, "AvailableCopiesTextBox", new TextBox { Text = "10" });

            bool isValid = (bool)InvokePrivateMethod(window, "ValidateAllFields");

            Assert.IsFalse(isValid, "Валидация должна вернуть false, если доступных книг больше, чем общих");
        }

        [TestMethod]
        [STAThread]
        public void ValidateAllFields_WithNegativeNumbers_ReturnsFalse()
        {
            var window = new AddBookWindow();
            SetPrivateField(window, "TitleTextBox", new TextBox { Text = "Название" });
            SetPrivateField(window, "AuthorTextBox", new TextBox { Text = "Автор" });
            SetPrivateField(window, "GenreTextBox", new TextBox { Text = "Жанр" });
            SetPrivateField(window, "TotalCopiesTextBox", new TextBox { Text = "-5" }); // Отрицательное число
            SetPrivateField(window, "AvailableCopiesTextBox", new TextBox { Text = "2" });

            bool isValid = (bool)InvokePrivateMethod(window, "ValidateAllFields");

            Assert.IsFalse(isValid, "Валидация должна вернуть false при отрицательных значениях");
        }

    }
}
