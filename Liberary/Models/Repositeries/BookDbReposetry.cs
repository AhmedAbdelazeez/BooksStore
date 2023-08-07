using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositeries
{
    public class BookDbReposetry : IBooksStoreRepository<Book>
    {

        
        BookStoreDbContext db;
        public BookDbReposetry(BookStoreDbContext _db )
        {
           db= _db;
        }
        public void Add(Book entity)
        {
            
            db.Books.Add(entity);
            db.SaveChanges();

        }

        public void Delete(int id)
        {
            var book = Find(id);

            db.Books.Remove(book);
            db.SaveChanges();

        }

        public Book Find(int id)
        {
            return db.Books.Include(a => a.Author).SingleOrDefault(v => v.Id == id);
        }

        public IList<Book> List()
        {
            //include مهمه
            return db.Books.Include(a=>a.Author).ToList();
        }



        public void Update(int id, Book NewBook)
        {

            db.Update(NewBook);

            db.SaveChanges();


        }

        public List<Book> Search(string term)
        {
            var resualt = db.Books.Include(a => a.Author)
             .Where(b => b.Title.Contains(term)

                    || b.Description.Contains(term) 
                    || b.Author.Name.Contains(term)).ToList();

            return resualt;  
        }
    }
}

