/*
 * El código fuente de este archivo es propiedad intelectual de Gerzon Gonzalez.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTO;

/**
 * @author gerzon
 */
[Table("Data", Schema = "Tide")]
public class Data
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Station? Station { get; set; }

    [Required]
    public DateTime Time { get; init; } = DateTime.Now;

    [Required]
    public float Value { get; set; } = -1;
}