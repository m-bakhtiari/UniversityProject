using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Data.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

    }
}
