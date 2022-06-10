using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace logapp.Models;

public record TagTypeDTO
{

    // [JsonPropertyName("type_id")]
    // public int Id { get; set; }

    [JsonPropertyName("type_name")]
    [Required]
    [MaxLength(255)]

    public string TypeName { get; set; }



}

// public record CreateTagTypeDTO
// {
//     [JsonPropertyName("type_name")]
//     [Required]
//     [MaxLength(255)]

//     public string TypeName { get; set; }

// }