
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;



[Authorize(Roles = "Admin")]
public class TeamnumbersController : Controller
{
    private readonly NewContext _context;

    public TeamnumbersController(NewContext context)
    {
        _context = context;
    }

    // GET: TEAMNUMBERSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Teamnumbers.ToListAsync());
    }

    // GET: TEAMNUMBERSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teamnumbers = await _context.Teamnumbers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teamnumbers == null)
        {
            return NotFound();
        }

        return View(teamnumbers);
    }

    // GET: TEAMNUMBERSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TEAMNUMBERSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
   
    public async Task<IActionResult> Create([Bind("Id,Name,Jobtitle,Image")] Teamnumbers teamnumbers, IFormFile? imageFile)
    {
        // استبعاد التحقق الإجباري لحقل الصورة لتجنب أخطاء ModelState
        ModelState.Remove("Image");

        if (ModelState.IsValid)
        {
            // لو المستخدم رفع صورة
            if (imageFile != null && imageFile.Length > 0)
            {
                // توليد اسم فريد وامتداد الملف بطريقة سليمة
                string extension = Path.GetExtension(imageFile.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + extension;

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

                // حفظ المسار النسبي الصحيح في قاعدة البيانات
                teamnumbers.Image = "/images/" + uniqueFileName;
            }
            else
            {
                // صورة افتراضية في حال لم يقم المستخدم برفع صورة
                teamnumbers.Image = "/images/default.png";
            }

            _context.Add(teamnumbers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(teamnumbers);
    }


    // GET: TEAMNUMBERSS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teamnumbers = await _context.Teamnumbers.FindAsync(id);
        if (teamnumbers == null)
        {
            return NotFound();
        }
        return View(teamnumbers);
    }

    // POST: TEAMNUMBERSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Jobtitle,Image")] Teamnumbers teamnumbers, IFormFile? imageFile)
    {
        if (id != teamnumbers.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Image");

        if (ModelState.IsValid)
        {
            try
            {
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

                    teamnumbers.Image = "/images/" + uniqueFileName;
                }
                else
                {
                    // الاحتفاظ بالصورة القديمة في حال عدم رفع صورة جديدة
                    var existingTeamMember = await _context.Teamnumbers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                    if (existingTeamMember != null)
                    {
                        teamnumbers.Image = existingTeamMember.Image;
                    }
                }

                _context.Update(teamnumbers);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamnumbersExists(teamnumbers.Id))
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
        return View(teamnumbers);
    }

    // GET: TEAMNUMBERSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teamnumbers = await _context.Teamnumbers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teamnumbers == null)
        {
            return NotFound();
        }

        return View(teamnumbers);
    }

    // POST: TEAMNUMBERSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var teamnumbers = await _context.Teamnumbers.FindAsync(id);
        if (teamnumbers != null)
        {
            _context.Teamnumbers.Remove(teamnumbers);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeamnumbersExists(int? id)
    {
        return _context.Teamnumbers.Any(e => e.Id == id);
    }
}
