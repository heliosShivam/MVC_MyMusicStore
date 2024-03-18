using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_MyMusicStore.Data;
using MVC_MyMusicStore.Models;
using X.PagedList;

namespace MVC_MyMusicStore.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AlbumsController : Controller
    {
        private readonly AppDbContext _db;

        public AlbumsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: Albums
        [HttpGet]
        public IActionResult Index(string searchText , int? page)
        {
            
            IQueryable<Album> albums  = _db.Albums.Include(a => a.Artist).Include(a => a.Genre);
            if (!string.IsNullOrEmpty(searchText))
            {
                albums = albums.Where(a => (a.Title.Contains(searchText) 
                || a.Artist.Name.Contains(searchText) 
                || a.Genre.Name.Contains(searchText)));
            }
            int pageNumber = page ?? 1;
            int pageSize = 10;

            return View(albums.OrderBy(a => a.Title).ToList());
        }


        // GET: Albums/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name");
            return View();
        }

        // POST: Albums/Create

        [HttpPost]
        public IActionResult Create(Album a)
        {
            if (ModelState.IsValid)
            {
                _db.Albums.Add(a);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("Error");
                ViewBag.ArtistId = new SelectList(_db.Artists, "ArtistId", "Name", a.ArtistId);
                ViewBag.GenreId = new SelectList(_db.Genres, "GenreId", "Name", a.GenreId);
                return View(a);
            }

        }

        // GET: Albums/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _db.Albums.Find(id);
            Console.WriteLine("Print :" + album);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewData["GenreId"] = new SelectList(_db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Albums.Update(album);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
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
            ViewData["ArtistId"] = new SelectList(_db.Artists, "ArtistId", "ArtistId", album.ArtistId);
            ViewData["GenreId"] = new SelectList(_db.Genres, "GenreId", "GenreId", album.GenreId);
            return View(album);
        }

        // GET: Albums/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _db.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefault(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var album = _db.Albums.Find(id);
            if (album != null)
            {
                _db.Albums.Remove(album);
            }

            _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _db.Albums.Any(e => e.AlbumId == id);
        }
    }
}
