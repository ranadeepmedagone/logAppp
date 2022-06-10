using System.Text.Json.Serialization;
using logapp.DTOs;

namespace logapp.Models;

public record Tag
{

    public int Id { get; set; }


    public string Name { get; set; }



    public int TypeId { get; set; }



    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;






    public TagDTO asDto => new TagDTO
    {
        Id = Id,
        Name = Name,
        TypeId = TypeId,
        CreatedAt = CreatedAt,


    };

    
}

