using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryDesktop.Classes;

namespace LibraryDesktop.Tests
{
    [TestClass]
    public class BookTests
    {
        [TestMethod]
        public void Book_Properties_ShouldBeSetAndRerievedCorrectly()
        {
            var book = new Book
            {
                Title = "Война и мир",
                Author = "Лев Толстой",
                Genre = "Роман",
                TotalCopies = 10,
                AvailableCopies = 7
            };

            Assert.AreEqual("Война и мир", book.Title);
            Assert.AreEqual("Лев Толстой", book.Author);
            Assert.AreEqual("Роман", book.Genre);
            Assert.AreEqual(10, book.TotalCopies);
            Assert.AreEqual(7, book.AvailableCopies);
        }

        [TestMethod]
        public void Book_DefaultValues_ShouldBeValid()
        {
            var book = new Book();

            Assert.IsNull(book.Title);
            Assert.AreEqual(0, book.TotalCopies);
            Assert.AreEqual(0, book.AvailableCopies);
        }


    }
}

