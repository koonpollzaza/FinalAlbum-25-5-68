using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace New_Album.Models
{
    public class FileMetadata
    {
        [Required(ErrorMessage = "Please enter the song title.")]
        public IFormFile? Ifile { get; set; } = null;
    }
    [MetadataType(typeof(FileMetadata))]
    public partial class File
    {
        public static File Create(AlbumDbContext dbContext, IFormFile? coverFile)
        {
            string originalFileName = Path.GetFileNameWithoutExtension(coverFile.FileName);
            string extension = Path.GetExtension(coverFile.FileName);
            string sanitizedFileName = Regex.Replace(originalFileName, "[^a-zA-Z0-9_-]", "").ToLower();
            string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + sanitizedFileName + extension;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                coverFile.CopyTo(stream);
            }
            DateTime datenow = DateTime.Now;

            File newFile = new File();

            newFile.FileName = fileName;
            newFile.FilePath = "/uploads/" + fileName;
            newFile.IsDelete = false;
            newFile.CreateBy = "Toon";
            newFile.CreateDate = datenow;
            newFile.UpdateBy = "Toon";
            newFile.UpdateDate = datenow;

            dbContext.Files.Add(newFile);
            dbContext.SaveChanges();
            return newFile;
        }


        //public File Update(AlbumDbContext dbContext, IFormFile? coverFile, Album albun)
        //{
        //    string originalFileName = Path.GetFileNameWithoutExtension(coverFile.FileName);
        //    string extension = Path.GetExtension(coverFile.FileName);
        //    string sanitizedFileName = Regex.Replace(originalFileName, "[^a-zA-Z0-9_-]", "").ToLower();
        //    string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + sanitizedFileName + extension;
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

        //    using (FileStream stream = new FileStream(path, FileMode.Create))
        //    {
        //        coverFile?.CopyTo(stream);
        //    }
        //    DateTime datenow = DateTime.Now;

        //    this.FileName = fileName;
        //    this.FilePath = "/uploads/" + fileName;
        //    this.UpdateBy = "Toon";
        //    this.UpdateDate = datenow;
        //    dbContext.Files.Update(this);
        //    return this;
        //}

        public File Delete(AlbumDbContext dbContext)
        {
            DateTime datenow = DateTime.Now;
            this.IsDelete = true;
            this.UpdateBy = "Toon";
            this.UpdateDate = datenow;
            dbContext.Files.Update(this);
            return this;
        }
    }
}
