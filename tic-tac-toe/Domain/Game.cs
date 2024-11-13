using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Game : BaseEntity
{
    [MaxLength(128)]
    public string GameName { get; set; } = default!;
    
    [MaxLength(10240)]
    public string GameState { get; set; } = default!;

    public int ConfigId { get; set; }
    public Config? Config { get; set; }
}
