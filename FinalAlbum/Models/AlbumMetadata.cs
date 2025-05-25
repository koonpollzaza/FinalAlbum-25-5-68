using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinalAlbum.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalAlbum.Models
{
    public class AlbumMetadata
    {
    }


    [MetadataType(typeof(AlbumMetadata))]
    public partial class Album
    {
        [NotMapped]
        public IFormFile? Ifile { get; set; }
        public List<Album> GetAll(AlbumContext dbContext, string searchString)
        {
            return dbContext.Albums.Where(q => q.IsDelete != true)
                                   .Include(f => f.File)
                                   .Include(s => s.Songs.Where(q => q.IsDelete != true))
                                   .Where(a => string.IsNullOrEmpty(searchString) || a.Name.Contains(searchString))
            .ToList();
        }

        public bool Create(AlbumContext dbContext, IFormFile? Ifile)
        {

            DateTime datenow = DateTime.Now;

            File file = new File();
            file = File.Create(dbContext, Ifile);
            this.FileId = file.Id;

            List<Song> songs = this.Songs.ToList();

            IsDelete = false;
            CreateBy = "pon";
            CreateDate = datenow;

            foreach (Song s in songs)
            {
                if (!string.IsNullOrEmpty(s.Name))
                {
                    s.Create(dbContext, this.Id); // Create SongMetadata
                }
            }
            dbContext.Albums.Add(this);
            dbContext.SaveChanges();
            return true;
        }

        public Album? GetById(AlbumContext dbContext, int id)
        {
            Album? album = dbContext.Albums.Include(d => d.Songs.Where(q => q.IsDelete != true))
                                           .Include(f => f.File)
                                           .FirstOrDefault(q => q.IsDelete != true && q.Id == id);
            return album;
        }

        public Album Edit(AlbumContext dbContext, IFormFile? Ifile)
        {
            DateTime datenow = DateTime.Now;

            this.UpdateBy = "pon";
            this.UpdateDate = datenow;

            //อัพไฟล์ใหม่
            if (Ifile != null && Ifile.Length > 0)
            {
                File new_File = File.Create(dbContext, Ifile);
                if (new_File != null)
                {
                    this.FileId = new_File.Id;
                }
            }

            //อัพเพลง
            foreach (Song song in this.Songs)
            {
                if (song.Id > 0 && !string.IsNullOrWhiteSpace(song.Name))
                {
                    song.UpdateBy = "pon";
                    song.UpdateDate = datenow;
                    song.IsDelete = false;
                }
                else 
                {
                    song.CreateBy = "pon";
                    song.CreateDate = datenow;
                    song.UpdateBy = "pon";
                    song.UpdateDate = datenow;
                    song.IsDelete = false;
                }
            }
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            return this;
        }
        public bool Delete(AlbumContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            foreach (Song song in this.Songs)
            {
                song.Delete(dbContext);
            }

            File.Delete(dbContext);

            IsDelete = true;
            UpdateBy = "pon";
            UpdateDate = datenow;
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            return true;
        }
    }

}