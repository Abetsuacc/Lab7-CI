using SubmitLabServer.Models;
using SubmitLabServer.Services;

namespace SubmitLabServer.Tests;

[TestClass]
public class LibraryServiceTests
{
    private LibraryService _service = null!;
    private string _testRoot = string.Empty;
    private string _originalCurrentDirectory = string.Empty;

    [TestInitialize]

    public void Setup()
    {
        _originalCurrentDirectory = Directory.GetCurrentDirectory();
        _testRoot = Path.Combine(Path.GetTempPath(), $"SubmitLabTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(_testRoot, "Data"));
        Directory.SetCurrentDirectory(_testRoot);
        _service = new LibraryService();
    }

    [TestCleanup]
    public void Cleanup()
    {
        Directory.SetCurrentDirectory(_originalCurrentDirectory);
        if (Directory.Exists(_testRoot))
        {
            Directory.Delete(_testRoot, recursive: true);
        }
    }

    [TestMethod]
    public void ReadBooks_ValidCsv()
    {
     
        File.WriteAllLines("./Data/Book.csv", new[]
        {
            "1,Dune,Frank Herbert,9780441172719"
        });

     
        _service.ReadBooks();

       
        Assert.AreEqual(1, _service.GetBooks().Count);
        Assert.AreEqual("Dune", _service.GetBooks()[0].Title);
    }

    [TestMethod]
    public void AddBook_ValidBook()
    {
        // Arrange
        var book = new Book { Title = "Clean Code", Author = "Robert Martin", ISBN = "123" };

        // Act
        _service.AddBook(book);

        // Assert
        Assert.AreEqual(1, _service.GetBooks().Count);
        Assert.AreEqual(1, _service.GetBooks()[0].Id);
    }

    [TestMethod]
    public void EditBook_ExistingId()
    {
        
        _service.AddBook(new Book { Title = "Old", Author = "A", ISBN = "111" });
        var id = _service.GetBooks()[0].Id;

        
        _service.EditBook(id, new Book { Title = "New", Author = "B", ISBN = "222" });

        
        Assert.AreEqual("New", _service.GetBooks()[0].Title);
        Assert.AreEqual("B", _service.GetBooks()[0].Author);
    }

    [TestMethod]
    public void DeleteBook_RemoveBook()
    {
       
        _service.AddBook(new Book { Title = "Delete Me", Author = "A", ISBN = "111" });
        var id = _service.GetBooks()[0].Id;

       
        _service.DeleteBook(id);

        
        Assert.AreEqual(0, _service.GetBooks().Count);
    }

    [TestMethod]
    public void BorrowBook_ValidIds()
    {
        
        _service.AddBook(new Book { Title = "Borrow Me", Author = "A", ISBN = "111" });
        _service.AddUser(new User { Name = "Tim", Email = "tim@example.com" });
        var bookId = _service.GetBooks()[0].Id;
        var user = _service.GetUsers()[0];

      
        _service.BorrowBook(bookId, user.Id);

       
        Assert.AreEqual(0, _service.GetBooks().Count);
        Assert.IsTrue(_service.GetBorrowedBooks().ContainsKey(user));
    }

    [TestMethod]
    public void ReturnBook_ValidBorrowReturns()
    {
        // arr
        _service.AddBook(new Book { Title = "Return Me", Author = "A", ISBN = "111" });
        _service.AddUser(new User { Name = "Tim", Email = "tim@example.com" });
        var user = _service.GetUsers()[0];
        _service.BorrowBook(_service.GetBooks()[0].Id, user.Id);

       // acting
        _service.ReturnBook(user.Id, 0);

        // Asser
        Assert.AreEqual(1, _service.GetBooks().Count);
        Assert.AreEqual(0, _service.GetBorrowedBooks()[user].Count);
    }

    [TestMethod]
    public void BorrowBook_InvalidBookIdDoesNothing()
    {
        
        _service.AddUser(new User { Name = "Tim", Email = "tim@example.com" });
        var userId = _service.GetUsers()[0].Id;

       
        _service.BorrowBook(999, userId);

       
        Assert.AreEqual(0, _service.GetBorrowedBooks().Count);
    }
}
