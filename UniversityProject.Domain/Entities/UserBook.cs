using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityProject.Domain.Entities
{
    public class UserBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #region Relations

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        #endregion

    }
}
