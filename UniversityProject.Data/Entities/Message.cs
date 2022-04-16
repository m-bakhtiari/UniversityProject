using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Data.Entities
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(300)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Phone { get; set; }

        [MaxLength(800)]
        public string Subject { get; set; }

        [Required]
        public string Text { get; set; }

        public bool ReadingStatus { get; set; }

        public DateTime RecordDate { get; set; }
    }
}
