using SubmitLabServer.Models;


namespace SubmitLabServer.Services

{
    public interface ILibraryService

    {

        List<Book> GetBooks();
        List<User> GetUsers();
        Dictionary<User, List<Book>> GetBorrowedBooks();

        void ReadBooks();
        void ReadUsers();

        void AddBook(Book book);
        void EditBook(int id, Book updatedBook);
        void DeleteBook(int id);

        void AddUser(User user);
        void EditUser(int id, User updatedUser);
        void DeleteUser(int id);

        void BorrowBook(int bookId, int userId);
        void ReturnBook(int userId, int bookIndex);
    }
}
