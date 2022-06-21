using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
// using Nconnect.Utilities;

namespace logapp.DTOs
{
    public record QTagFilterDTO
    {
        private string _name = null;


        [FromQuery(Name = "name")]
        public string Name { get => _name; set => _name = value; }


    }


}