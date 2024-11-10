using System.ComponentModel.DataAnnotations;

namespace Domain;

public class SaveGame
{
    // Primary Key
    public int Id { get; set; }

    [MaxLength(128)]
    public string CreatedAtDateTime { get; set; } = default!;

    [MaxLength(10240)]
    public string State { get; set; } = default!;

    // Expose the Foreign Key
    public int ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }

}