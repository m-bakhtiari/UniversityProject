using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class LibraryDto
    {
        public List<Category> Categories { get; set; }
        public List<string> Authors { get; set; }
        public List<string> Publishers { get; set; }
        public string StartPublishDate { get; set; }
        public string EndPublishDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<Book> Books { get; set; }
        public int PageId { get; set; } = 1;
        public int ItemPerPage { get; set; } = 1;
        public List<int> CategoryIdSearch { get; set; }
        public List<string> AuthorNameSearch { get; set; }
        public List<string> PublisherNameSearch { get; set; }
        public string UrlParams { get; set; }
        public int CountAll { get; set; }
        public string Title { get; set; }
        public bool IsAvailable { get; set; }
        public string SortBy { get; set; }
    }
}
