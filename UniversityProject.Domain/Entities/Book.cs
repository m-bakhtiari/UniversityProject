using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Domain.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(800)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(800)]
        public string AuthorName { get; set; }

        [MaxLength(500)]
        public string PublisherName { get; set; }

        public bool IsAvailable { get; set; }

        public int UsableDays { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public bool IsDelete { get; set; }

        [MaxLength(300)]
        public string ImageName { get; set; }

        #region Relations

        public List<Comment> Comments { get; set; }

        #endregion   
    }
}
