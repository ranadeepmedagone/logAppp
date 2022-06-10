using System.Text.Json.Serialization;
using logapp.DTOs;

namespace logapp.Models;

public record User
{

    public int Id { get; set; }


    public string Name { get; set; }


    public string Email { get; set; }

    public string HashPassword { get; set; }


    public DateTimeOffset LastLogin { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;


    public bool Status { get; set; }


    public bool IsSuperUser { get; set; }

    public UserDTO asDto => new UserDTO
    {
        Id = Id,
        Name = Name,
        Email = Email,
        HashPassword = HashPassword,
        LastLogin = LastLogin,
        CreatedAt = CreatedAt,
        Status = Status,
        IsSuperUser = IsSuperUser


    };
}

