using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class DashboardDto
    {
        public List<Book> FavoriteBooks { get; set; }
        public List<Book> ShoppingCartBooks { get; set; }
        public List<Book> OldBooks { get; set; }
        public User User { get; set; }
    }
}
