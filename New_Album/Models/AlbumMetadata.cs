using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace New_Album.Models
{
    public class AlbumMetadata
    {
    }


    [MetadataType(typeof(AlbumMetadata))]
    public partial class Album
    {
        [NotMapped]
        public IFormFile? Ifile { get; set; }
        public List<Album> GetAll(AlbumDbContext dbContext ,string searchString)
        {
            return dbContext.Albums.Where(q => q.IsDelete != true)
                                   .Include(f => f.File)
                                   .Include(s => s.Songs.Where(q => q.IsDelete != true))
                                   .Where(a =>  string.IsNullOrEmpty(searchString) || a.Name.Contains(searchString))
                                   .ToList(); 
        }

        public bool Create(AlbumDbContext dbContext, IFormFile? Ifile)
        {
            
            DateTime datenow = DateTime.Now;

            File file = new File();
            file = File.Create(dbContext, Ifile);
            this.FileId = file.Id;

            List<Song> songs = this.Songs.ToList();

            IsDelete = false;
            CreateBy = "Toon";
            CreateDate = datenow;
            UpdateBy = "Toon";
            UpdateDate = datenow;
            dbContext.Albums.Add(this);
            dbContext.SaveChanges();
            

            foreach (Song s in songs)
            {
                if(!string.IsNullOrEmpty(s.Name))
                {   
                    s.Create(dbContext, this.Id); // Create SongMetadata
                }    
            }
            return true;
        
        }

        public Album? GetById(AlbumDbContext dbContext, int id)
        {
            Album? album = dbContext.Albums.Include(d => d.Songs.Where(q => q.IsDelete != true))
                                           .Include(f => f.File)
                                           .FirstOrDefault(q => q.IsDelete != true && q.Id == id);
            return album;
        }

        public Album Update(AlbumDbContext dbContext, IFormFile? Ifile)
        {
            DateTime datenow = DateTime.Now;

            this.UpdateBy = "Toon";
            this.UpdateDate = datenow;

            //อัพไฟล์ใหม่
            if(Ifile != null && Ifile.Length > 0)
            {
                File new_File = File.Create(dbContext, Ifile);
                if(new_File != null)
                {
                    this.FileId = new_File.Id;
                }
            }

            //อัพเพลง
            foreach(Song song in this.Songs)
            {
                if (song.Id > 0 && !string.IsNullOrWhiteSpace(song.Name))
                {
                    song.UpdateBy = "Toon";
                    song.UpdateDate = datenow;
                }
                if(song.Id == 0)
                {
                    song.CreateDate = datenow;
                    song.CreateBy = "Toon";
                }
            }
            
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            return this;
        }

        public bool Delete(AlbumDbContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            foreach(Song song in this.Songs)
            {
                song.Delete(dbContext);
            }

            File.Delete(dbContext);

            IsDelete = true;
            UpdateBy = "Toon";
            UpdateDate = datenow;
            dbContext.Albums.Update(this);
            dbContext.SaveChanges();
            return true;
        }
    }

}