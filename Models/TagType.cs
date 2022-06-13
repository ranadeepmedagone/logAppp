using System.Text.Json.Serialization;

namespace logapp.Models;

public record TagType{

    public int Id { get; set; }


    public string TypeName { get; set; }

    public TagTypeDTO asDto => new TagTypeDTO
    {
        // Id = Id,
        TypeName = TypeName,
        


    };


}