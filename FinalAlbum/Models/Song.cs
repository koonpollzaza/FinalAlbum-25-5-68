﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalAlbum.Models;

[Table("Song")]
public partial class Song
{
    [Key]
    public int Id { get; set; }

    public int AlbumId { get; set; }

    [Required]
    public string Name { get; set; }

    public string CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [StringLength(50)]
    public string UpdateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    public bool? IsDelete { get; set; }

    [ForeignKey("AlbumId")]
    [InverseProperty("Songs")]
    public virtual Album Album { get; set; }
}
