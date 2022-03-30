using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace UniversityProject.Core.DTOs
{
    public class SliderDto
    {
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public int Position { get; set; }
        public List<BookTitle> BookTitles { get; set; }
        public int Id { get; set; }
    }

    public class BookTitle
    {
        public string LinkType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
