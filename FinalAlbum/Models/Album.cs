using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalAlbum.Models;

[Table("Album")]
public partial class Album
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int? FileId { get; set; }

    [Required]
    public string Description { get; set; }

    [StringLength(50)]
    public string CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(50)]
    public string UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    [ForeignKey("FileId")]
    [InverseProperty("Albums")]
    public virtual File File { get; set; }

    [InverseProperty("Album")]
    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
