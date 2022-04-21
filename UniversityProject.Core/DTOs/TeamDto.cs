using Microsoft.AspNetCore.Http;

namespace UniversityProject.Core.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string JobTitle { get; set; }

        public IFormFile Image { get; set; }

        public string OldImage { get; set; }
    }
}
