using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Data.Entities
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(400)]
        public string Image { get; set; }

        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string LinkUrl { get; set; }

        public int Position { get; set; }

    }
}
