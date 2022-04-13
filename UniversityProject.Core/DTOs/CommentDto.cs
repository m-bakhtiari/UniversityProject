using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Core.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public int BookId { get; set; }
        public DateTime RecordDate { get; set; }
        public int? ParentId { get; set; }
        public int PageId { get; set; } = 1;
        public int CountAll { get; set; }
    }
}
