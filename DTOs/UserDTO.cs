using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using logapp.Models;

namespace logapp.DTOs;

public record UserDTO
{
    [JsonPropertyName("user_id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]

    public string Name { get; set; }


    [JsonPropertyName("email")]
    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [JsonPropertyName("hash_password")]
    public string HashPassword { get; set; }

    [JsonPropertyName("last_login")]

    public DateTimeOffset LastLogin { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    [JsonPropertyName("status")]

    public bool Status { get; set; }

    [JsonPropertyName("is_superuser")]

    public bool IsSuperUser { get; set; }

    [JsonPropertyName("tags")]

    public List<TagDTO> ListOfTags { get; set; }


}

public record UserLoginDTO
{
    [Required]
    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("hash_password")]
    public string HashPassword { get; set; }

    



}


public record UserLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("is_superuser")]

    public bool IsSuperUser { get; set; }



}

public record UserCreateDTO
{
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]

    public string Name { get; set; }


    [JsonPropertyName("email")]
    [Required]
    [MaxLength(255)]
    public string Email { get; set; }

    [JsonPropertyName("hash_password")]
    public string HashPassword { get; set; }

}

public record UserUpdateDTO
{
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(255)]

    public string Name { get; set; }

    [JsonPropertyName("hash_password")]
    public string HashPassword { get; set; }


}