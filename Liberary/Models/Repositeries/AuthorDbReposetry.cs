using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositeries
{
    public class AuthorDbReposetry : IBooksStoreRepository<Author>
    {


        BookStoreDbContext db;
        public AuthorDbReposetry(BookStoreDbContext _db)
        {
            db = _db;

        }
        public void Add(Author entity)
        {
            db.Authors.Add(entity);
            db.SaveChanges();

        }

        public void Delete(int id)
        {
            var author = Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();

        }

        public Author Find(int id)
        {
            var author = db.Authors.SingleOrDefault(c => c.Id == id);

            return author;
        }


        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

       
        public void Update(int id, Author NewAuthor)
        {
            db.Update(NewAuthor);
            db.SaveChanges();
        }

       public List<Author>  Search(string term)
        {
            return db.Authors.Where(a => a.Name.Contains(term)).ToList();
        }
    }
}

    

