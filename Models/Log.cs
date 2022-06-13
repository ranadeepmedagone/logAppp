using System.Text.Json.Serialization;
using logapp.DTOs;

namespace logapp.Models;

public record Log
{

    public long Id { get; set; }


    public string Title { get; set; }


    public string Description { get; set; }

    public string StackTrace { get; set; }



    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } 




    public int UpdatedByUserId { get; set; } 


    public bool PartiaLlyDeleted { get; set; }

    public LogDTO asDto => new LogDTO
    {
        Id = Id,
        Title = Title,
        Description = Description,
        StackTrace = StackTrace,
        CreatedAt = CreatedAt,
        UpdatedAt = UpdatedAt,
        UpdatedByUserId = UpdatedByUserId,
        PartiaLlyDeleted = PartiaLlyDeleted


    };


}

