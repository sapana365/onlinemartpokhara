
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlinemartpokhara.Data;
using onlinemartpokhara.Models;
using System.Diagnostics;

using onlinemartpokhara.Utility;

using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using onlinemartpokhara.ViewModels;


namespace onlinemartpokhara.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        
        private ApplicationDbContext _db;

        public object Session { get; private set; }

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int? page)
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList().ToPagedList(page ?? 1, 9));

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Detail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);

            ViewBag.ProductsId = id.Value;

            var comments = _db.ArticlesComments.Where(d => d.ProductsId.Equals(id.Value)).ToList();
            ViewBag.Comments = comments;
             var ratings = _db.ArticlesComments.Where(d => d.ProductsId.Equals(id.Value)).ToList();
                if (ratings.Count > 0)
                {
                    var ratingSum = ratings.Sum(d => d.Rating.Value);
                    ViewBag.RatingSum = ratingSum;
                    var ratingCount = ratings.Count();
                    ViewBag.RatingCount = ratingCount;
                }
                else
                {
                    ViewBag.RatingSum = 0;
                    ViewBag.RatingCount = 0;
                }
            
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.ProductsId = id.Value;


            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }       
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]

        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        //GET product Cart action method
        [Authorize]
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }
        //public ActionResult CreateReview()
        //{
        //    return View();
        //}

        // POST: ArticleCommentsController/Create
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(ArticleCommentViewModel vm)
        {
            var comment = vm.Comment;
            var articleId = vm.ProductsId;
            var rating = vm.Rating;

            ArticlesComment artComment = new ArticlesComment()
            {
                ProductsId = articleId,
                Comments = comment,
                Rating = rating,
                ThisDateTime = DateTime.Now
            };

            _db.ArticlesComments.Add(artComment);
            _db.SaveChanges();

            return RedirectToAction("Detail", "Home", new { id = articleId });
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateReview(int CommentId, string Comments, DateTime ThisDateTime, int ProductsId, int? Rating, ArticlesComment articlesComment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.ArticlesComments.Add(articlesComment);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(articlesComment);
        //}



    }
}
