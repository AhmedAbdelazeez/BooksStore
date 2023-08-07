using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models.Repositeries
{
    public class BookRepositery : IBooksStoreRepository<Book>
    {

        List<Book> books;

        public BookRepositery()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id = 1, Title = "C#", Description = "no des" ,
                    ImageUrl="a.png",
                    Author=new Author { Id=2}
                },
                new Book
                {
                    Id = 2, Title = "python", Description = "no des",
                    ImageUrl="b.png"
                    ,
                    Author = new Author()
                },
                new Book
                {
                    Id = 3, Title = "java", Description = "no des",
                    ImageUrl="c.png",
                    Author = new Author()
                }
            };
        }
        public void Add(Book entity)
        {
            entity.Id=books.Max(x => x.Id)+1;
            books.Add(entity);  
        }

        public void Delete(int id)
        {
            var book = Find(id);

            books.Remove(book); 
        }

        public Book Find(int id)
        {
            return books.SingleOrDefault(v => v.Id == id);
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(a => a.Title.Contains(term)).ToList();
            
        }

        public void Update(int id, Book NewBook)
        {
            var book = Find(id);

            book.Title = NewBook.Title;
            book.Description = NewBook.Description;
            book.Author = NewBook.Author;
            book.Id = id;
            book.ImageUrl = NewBook.ImageUrl;


        }
    }
}
