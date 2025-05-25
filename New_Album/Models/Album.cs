using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace New_Album.Models;

[Table("Album")]
public partial class Album
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? FileId { get; set; }

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    public string? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    public string? UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    public bool IsDelete { get; set; }

    [ForeignKey("FileId")]
    [InverseProperty("Albums")]
    public virtual File? File { get; set; }

    [InverseProperty("Album")]
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
