/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTO;

/**
 * @author gerzon
 */
[Table("Organization", Schema = "Tide")]
public class Organization
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public required string Name { get; set; }

    [Required]
    public required Country Country { get; set; }

    [Required]
    public required bool Active { get; init; }

    public ICollection<Station> Stations { get; set; } = new List<Station>();
}