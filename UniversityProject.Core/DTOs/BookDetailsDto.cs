using System;
using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class BookDetailsDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string AuthorName { get; set; }
        public string PublisherName { get; set; }
        public int UsableDays { get; set; }
        public DateTime PublishDate { get; set; }
        public string ImageName { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime AddedDate { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Comment> Answers { get; set; }
        public int PageId { get; set; } = 1;
        public int CountAll { get; set; }
        public List<Category> Categories { get; set; }
        public string UserCommentText { get; set; }
        public bool CommentPart { get; set; }
    }

}
