/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTO;

/**
 * @author gerzon
 */
[Table("Country", Schema = "Tide")]
public class Country
{
    [Key]
    [Required]
    [StringLength(2)]
    public required string Code { get; set; }

    [Required]
    [StringLength(32)]
    public required string Name { get; set; }

    [Required]
    public required bool Active { get; init; }
    
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}