/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTO;

/**
 * @author gerzon
 */
[Table("Station", Schema = "Tide")]
public class Station
{
    [Key]
    [Required]
    [StringLength(4)]
    public required string Code { get; set; }

    [Required]
    [StringLength(128)]
    public required string Name { get; set; }

    [Required]
    [StringLength(512)]
    public required string Location { get; set; }

    [Required]
    [StringLength(15)]
    public required string IpAddress { get; set; }

    [Required]
    public required double Latitude { get; set; }

    [Required]
    public required double Longitude { get; set; }

    [Required]
    public required double Depth { get; set; }

    [Required]
    public required Organization Organization { get; set; }

    [Required]
    public required bool Active { get; init; }

    public ICollection<Data> Data { get; set; } = new List<Data>();
}