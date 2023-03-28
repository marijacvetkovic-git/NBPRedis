using System.Text;
using System.Text.Unicode;
using ReaderCult.Models;
using StackExchange.Redis;

namespace ReaderCult.Services{
    public class RedisServices : IRedisServices
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisServices(IConnectionMultiplexer r){
            _redis=r;
        }
        public Book? AddBook(Book b)
        {
            var db = _redis.GetDatabase();
            var key= $"book:{b.ISBN}:id";
            var bookExists= db.HashGetAll(key);
            if(bookExists.Any())
            {
                return null;
            }
            HashEntry[] bookInfo= {
                new HashEntry("isbn",b.ISBN),
                new HashEntry("naslov", b.Naslov),
                new HashEntry("autor",b.Autor),
                new HashEntry("zanr",b.Zanr),
                new HashEntry("datumizdavanja", b.DatumIzdavanja.ToString()),
                new HashEntry("ukratkooknjizi",b.UkratkoOKnjizi),
                new HashEntry("prosecanrate",0)
            };

            db.HashSet(key, bookInfo);
            db.SetAdd("NotDoneBooks:",b.ISBN);
            return b;
        }
        public string AddBookInList(string username, string isbn)
        {
            var db = _redis.GetDatabase();
            var userkey= $"user:{username}:id";
            var bookkey=$"book:{isbn}:id";
            var usernameExists= db.HashGetAll(userkey);
            var bookExists= db.HashGetAll(bookkey);
            if (!usernameExists.Any())
                return "User not found";

            if (!bookExists.Any())
                return "Book not found";

            string[] knjige = db.SetMembers("Read:"+username).ToStringArray();

            if(!knjige.Contains(isbn))
            {
                db.SetAdd("Read:"+username,isbn);
                return "Book added to your list";
            }
            else
                return "Book already exitsts in this list";
        }

        public string AddRateBook(string username, string isbn, string rate)
        {
            var db = _redis.GetDatabase();
          
            var userkey= $"user:{username}:id";
            var bookkey=$"book:{isbn}:id";
            var usernameExists= db.HashGetAll(userkey);
            var bookExists= db.HashGetAll(bookkey);
            if (!usernameExists.Any())
                return "User not found";

            if (!bookExists.Any())
                return "Book not found";


            string[] rates = db.SetMembers("RatedBook:"+isbn).ToStringArray();

            if(!rates.Contains(username))
            {
                db.SetAdd("RatedBook:"+isbn,username+" "+rate);

                int rates1 = db.SetMembers("RatedBook:"+isbn).Count();
                string[] ratesupdated = db.SetMembers("RatedBook:"+isbn).ToStringArray();

                float avrgrate=0;
                foreach(string ratee in ratesupdated)
                {
                    string[] k = ratee.Split(" ");
                    int r= int.Parse(k[1]);
                    avrgrate+=r;
                }
                avrgrate=avrgrate/rates1;
                db.HashSet(bookkey,"prosecanrate",avrgrate);
                return "Rate added to list";
            }
            else
                return "You already rated this book";
        }
        public string DeleteBook(string isbn)
        {
            var db = _redis.GetDatabase();
            var bookkey=$"book:{isbn}:id";
            var bookExists= db.HashGetAll(bookkey);
            if(!bookExists.Any())
            {
                return null; 
            }      
            db.KeyDelete(bookkey);
            db.KeyDelete("RatedBook:"+isbn);
            if(db.SetContains("DoneBooks:",isbn)){
                db.SetRemove("DoneBooks:",isbn);
            }
            if(db.SetContains("NotDoneBooks:",isbn)){
                db.SetRemove("NotDoneBooks:",isbn);
            }
            if(db.SetContains("ToReadThisWeek:",isbn)){
                db.SetRemove("ToReadThisWeek:",isbn);
            }
            var pattern = $"Read:*";        
            var keys = _redis.GetServer("redis-19246.c8.us-east-1-2.ec2.cloud.redislabs.com", 19246).Keys(pattern:pattern).ToArray();
            foreach(var key in keys)
            {
                if(db.SetContains(key,isbn))
                {
                    db.SetRemove(key,isbn);
                }
            }
            return "Deleted!";    
        }
   
        public string DeleteProfile(string username, string password)
        {
            var db = _redis.GetDatabase();
            var userkey= $"user:{username}:id";
            var usernameExists= db.HashGetAll(userkey);
             if (!usernameExists.Any())
                return "User not found";

            
            if(db.HashGet(userkey, "password")==password)
            {
                db.KeyDelete(userkey);
                var readset=$"Read:{username}";
                db.KeyDelete(readset);
                var pattern =$"RatedBook:*";
                var keys = _redis.GetServer("redis-19246.c8.us-east-1-2.ec2.cloud.redislabs.com", 19246).Keys(pattern:pattern).ToArray();
                foreach(var key in keys)
                {
                    string[] rates = db.SetMembers(key).ToStringArray(); 
                    foreach(var rate in rates)
                    {
                        var user=rate.Split(" ");
                        if(user[0] == username)
                        {
                            db.SetRemove(key,rate);
                        }
                    }
                }

                return "Deleted";

            }
            return "";
        }
      
        public string [] GetAllBookReadByUser(string username)
        {
            var db = _redis.GetDatabase();
            string[] knjige = db.SetMembers("Read:"+username).ToStringArray();
            return knjige; 
        }
       
