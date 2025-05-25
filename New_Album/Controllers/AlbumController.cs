
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using New_Album.Models;

namespace New_Album.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumDbContext _context;

        public AlbumController(AlbumDbContext context)
        {
            _context = context;
        }



        public IActionResult Index(string seachString)
        {
            List<Album> album = new Album().GetAll(_context, seachString);

            return View(album);
        }



        [HttpGet]
        public IActionResult Create()
        {
            Album album = new Album();
            album.Songs = new List<Song>(); // เริ่มต้นไม่มีเพลง
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Album album, string actionType, IFormFile Ifile, string? actionDelete)
        {

            if (actionType == "AddSong")
            {
                album.Songs.Add(new Song());
                return View(album);
            }

            //if (actionDelete == "DeleteSong")
            //{
            //    album.Songs.Remove(new Song());
            //    return View(album);
            //}


            if (ModelState.IsValid)
            {
    
                album.Create(_context, Ifile);
                
                return RedirectToAction(nameof(Index));
            }


            return View(album);
        }




        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Album album = new Album().GetById(_context, id.Value);


            
            if (album == null)
            {
                return NotFound();
            }


            return View(album);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Album album, string? actionType, string? OldCoverPhotoPath, string? actionDelete)
        {

            if (actionType == "AddSong")
            {
                album.Songs.Add(new Song());
                if (album.File == null && !string.IsNullOrEmpty(OldCoverPhotoPath))
                {
                    album.File = new Models.File { FilePath = OldCoverPhotoPath };
                }
                return View(album);
            }

            if (actionDelete == "DeleteSong")
            {
                Song son = new Song();
                son.IsDelete = true;
                return View(album);
            }


            IFormFile newIfile = Request.Form.Files["Ifile"];

            if (ModelState.IsValid)
            {
                album.Update(_context, newIfile);
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }




        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Album items = new Album().GetById(_context, id.Value);
            if (items != null)
            {
                items.Delete(_context);

            }
            return RedirectToAction(nameof(Index));
        }


    }
}
