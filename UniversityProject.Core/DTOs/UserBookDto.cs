using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class UserBookDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public List<BookTitle> BookTitles { get; set; }
        public List<User> Users { get; set; }
    }
}
