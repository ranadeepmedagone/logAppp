// using System.Text.Json.Serialization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.ModelBinding;

// namespace logapp.DTOs
// {
//     public record PaginationInputDTO
//     {
//         [FromQuery(Name = "page")]
//         public int Page { get; set; } = 1;

//         [FromQuery(Name = "limit")]
//         public int Limit { get; set; } = 20;

//         [BindNever]
//         public int Offset { get => (Page - 1) * Limit; }

//         [BindNever]
//         public static PaginationInputDTO Max { get => new PaginationInputDTO { Limit = int.MaxValue }; }

//         [BindNever]
//         public static PaginationInputDTO One { get => new PaginationInputDTO { Limit = 1 }; }
//     }
// }