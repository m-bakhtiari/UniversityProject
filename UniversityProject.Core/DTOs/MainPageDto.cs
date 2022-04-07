using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class MainPageDto
    {
        public List<Slider> Sliders { get; set; }
        public List<Book> FavoriteBooks { get; set; }
        public List<Book> LatestBooks { get; set; }
        public List<Banner> Banners { get; set; }
    }
}
