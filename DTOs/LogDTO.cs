using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using logapp.Models;

namespace logapp.DTOs;

public record LogDTO
{
    public static bool PartiallyDeleted { get; internal set; }
    [JsonPropertyName("log_id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    [Required]
    [MaxLength(500)]

    public string Title { get; set; }


    [JsonPropertyName("description")]

    public string Description { get; set; }

    [JsonPropertyName("stack_trace")]
    public string StackTrace { get; set; }



    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;



    [JsonPropertyName("updated_by_user_id")]

    public int UpdatedByUserId { get; set; }

    [JsonPropertyName("partially_deleted")]

    public bool PartiaLlyDeleted { get; set; }

    // [JsonPropertyName("log_seen")]

    // public List<LogSeenDTO> LogSeenBy { get; set; }



    [JsonPropertyName("tags")]

    public List<TagDTO> ListOfTags { get; set; }

    // [JsonPropertyName("user")]

    // public List<UserDTO> UpdatedByUser { get; set; }


}

public record CreateLogDTO
{
    [JsonPropertyName("title")]
    [Required]
    [MaxLength(500)]

    public string Title { get; set; }


    [JsonPropertyName("description")]

    public string Description { get; set; }

    [JsonPropertyName("stack_trace")]
    public string StackTrace { get; set; }
}


public record UpdateLogDTO
{
    

    [JsonPropertyName("description")]

    public string Description { get; set; }


    


}


public record DeleteLogDTO
{



    [JsonPropertyName("partially_deleted")]

    public bool PartiallyDeleted { get; set; }


}






