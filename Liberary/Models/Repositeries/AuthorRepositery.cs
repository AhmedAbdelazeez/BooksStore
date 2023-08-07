using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositeries
{
    public class AuthorRepositery : IBooksStoreRepository<Author>
    {
        IList<Author> authors;

        public AuthorRepositery()
        {
            authors = new List<Author>()
            {

            new Author { Id = 1, Name = "Ahmed" },
            new Author { Id = 2, Name = "Zezo" },
            new Author { Id = 3, Name = "Aziz" },

        };
    }
        public void Add(Author entity)
        {
            entity.Id=authors.Max(x => x.Id)+1;
            authors.Add(entity);    
        }

        public void Delete(int id)
        {
            var author = Find(id);
            authors.Remove(author);
         }

        public Author Find(int id)
        {
            var author = authors.SingleOrDefault(c => c.Id == id);

            return author;
        }
           

        public IList<Author> List()
        {
            return authors;
        }

        public List<Author> Search(string term)
        {
            return authors.Where(a=>a.Name.Contains(term)).ToList();
        }

        public void Update(int id, Author NewAuthor)
        {
            var author = Find(id);
            author.Name = NewAuthor.Name; 
        }
    }
}
