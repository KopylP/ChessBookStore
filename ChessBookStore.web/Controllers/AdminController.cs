using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChessBookStore.web.Data;
using ChessBookStore.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ChessBookStore.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        BookStoreDbContext _context;
        IHostingEnvironment _appEnvironment;
        UserManager<User> _manager;
        RoleManager<IdentityRole> _roleManager;
        public AdminController(BookStoreDbContext context, IHostingEnvironment environment, UserManager<User> manager, RoleManager<IdentityRole> roleManager) {
            _context = context; _appEnvironment = environment;
            _manager = manager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult AddBook(string author = null)
        {
            AddBookModelView model = new AddBookModelView();
            model.AuthorName = author;
            model.Authors = _context.Authors.ToList();
            model.Categories = _context.Categories.ToList();
            model.Disconts = _context.Disconts.ToList();
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddBook(AddBookModelView model)
        {
            if (ModelState.IsValid)
            {
                string path = "wwwroot/images/books/";//@"\/\:<>?'"
                var nameOfBook = model.NameEn.Replace(" ", "").Replace(@"\", "").Replace(@"/", "").Replace(@"|", "").Replace(@":", "").Replace(@"'", "").Replace(@"?", "").Replace("<", "").Replace(">", "") + ".jpg";
                using (FileStream stream = new FileStream(path + nameOfBook, FileMode.Create, FileAccess.Write))
                {
                    model.Image.CopyTo(stream);
                }
                using (FileStream stream = new FileStream(path + nameOfBook, FileMode.Open, FileAccess.ReadWrite))
                {
                    MagickImage image = new MagickImage(stream);
                    image.Resize(200, 0);
                    //_appEnvironment.ContentRootPath + "\\wwwroot\\images\\books\\" + "min_" + model.NameEn + ".jpg"
                    using (FileStream streamTwo = new FileStream(path + "min_" + nameOfBook, FileMode.Create, FileAccess.Write))
                    {
                        image.Write(streamTwo);
                    }
                }
                Author author = _context.Authors.FirstOrDefault(p => p.Name.Equals(model.AuthorName));
                if (author == null)
                {
                    ModelState.AddModelError("Author", "Автора надано невірно!");
                }
                else
                {
                    var book = new Book()
                    {
                        Name = model.Name,
                        NameEn = model.NameEn,
                        CategoryId = model.CategoryId,
                        Count = model.Count,
                        ImagePath = nameOfBook,
                        Price = model.Price,
                        //DiscontId = model.DiscontId,
                        Description = model.Description,
                        DescriptionEn = model.DescriptionEn,
                        Year = model.Year,
                        AuthorId = author.Id
                    };
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //foreach(var error in ModelState.Err)
            }
            model.Authors = _context.Authors.ToList();
            model.Categories = _context.Categories.ToList();
            model.Disconts = _context.Disconts.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAuthor(AddAuthorViewModel model)
        {
            if(ModelState.IsValid)
            {
                Author author = _context.Authors.FirstOrDefault(p => p.Name.Equals(model.Name));
                if(author != null)
                {
                    ModelState.AddModelError("Name", "Автор з таким ім'ям вже існує!");
                }
                else
                {
                    author = new Author() { Name = model.Name, NameEn = model.NameEn };
                    _context.Authors.Add(author);
                    _context.SaveChanges();
                    return RedirectToAction("AfterAuthorCreate", new { name = author.Name});
                }
            }
            return View(model);
        }
        public IActionResult AfterAuthorCreate(string name)
        {
            ViewBag.Author = name;
            return View();
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var books = _context.Books.Include(p => p.Author).Include(p => p.Category).ToList();
            return View(books);
        }
        [HttpGet]
        public IActionResult EditSingleBook(int bookId)
        {
            EditBookViewModel model = new EditBookViewModel();
            Book book = _context.Books.Include(p => p.Author).Include(p => p.Category).FirstOrDefault(p => p.Id == bookId);
            if(book != null)
            {
                model.Id = book.Id;
                model.Authors = _context.Authors.ToList();
                model.Categories = _context.Categories.ToList();
                model.AuthorName = book.Author.Name;
                model.Name = book.Name;
                model.NameEn = book.NameEn;
                model.Description = book.Description;
                model.DescriptionEn = book.DescriptionEn;
                model.Count = book.Count;
                model.CategoryId = book.CategoryId;
                model.Year = book.Year;
                model.Price = book.Price;
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditSingleBook(EditBookViewModel model)
        {
            if(ModelState.IsValid)
            {
                Book book = _context.Books.Find(model.Id);
                if(book != null)
                {
                    Author author = _context.Authors.FirstOrDefault(p => p.Name.Equals(model.AuthorName));
                    if(author != null)
                    {
                        book.AuthorId = author.Id;
                        book.Name = model.Name;
                        book.NameEn = model.NameEn;
                        book.Description = model.Description;
                        book.DescriptionEn = model.DescriptionEn;
                        book.CategoryId = model.CategoryId;
                        book.Count = model.Count;
                        book.Year = model.Year;
                        _context.Books.Update(book);
                        _context.SaveChanges();
                        return RedirectToAction("Edit");
                    }
                }
            }
            model.Categories = _context.Categories.ToList();
            model.Authors = _context.Authors.ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBook(int bookId)
        {
            Book book = _context.Books.Find(bookId);
            if(System.IO.File.Exists("wwwroot/images/books/" + book.ImagePath))
            {
                System.IO.File.Delete("wwwroot/images/books/" + book.ImagePath);
                System.IO.File.Delete("wwwroot/images/books/min_" + book.ImagePath);
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Edit");
        }
        public IActionResult Authors()
        {
            var authors = _context.Authors.OrderBy(p => p.Name).ToList();
            return View(authors);
        }
        [HttpGet]
        public IActionResult EditAuthor(int id)
        {
            Author author = _context.Authors.Find(id);
            return View(author);
        }
        [HttpPost]
        public IActionResult EditAuthor(Author model)
        {
            if(ModelState.IsValid)
            {
                _context.Authors.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Authors");
            }
            return View(model);
        }

        public IActionResult Users()
        {
            _roleManager.Roles.Select(p => p.Name);
            ViewBag.Roles = _roleManager.Roles.Select(p => p.Name).ToList();
            var users = _manager.Users.ToList();
            var models = users.Select(p => new SingleUserFromListViewModel() { Name = p.Name, Email = p.Email, LastName = p.LastName, UserName = p.UserName, UserRoles =  _manager.GetRolesAsync(p).Result }).ToList();
            return View(models);
        }
        [HttpPost]
        public IActionResult ChangeRoles(ChangeRoleModel model)
        {
            User user = _manager.FindByNameAsync(model.UserName).Result;
            foreach (var role in _roleManager.Roles.ToList())
            {
                var result = _manager.RemoveFromRoleAsync(user, role.Name.ToUpper()).Result;
            }
            foreach (var role in model.Roles)
            {
                var nresult = _manager.AddToRoleAsync(user, role.ToUpper()).Result;
            }
            return RedirectToAction("Users");
        }

    }
}