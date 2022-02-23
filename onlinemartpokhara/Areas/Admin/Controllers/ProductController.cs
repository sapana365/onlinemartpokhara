using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using onlinemartpokhara.Data;
using onlinemartpokhara.Models;
using onlinemartpokhara.ViewModels;

namespace onlinemartpokhara.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;

        private readonly IWebHostEnvironment _he;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;

        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(f => f.SpecialTag).ToList());
        }



        //POST Index action method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }
            return View(products);
        }
        //Get Create method
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            return View();
        }

        //Post Create method
        [HttpPost]
        public async Task<IActionResult> Create(Products product)
        {
            var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
            if (searchProduct != null)
            {
                ViewBag.message = "This product is already exist";
                ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                return View(product);
            }
            
            
                string wwwwRootPath = _he.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFIle.FileName);
                string extension = Path.GetExtension(product.ImageFIle.FileName);
                product.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwwRootPath + "/Images/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await product.ImageFIle.CopyToAsync(fileStream);
            }
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

           
        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST Edit Action Method
        [HttpPost]
        public async Task<IActionResult> Edit(Products product)
        {
                    
                    string wwwwRootPath = _he.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFIle.FileName);
                    string extension = Path.GetExtension(product.ImageFIle.FileName);
                    product.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwwRootPath + "/Images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFIle.CopyToAsync(fileStream);
                    }

                
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          
        }

            
        //GET Details Action Method
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

;
            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
               .FirstOrDefault(c => c.Id == id);
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

        //GET Delete Action Method

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.Include(c => c.SpecialTag).Include(c => c.ProductTypes).Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }

        //POST Delete Action Method

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
