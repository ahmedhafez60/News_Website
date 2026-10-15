
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;


[Authorize(Roles = "Admin")]
public class ReadersController : Controller
{
    private readonly NewContext _context;

    public ReadersController(NewContext context)
    {
        _context = context;
    }

    // GET: READERSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Readers.ToListAsync());
    }

    // GET: READERSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var readers = await _context.Readers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (readers == null)
        {
            return NotFound();
        }

        return View(readers);
    }

    // GET: READERSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: READERSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Rating,Review,CreatedAt")] Readers readers)
    {
        if (ModelState.IsValid)
        {
            _context.Add(readers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(readers);
    }

    // GET: READERSS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var readers = await _context.Readers.FindAsync(id);
        if (readers == null)
        {
            return NotFound();
        }
        return View(readers);
    }

    // POST: READERSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Rating,Review,CreatedAt")] Readers readers)
    {
        if (id != readers.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(readers);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReadersExists(readers.Id))
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
        return View(readers);
    }

    // GET: READERSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var readers = await _context.Readers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (readers == null)
        {
            return NotFound();
        }

        return View(readers);
    }

    // POST: READERSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var readers = await _context.Readers.FindAsync(id);
        if (readers != null)
        {
            _context.Readers.Remove(readers);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReadersExists(int? id)
    {
        return _context.Readers.Any(e => e.Id == id);
    }
}
