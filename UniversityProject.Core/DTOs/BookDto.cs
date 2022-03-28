using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public bool IsAvailable { get; set; }
        public int UsableDays { get; set; }
        public int PublishYear { get; set; }
        public int PublishMonth { get; set; }
        public bool IsDelete { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<Category> Categories { get; set; }
    }
}
