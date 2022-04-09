﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Data.Entities
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

        public int UsableDays { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public bool IsDelete { get; set; }

        [MaxLength(300)]
        public string ImageName { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
        public DateTime AddedDate { get; set; }

        #region Relations

        public List<Comment> Comments { get; set; }
        public List<UserBook> UserBooks { get; set; }
        public List<BookCategory> BookCategories { get; set; }

        #endregion   
    }
}
