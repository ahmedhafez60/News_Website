using WebApplication1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;


[Authorize(Roles = "Admin")]
public class ContactUsController : Controller
{
    private readonly NewContext _context;

    public ContactUsController(NewContext context)
    {
        _context = context;
    }

    // GET: CONTACTUSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Contacts.ToListAsync());
    }

    // GET: CONTACTUSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contactus = await _context.Contacts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contactus == null)
        {
            return NotFound();
        }

        return View(contactus);
    }

    // GET: CONTACTUSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CONTACTUSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,Subject,Message")] ContactUs contactus)
    {
        if (ModelState.IsValid)
        {
            _context.Add(contactus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(contactus);
    }

    // GET: CONTACTUSS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contactus = await _context.Contacts.FindAsync(id);
        if (contactus == null)
        {
            return NotFound();
        }
        return View(contactus);
    }

    // POST: CONTACTUSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Email,Subject,Message")] ContactUs contactus)
    {
        if (id != contactus.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(contactus);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactUsExists(contactus.Id))
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
        return View(contactus);
    }

    // GET: CONTACTUSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var contactus = await _context.Contacts
            .FirstOrDefaultAsync(m => m.Id == id);
        if (contactus == null)
        {
            return NotFound();
        }

        return View(contactus);
    }

    // POST: CONTACTUSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var contactus = await _context.Contacts.FindAsync(id);
        if (contactus != null)
        {
            _context.Contacts.Remove(contactus);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ContactUsExists(int? id)
    {
        return _context.Contacts.Any(e => e.Id == id);
    }
}
