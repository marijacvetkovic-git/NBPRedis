using ReaderCult.Models;
using StackExchange.Redis;

namespace ReaderCult.Services{
    public interface IRedisServices{
        public User? SignUp(User u);
        public User? LogIn(string username, string password);
        public string DeleteProfile(string username, string password);
        public string AddBookInList(string isbn, string username); 
        public string [] GetAllBookReadByUser(string idUser); 
        public User? GetUser(string username); 
        public Book? AddBook(Book b);
        public string DeleteBook(string idBook);  
        public string AddRateBook(string idKorisnika,string idBook,string rate); 
        public string GetMyRateOfBook(string idUsera, string idBook); 
        public Book? GetBook(string isbn); 
        public string AddToBooksThatWereRead(string isbn);
        public List<Book> GetNotDoneBooks();
        public List<Book> GetDoneBooks();
        public string SetBookToRead(string isbn);
        public Book GetToReadBook();
        


    }
}