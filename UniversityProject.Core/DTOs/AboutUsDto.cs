using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class AboutUsDto
    {
        public List<Team> Teams { get; set; }
        public int BookCount { get; set; }
        public int UserCount { get; set; }
        public int CategoryCount { get; set; }
        public int CommentCount { get; set; }
    }
}
