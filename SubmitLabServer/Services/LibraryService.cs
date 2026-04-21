using static System.Reflection.Metadata.BlobBuilder;
using SubmitLabServer.Models;

namespace SubmitLabServer.Services
{
    public class LibraryService : ILibraryService
    {
        
        private List<Book> books = new List<Book>();
        private List<User> users = new List<User>();
        private Dictionary<User, List<Book>> borrowedBooks = new Dictionary<User, List<Book>>();


        public void ReadBooks()
        {
            books.Clear();
            try
            {
                foreach (var line in File.ReadLines("./Data/Books.csv"))
                {
                    var fields = line.Split(',');
                    if (fields.Length >= 4 && int.TryParse(fields[0].Trim(), out var id))
                    {
                        var book = new Book
                        {
                            Id = id,
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim()
                        };
                        books.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading books: {ex.Message}");
            }
        }

        public void ReadUsers()
        {
            users.Clear();
            try
            {
                foreach (var line in File.ReadLines("./Data/Users.csv"))
                {
                    var fields = line.Split(',');
                    if (fields.Length >= 3 && int.TryParse(fields[0].Trim(), out var id))
                    {
                        var user = new User
                        {
                            Id = id,
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        };
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading users: {ex.Message}");
            }
        }

        

        public List<Book> GetBooks() => books;
        public List<User> GetUsers() => users;
        public Dictionary<User, List<Book>> GetBorrowedBooks() => borrowedBooks;

       

        private void WriteBooks()
        {
            var lines = books.Select(b => $"{b.Id},{b.Title},{b.Author},{b.ISBN}");
            File.WriteAllLines("./Data/Books.csv", lines);
        }

        private void WriteUsers()
        {
            var lines = users.Select(u => $"{u.Id},{u.Name},{u.Email}");
            File.WriteAllLines("./Data/Users.csv", lines);
        }

       

        public void AddBook(Book book)
        {
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            WriteBooks();
        }

        public void EditBook(int id, Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                if (!string.IsNullOrEmpty(updatedBook.Title)) book.Title = updatedBook.Title;
                if (!string.IsNullOrEmpty(updatedBook.Author)) book.Author = updatedBook.Author;
                if (!string.IsNullOrEmpty(updatedBook.ISBN)) book.ISBN = updatedBook.ISBN;
                WriteBooks();
            }
        }

        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                WriteBooks();
            }
        }

        //exost

        public void AddUser(User user)
        {
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            WriteUsers();
        }

        public void EditUser(int id, User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(updatedUser.Name)) user.Name = updatedUser.Name;
                if (!string.IsNullOrEmpty(updatedUser.Email)) user.Email = updatedUser.Email;
                WriteUsers();
            }
        }
        // user can not be less than id 0 
        public void DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
                WriteUsers();
            }
        }


        public void BorrowBook(int bookId, int userId)
        {
            var book = books.FirstOrDefault(b => b.Id == bookId);
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (book != null && user != null)
            {
                if (!borrowedBooks.ContainsKey(user))
                    borrowedBooks[user] = new List<Book>();
                borrowedBooks[user].Add(book);
                books.Remove(book);
                WriteBooks();
            }
        }
        // if books exists
        public void ReturnBook(int userId, int bookIndex)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null && borrowedBooks.ContainsKey(user) && bookIndex < borrowedBooks[user].Count)
            {
                var bookToReturn = borrowedBooks[user][bookIndex];
                borrowedBooks[user].RemoveAt(bookIndex);
                books.Add(bookToReturn);
                WriteBooks();
            }
        }
    }
}

