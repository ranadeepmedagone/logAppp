using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using logapp.DTOs;

namespace logapp.Models;

public record TagDTO
{

    [JsonPropertyName("tag_id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]

    public string Name { get; set; }


    [JsonPropertyName("type_id")]

    public int TypeId { get; set; }



    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;



    [JsonPropertyName("type_name")]

    public List<TagTypeDTO> TagTypes { get; set; }


    [JsonPropertyName("logs")]

    public List<LogDTO> ListOfLogs { get; set; }

}


public record CreateTagDTO
{
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]

    public string Name { get; set; }

    [JsonPropertyName("type_id")]

    public int TypeId { get; set; }


}


public record UpdateTagDTO
{
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]

    public string Name { get; set; }

}

