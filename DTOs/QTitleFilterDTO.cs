using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
// using Nconnect.Utilities;

namespace logapp.DTOs
{
    public record QTitleFilterDTO
    {
        private string _title = null;


        [FromQuery(Name = "title")]
        public string Title { get => _title; set => _title = value; }


    }


}