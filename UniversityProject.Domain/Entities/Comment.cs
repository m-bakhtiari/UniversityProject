using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityProject.Domain.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [MaxLength(3500)]
        public string Text { get; set; }

        [Required]
        public DateTime RecordDate { get; set; }

        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Comment BookComment { get; set; }
        #endregion
    }
}
