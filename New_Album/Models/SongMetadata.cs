using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace New_Album.Models
{
    public class SongMetadata
    {
        [Required(ErrorMessage = "Please enter the song title.")]
        public string Name { get; set; } = null!;
    }

    [MetadataType(typeof( SongMetadata))]
    public partial class Song
    {
        public Song Create(AlbumDbContext dbContext, int AlbumId)
        {
            DateTime datenow = DateTime.Now;
            this.AlbumId = AlbumId;
            this.IsDelete = false;
            this.CreateBy = "Toon";
            this.CreateDate = datenow;
            this.UpdateBy = "Toon";
            this.UpdateDate = datenow;
            dbContext.Songs.Add(this);
            //dbContext.SaveChanges();
            return this;
        } 

        public Song Update(AlbumDbContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            Song existingSong = dbContext.Songs.AsNoTracking().FirstOrDefault(s => s.Id == this.Id);

            List<Song> allSongIds = dbContext.Songs.Where(s => s.AlbumId == this.AlbumId && !s.IsDelete)
                                                   .AsNoTracking()
                                                   .ToList();
            List<int> thisSongIds = dbContext.Songs.Where(s => s.Id != 0)
                                                   .Select(s => s.Id)
                                                   .ToList();

            foreach (Song oldSong in allSongIds)
            {
                if (!thisSongIds.Contains(oldSong.Id))
                {
                    oldSong.IsDelete = true;
                    oldSong.UpdateBy = "Toon";
                    oldSong.UpdateDate = DateTime.Now;
                }
               
            }

            if (this.Id != 0 && existingSong.Name != Name)
            {
                this.UpdateBy = "Toon";
                this.UpdateDate = datenow;
            }
            else
            {
                //this.AlbumId = this.Id;
                this.CreateBy = "Toon";
                this.CreateDate = datenow;
                this.UpdateBy = "Toon";
                this.UpdateDate = datenow;
                this.IsDelete = false;
                
            }

            return this;
        }

        public Song Delete(AlbumDbContext dbContext)
        {

            DateTime datenow = DateTime.Now;

            this.IsDelete = true;
            this.UpdateBy = "Toon";
            this.UpdateDate = datenow;
            dbContext.Songs.Update(this);
            return this;

        }

    }
}
