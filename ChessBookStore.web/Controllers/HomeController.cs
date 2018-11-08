using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChessBookStore.web.Models;
using ChessBookStore.web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ChessBookStore.web.Controllers
{
    public class HomeController : Controller
    {
        BookStoreDbContext _context;
        public HomeController(BookStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId = null, int? authorId = null)
        {
            ViewBag.Categories = _context.Categories.ToArray();
            ViewBag.Authors = _context.Authors.Where(p => p.Name != "невідомо").OrderBy(p => p.Name).Take(5).ToArray();
            string title = "";
            List<Book> books;
            if (categoryId == null)
            {
                if (authorId != null)
                {
                    books = _context.Books.Include(p => p.Author).Where(p => p.AuthorId == authorId).ToList();
                    title = _context.Authors.FirstOrDefault(p => p.Id == authorId).Name;
                }
                else
                {
                    title = "Усі книжки";
                    books = _context.Books.Include(p => p.Author).ToList();
                }
            }
            else
            {
                books = _context.Books.Include(p => p.Author).Where(p => p.CategoryId == categoryId).ToList();
                title = _context.Categories.FirstOrDefault(p => p.Id == categoryId).Name;
            }
            ViewBag.Title = title;
            return View(books);
        }


        public IActionResult GetAuthors()
        {
            var authors = _context.Authors.Where(p => p.Name != "невідомо").OrderBy(p => p.Name).ToArray();
            return Json(authors);
        }

        public IActionResult Book(int id)
        {
            Book book = _context.Books.Include(p => p.Author).FirstOrDefault(p => p.Id == id);
            book.Author.Books = _context.Books.Where(p => p.AuthorId == book.AuthorId).Where(p => p.Id != id).ToList();
            if (book == null)
                return NotFound();
            return View(book);
        }
    }
}
