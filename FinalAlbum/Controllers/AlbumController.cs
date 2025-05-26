using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalAlbum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace New_Album.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumContext _context;

        public AlbumController(AlbumContext context)
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
            Album album = new Album
            {
                Songs = new List<Song> { new Song() } // ใส่เปล่าก็ได้ เพื่อให้ EditorFor แสดง
            };

            return View(album);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Album album, string actionType, IFormFile Ifile, string? actionDelete, string? RemoveId)
        {
            album.Songs ??= new List<Song>();

            if (actionType == "AddSong")
            {
                album.Songs.Add(new Song());
                ModelState.Clear();
                return View(album);
            }


            if (!string.IsNullOrEmpty(RemoveId))
            {
                ModelState.Clear();
                return View(album);
            }


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
        public IActionResult Edit(Album album, string? actionType, string? OldCoverPhotoPath, string? actionDelete, string? RemoveId)
        {

            if (actionType == "AddSong")
            {
                album.Songs.Add(new Song());
                if (album.File == null && !string.IsNullOrEmpty(OldCoverPhotoPath))
                {
                    album.File = new FinalAlbum.Models.File { FilePath = OldCoverPhotoPath };
                }
                return View(album);
            }

            if (!string.IsNullOrEmpty(RemoveId))
            {
                ModelState.Clear();
                return View(album);
            }

            if (actionDelete == "DeleteSong")
            {
                return View(album);
            }


            IFormFile newIfile = Request.Form.Files["Ifile"];

            if (ModelState.IsValid)
            {
                album.Edit(_context, newIfile);
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