using WebApplication1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


[Authorize(Roles = "Admin")]
public class NewsController : Controller        
{
    private readonly NewContext db;

    public NewsController(NewContext context)
    {
        db = context;
    }

    // GET: NEWSS
    public async Task<IActionResult> Index()    
    {
        var newsList = await db.News
                                 .Include(n => n.Category)
                                 .ToListAsync();

        return View(newsList);
    }

    // GET: NEWSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var news = await db.News.Include(n => n.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (news == null)
        {
            return NotFound();
        }

        return View(news);
    }

    // GET: NEWSS/Create
    // GET: News/Create
   
    public IActionResult Create()
    {
        // تعبئة القائمة المنسدلة للتصنيفات
        ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Date,Topic,CategoryId")] News news, IFormFile imageFile)
    {
        // استبعاد التحقق الإجباري لكائن الـ Category والصورة لضمان نجاح الحفظ
        ModelState.Remove("Category");
        ModelState.Remove("Image");

        if (ModelState.IsValid)
        {
            // معالجة ورفع الصورة
            if (imageFile != null && imageFile.Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                news.Image = "/images/" + uniqueFileName;
            }
            else
            {
                news.Image = "/images/default.png"; // صورة افتراضية لو المستخدم مترفش صورة
            }

            // حفظ البيانات في قاعدة البيانات
            db.Add(news);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // لو فيه خطأ، بنرجع نملا القائمة المنسدلة تاني عشان الصفحة تعرض التصنيفات
        ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", news.CategoryId);
        return View(news);
    }
    // POST: NEWSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

    // GET: News/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var news = await db.News.FindAsync(id);
    if (news == null)
    {
        return NotFound();
    }

    // تعبئة القائمة المنسدلة للتصنيفات لكي تظهر في صفحة التعديل
    ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", news.CategoryId);
    return View(news);
}


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Date,Topic,CategoryId,Image")] News news, IFormFile? imageFile)
    {
        if (id != news.Id)
        {
            return NotFound();
        }

        // استبعاد التحقق الإجباري لكائن الـ Category لتجنب خطأ الـ ModelState
        ModelState.Remove("Category");

        if (ModelState.IsValid)
        {
            try
            {
                // لو المستخدم رفع صورة جديدة أثناء التعديل
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // تحديث مسار الصورة الجديدة
                    news.Image = "/images/" + uniqueFileName;
                }
                else
                {
                    // لو مرفعش صورة جديدة، بنجيب الصورة القديمة من الداتابيز عشان متتكتبش فاضية
                    var existingNews = await db.News.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
                    if (existingNews != null)
                    {
                        news.Image = existingNews.Image;
                    }
                }

                db.Update(news);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(news.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // لو حصل خطأ في الـ ModelState، بنرجع نعبي القائمة المنسدلة للتصنيفات
        ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", news.CategoryId);
        return View(news);
    }






    // GET: NEWSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var news = await db.News.Include(n => n.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (news == null)
        {
            return NotFound();
        }

        return View(news);
    }

    // POST: NEWSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var news = await db.News.FindAsync(id);
        if (news != null)
        {
            db.News.Remove(news);
        }

        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool NewsExists(int? id)
    {
        return db.News.Any(e => e.Id == id);
    }
}
