using System.ComponentModel.DataAnnotations;
using Tarifikacija.Entities;

namespace VikoSoft.Data.Entities;

public class Report : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }
}