using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryDesktop.Pages;
using LibraryDesktop.Classes;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace LibraryDesktop.Tests
{
    [TestClass]
    public class ListBookPageDataTests
    {
        private string _testFilePath;
        private string _backupFilePath;

        [TestInitialize]
        public void Setup()
        {
            _testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            _backupFilePath = _testFilePath + ".bak";

            if (File.Exists(_testFilePath)) 
            {
                File.Move(_testFilePath, _backupFilePath);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath)) File.Delete(_testFilePath);
            if (File.Exists(_backupFilePath)) File.Move(_backupFilePath, _testFilePath);
        }

        [TestMethod]
        [STAThread]
        public void LoadDataFile_ValidFile_PopulatesBooksCollection()
        {
            File.WriteAllText(_testFilePath, "Книга 1|Автор 1|Фантастика|10|5\nКнига 2|Автор 2|Детектив|20|15");
            var page = new ListBooksPage();
            var privateObject = new PrivateObject(page);

            privateObject.Invoke("LoadDataFile");

            var books = (ObservableCollection<Book>)privateObject.GetField("books");
            Assert.IsNotNull(books);

            Assert.AreEqual(2, books.Count, "Должно быть загружено 2 книги");
            Assert.AreEqual("Книга 1", books[0].Title);

            Assert.AreEqual(5, books[0].AvailableCopies);
            Assert.AreEqual("Книга 2", books[1].Title);
        }

        [TestMethod]
        [STAThread]

        public void LoadDataFile_FileNotExists_ShowsErrorAndReturns()
        {
            if (File.Exists(_testFilePath)) File.Delete(_testFilePath);

            var page = new ListBooksPage();
            var privateObject = new PrivateObject(page);
            var books = (ObservableCollection<Book>)privateObject.GetField("books");
            books.Add(new Book { Title = "Test" });

            privateObject.Invoke("LoadDataFile");

            Assert.AreEqual(0, books.Count, "Коллекция должна быть пустой, если файл не найден");
        }
    }
}
