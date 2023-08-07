using BookStore.Models;
using BookStore.Models.Repositeries;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBooksStoreRepository<Book> booksRepository;
        private readonly IBooksStoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;
        public BookController
            (
            
            IBooksStoreRepository<Book> booksRepository 
        
            ,IBooksStoreRepository<Author> authorRepository,
            IHostingEnvironment hosting
            )
        {
            this.booksRepository = booksRepository;
            this.authorRepository = authorRepository;
            this.hosting= hosting;  
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books= booksRepository.List();  
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = booksRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };

            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string filleName = UploadFile(model.File)?? string.Empty;
                   

                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "please select an Author from the list !";
                        
                        return View(GetAllAuthors());
                    }

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Descreption,
                        Author = author,
                        ImageUrl=filleName
                    };

                    booksRepository.Add(book);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }

            }
           
            ModelState.AddModelError("", "you have to fill all the requerd fildes");
            return View(GetAllAuthors());
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = booksRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var viewmodel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Descreption=book.Description,
                AuthorId= authorId,
                Authors=authorRepository.List().ToList(),
                ImageUrl=book.ImageUrl,
            };
            return View(viewmodel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel viewModel)
        {
            try
            {
                string filleName = UploadFile(viewModel.File,viewModel.ImageUrl);
                
                var author = authorRepository.Find(viewModel.AuthorId);
                Book book = new Book
                {
                     Id=viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Descreption,
                    Author = author,
                    ImageUrl=filleName
                };

                booksRepository.Update(viewModel.BookId,book);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = booksRepository.Find(id);


            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                booksRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      List<Author>  FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author {Id=-1,Name ="-------Please Select an Author -------" });

            return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var vsmodel = new BookAuthorViewModel
            {

                Authors = FillSelectList()

            };
            return vsmodel;
        }

        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
               
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullpath = Path.Combine(uploads, file.FileName);

                file.CopyTo(new FileStream(fullpath, FileMode.Create));
                return file.FileName;


            }

            return null;

           
        }

        string UploadFile(IFormFile file,string ImageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
              
                string Newpath = Path.Combine(uploads, file.FileName);
              
                string oldpath = Path.Combine(uploads, ImageUrl);

                if (oldpath != Newpath)
                {
                    System.IO.File.Delete(oldpath);

                    //save new file
                    file.CopyTo(new FileStream(Newpath, FileMode.Create));
                }
                return file.FileName;   
            }

            return ImageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = booksRepository.Search(term);

            return View("Index", result);

        }

    }
}