        public User? GetUser(string username)
        {
            var db = _redis.GetDatabase();
            var userkey= $"user:{username}:id";     
            var usernameExists= db.HashGetAll(userkey);
           
            if (!usernameExists.Any())
                return null;

            var user = new User();
            foreach(var vrsta in usernameExists )
            {
                if(vrsta.Name=="username")
                {
                    user.Username=vrsta.Value;
                }
                if(vrsta.Name=="password")
                {
                    user.Password=vrsta.Value;
                }
                if(vrsta.Name=="name")
                {
                    user.Name=vrsta.Value;
                }
                if(vrsta.Name=="surname")
                {
                    user.Surname=vrsta.Value;
                }
                 if(vrsta.Name=="role")
                {
                    user.Role= vrsta.Value;
                }
                
            }
            return user;
        
            
        }
      
        public Book? GetBook(string isbn)
        {
            var db = _redis.GetDatabase();
            var bookkey=$"book:{isbn}:id";
            var bookExists= db.HashGetAll(bookkey);
            if(!bookExists.Any())
            {
                return null; 
            }      
            var book = new Book();
            foreach(var vrsta in bookExists )
            {
                if(vrsta.Name=="isbn")
                {
                    book.ISBN=vrsta.Value;
                }
                if(vrsta.Name=="naslov")
                {
                    book.Naslov=vrsta.Value;
                }
                   if(vrsta.Name=="autor")
                {
                    book.Autor=vrsta.Value;
                }
                   if(vrsta.Name=="zanr")
                {
                    book.Zanr=vrsta.Value;
                }
                 if(vrsta.Name=="datumizdavanja")
                {
                    book.DatumIzdavanja=DateTime.Parse(vrsta.Value);
                }
                 if(vrsta.Name=="ukratkooknjizi")
                {
                    book.UkratkoOKnjizi=vrsta.Value;
                }
                 if(vrsta.Name=="prosecanrate")
                {
                    book.ProsecanRate=float.Parse(vrsta.Value);
                }
            }
            return book;
        }
       
        public User? LogIn(string username, string password)
        {
            var db = _redis.GetDatabase();
             var userkey= $"user:{username}:id";
            var usernameExists= db.HashGetAll(userkey);
            if(!usernameExists.Any())
            {
                return null;
            }
            var pas = usernameExists.Where(p=>p.Name=="password").First().Value;
            if(pas!=password)
            {
                return null;
            }
            var user = new User();
            foreach(var vrsta in usernameExists )
            {
                if(vrsta.Name=="username")
                {
                    user.Username=vrsta.Value;
                }
                if(vrsta.Name=="password")
                {
                    user.Password=vrsta.Value;
                }
                   if(vrsta.Name=="name")
                {
                    user.Name=vrsta.Value;
                }
                   if(vrsta.Name=="surname")
                {
                    user.Surname=vrsta.Value;
                }
                if(vrsta.Name=="role")
                {
                    user.Role=vrsta.Value;
                }
            }
            return user; 
        }
        
        public User? SignUp(User u)
        {
            var db= _redis.GetDatabase();
            var key = $"user:{u.Username}:id";
            var usernameExists= db.HashGetAll(key);
            if(usernameExists.Any())
            {
                return null;
            }
      
            HashEntry[] userInfo= {
                new HashEntry("username",u.Username),
                new HashEntry("password", u.Password),
                new HashEntry("name",u.Name),
                new HashEntry("surname",u.Surname),
                new HashEntry("role",u.Role)
            };
            db.HashSet(key, userInfo);
            return u;
        }
      
        public string GetMyRateOfBook(string username, string isbn){
            var db = _redis.GetDatabase();
            string[] rate = db.SetMembers("RatedBook:"+isbn).ToStringArray();
            foreach(var r in rate){
                string[] jedan= r.Split(' ');
                string un= jedan[0];
                string r1= jedan[1];
                if(username==un){
                    return r1;
                }
            }
            return null;
         }

        public string AddToBooksThatWereRead(string isbn)
        {
            var db= _redis.GetDatabase();
            var bookExists= db.SetContains("DoneBooks:", isbn);
            if (bookExists)
                return null;
            db.SetAdd("DoneBooks:", isbn);
            db.SetRemove("NotDoneBooks:",isbn);
            
            return "Book added!";
        }

        public List<Book> GetNotDoneBooks()
        {
            var db= _redis.GetDatabase();
            string[] booksIsbn = db.SetMembers("NotDoneBooks:").ToStringArray();
            List<Book> books= new List<Book>();
            foreach(var book in booksIsbn){
                Book b= this.GetBook(book);
                books.Add(b);
            }
            return books;
        }

        public List<Book> GetDoneBooks()
        {
            var db= _redis.GetDatabase();
            string[] booksIsbn = db.SetMembers("DoneBooks:").ToStringArray();
            List<Book> books= new List<Book>();
            foreach(var book in booksIsbn){
                Book b= this.GetBook(book);
                books.Add(b);
            }
            return books;
        }

        public string SetBookToRead(string isbn)
        {
            var db= _redis.GetDatabase();
            if (db.SetLength("ToReadThisWeek:")!=0){
                var isbnLastBook= db.SetPop("ToReadThisWeek:");
                db.SetAdd("DoneBooks:", isbnLastBook);
                 db.SetRemove("NotDoneBooks:",isbnLastBook);
            }
           
            db.SetAdd("ToReadThisWeek:", isbn);
            return "Added!";
        }

        public Book GetToReadBook()
        {
            var db= _redis.GetDatabase();
            var book= db.SetMembers("ToReadThisWeek:");
            if(!book.Any()) return null;
            Book b= this.GetBook(book[0].ToString());
            return b;
        }

       
    }
}