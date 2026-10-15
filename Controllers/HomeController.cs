using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WebApplication1.Data;

namespace News_Website.Controllers
{
    public class HomeController : Controller
    {
        NewContext db ;
            public HomeController(NewContext  context)
        {
            db = context;

        }
        public IActionResult Index()
        {
            // 1. إرسال قائمة الأقسام في ViewBag
            ViewBag.Categories = db.Categories.ToList();

            // 2. جلب أحدث الأخبار مع القسم الخاص بكل خبر
            var latestNews = db.News
                               .Include(n => n.Category)
                               .OrderByDescending(n => n.Date)
                               .Take(6) // جلب أحدث 6 أخبار فقط
                               .ToList();

           

            return View(latestNews);
        }
        public IActionResult News(int? id, int page = 1)
        {
            int pageSize = 6; // عدد الأخبار في الصفحة الواحدة

            // 1. الاستعلام الأساسي مع تضمين القسم (Include Category)
            var newsQuery = db.News.Include(n => n.Category).AsQueryable();

            // 2. تطبيق منطق الـ ID وتحديث ViewData["name"] تماماً كما في كودك الأصلي
            if (id == null)
            {
                ViewData["name"] = "جميع الأخبار";
            }
            else
            {
                var category = db.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                ViewData["name"] = category.Name;
                newsQuery = newsQuery.Where(n => n.CategoryId == id);
            }

            // 3. حساب إجمالي عدد الأخبار والصفحات بناءً على الفلترة الحالية
            int totalItems = newsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // 4. جلب 6 أخبار فقط حسب رقم الصفحة مع ترتيب الأحدث أولاً
            var newsList = newsQuery
                .OrderByDescending(n => n.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 5. تمرير بيانات الترقيم للـ View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.CategoryId = id;

            return View(newsList);
        }


        public IActionResult Details(int id)
        {
            var newsItem = db.News.Include(n => n.Category)
                                  .FirstOrDefault(n => n.Id == id);

            if (newsItem == null)
            {
                return NotFound();
            }
            ViewBag.rselatedNews = db.News
                            .Where(n => n.CategoryId == newsItem.CategoryId && n.Id != id) // استثناء الخبر الحالي
                            .OrderByDescending(n => n.Date)
                            .Take(4)
                            .ToList();

            
            return View(newsItem);
        }

        public IActionResult Readers()
        {
            var res = db.Readers.ToList();
            return View(res);
        }

        [HttpPost]
        public IActionResult SaveReaders(Readers model)
        {
            db.Readers.Add(model);
            db.SaveChanges();
            return RedirectToAction("Readers");
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveContact(ContactUs model)
        {
            // لو البيانات غير صحيحة أو فيه حقل فاضي مطلوب
            if (!ModelState.IsValid)
            {
                // إرجاع النموذج الممتلئ بنفس البيانات للـ View
                return View("Contact", model);
            }

            if (!string.IsNullOrEmpty(model.Email) && !new EmailAddressAttribute().IsValid(model.Email))
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني المكتوب غير صحيح!");
            }

            db.Contacts.Add(model);
            db.SaveChanges();

            // رسالة نجاح في حالة الحفظ
            TempData["SuccessMessage"] = "تم إرسال رسالتك بنجاح!";

            return RedirectToAction("Contact");
        }


        public IActionResult Teamnumbers()
        {
            var result = db.Teamnumbers.ToList();
            return View(result);
          
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
