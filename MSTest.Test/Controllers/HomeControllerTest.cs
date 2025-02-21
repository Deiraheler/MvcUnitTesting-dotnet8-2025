using Telerik.JustMock;
using MvcUnitTesting_dotnet8.Models;
using MvcUnitTesting_dotnet8.Controllers;
using Microsoft.AspNetCore.Mvc;
using Telerik.JustMock.Helpers;
using System.Linq.Expressions;


namespace MvcUnitTesting.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_Returns_All_books_In_DB()
        {
            //Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            Mock.Arrange( () => bookRepository.GetAll()).
                Returns(new List<Book>()
                {
                    new Book { Genre="Fiction", ID=1, Name="Moby Dick", Price=12.50m},
                    new Book { Genre="Fiction", ID=2, Name="War and Peace", Price=17m},
                    new Book { Genre="Science Fiction", ID=1, Name="Escape from the vortex", Price=12.50m},
                    new Book { Genre="History", ID=2, Name="The Battle of the Somme", Price=22m},
                }).MustBeCalled();

            //Act
            HomeController controller = new HomeController(bookRepository,null);
            ViewResult viewResult = controller.Index("Fiction") as ViewResult;
            var model = viewResult.Model as IEnumerable<Book>;

            //Assert
            Assert.AreEqual(4, model.Count());

        }

       
        [TestMethod]
        public void Privacy()
        {
            // Arrange
            var bookRepository = Mock.Create<IRepository<Book>>();
            HomeController controller = new HomeController(bookRepository,null);

            // Act
            ViewResult result = controller.Privacy() as ViewResult;

            // Assert
            Assert.AreEqual("Your Privacy is our concern", result.ViewData["Message"]);
        }

        [TestMethod]
        public void show_ViewData_genre_test()
        {
            // Arrange
            var mockRepo = Mock.Create<IRepository<Book>>();
            HomeController controller = new HomeController(mockRepo, null);

            // Act
            ViewResult result = controller.Index("Fiction") as ViewResult;

            // Assert
            Assert.AreEqual("Fiction", result.ViewData["Genre"], "Genre should match 'Fiction'.");
        }

        [TestMethod]
        public void test_book_by_genre()
        {
            // Arrange
            var mockRepo = Telerik.JustMock.Mock.Create<IRepository<Book>>();
            var allBooks = new List<Book>
            {
                new Book { ID = 1, Name = "Fiction1", Genre = "Fiction" },
                new Book { ID = 2, Name = "Fiction2", Genre = "Fiction" },
                new Book { ID = 3, Name = "NonFiction1", Genre = "Non-Fiction" },
            };

            Telerik.JustMock.Mock
        .Arrange(() => mockRepo.Find(Telerik.JustMock.Arg.IsAny<Expression<Func<Book, bool>>>()))
        .Returns((Expression<Func<Book, bool>> predicate) =>
            allBooks.Where(predicate.Compile()));

            var controller = new HomeController(mockRepo, null);

            // Act
            // Calls controller.Index("Fiction"), which does _repository.Find(b => b.Genre == "Fiction")
            var result = controller.Index("Fiction") as ViewResult;
            var model = result?.Model as IEnumerable<Book>;

            // Assert
            Assert.IsNotNull(model, "Model should not be null.");
            Assert.AreEqual(2, model.Count(), "Should return exactly 2 Fiction books.");
        }


    }
}
