using System.ComponentModel.DataAnnotations;
using System.Data;
using FinalAlbum.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalAlbum.Models
{
    public class SongMetadata
    {
        [Required(ErrorMessage = "Please enter the song title.")]
        public string Name { get; set; } = null!;
    }

    [MetadataType(typeof(SongMetadata))]
    public partial class Song
    {
        public Song Create(AlbumContext dbContext, int AlbumId)
        {
            DateTime datenow = DateTime.Now;
            this.AlbumId = AlbumId;
            this.IsDelete = false;
            this.CreateBy = "pon";
            this.CreateDate = datenow;
            dbContext.Songs.Add(this);
            return this;
        }

        public Song Edit(AlbumContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            Song existingSong = dbContext.Songs.AsNoTracking().FirstOrDefault(s => s.Id == this.Id);

            List<Song> allSongIds = dbContext.Songs.Where(s => s.AlbumId == this.AlbumId && s.IsDelete == false)
                                                   .AsNoTracking()
                                                   .ToList();
            List<int> thisSongIds = dbContext.Songs.Where(s => s.Id != 0)
                                                   .Select(s => s.Id)
                                                   .ToList();

            foreach (Song oldSong in allSongIds)
            {
                if (!thisSongIds.Contains(oldSong.Id))
                {
                    oldSong.IsDelete = false;
                    oldSong.UpdateBy = "pon";
                    oldSong.UpdateDate = DateTime.Now;
                }
            }
            return this;
        }

        public Song Delete(AlbumContext dbContext)
        {

            DateTime datenow = DateTime.Now;

            this.IsDelete = true;
            this.UpdateBy = "pon";
            this.UpdateDate = datenow;
            dbContext.Songs.Update(this);
            return this;

        }

    }
}