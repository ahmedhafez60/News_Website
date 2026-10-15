using WebApplication1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace News_Website.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly NewContext _context;

        public AdminController(NewContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // إجمالي الأخبار
            ViewBag.TotalNews = await _context.News.CountAsync();
            ViewBag.TotalCategories = await _context.Categories.CountAsync();
            ViewBag.TotalRenders = await _context.Readers.CountAsync();
            ViewBag.TotalContacts = await _context.Contacts.CountAsync();

            // التصنيفات وعدد الأخبار داخل كل تصنيف
            var categories = await _context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Count = _context.News.Count(n => n.CategoryId == c.Id)
                })
                .ToListAsync();

            ViewBag.CategoryNames = categories.Select(c => c.Name).ToArray();
            ViewBag.CategoryCounts = categories.Select(c => c.Count).ToArray();

            // آخر 5 أخبار
            var latestNews = await _context.News
                .Include(n => n.Category)
                .OrderByDescending(n => n.Date)
                .Take(5)
                .ToListAsync();
            var latestRenders = await _context.Readers
                .OrderByDescending(n => n.Id)
                .Take(5)
                .ToListAsync();
            ViewBag.LatestNews = latestNews;
            ViewBag.LatestRenders = latestRenders;

            return View();
        }
    }
}