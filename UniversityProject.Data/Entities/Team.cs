using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Data.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(300)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string JobTitle { get; set; }

        [MaxLength(300)]
        public string Image { get; set; }

        [MaxLength(200)]
        public string Twitter { get; set; }

        [MaxLength(200)]
        public string Instagram { get; set; }

        [MaxLength(200)]
        public string Facebook { get; set; }
    }
}
