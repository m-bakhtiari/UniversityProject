using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class DashboardDto
    {
        public List<FavoriteBookDto> FavoriteBooks { get; set; }
        public List<FavoriteBookDto> ShoppingCartBooks { get; set; }
        public List<UserBook> OldBooks { get; set; }
        public LoginDto LoginDto { get; set; }
        public User User { get; set; }
    }

    public class FavoriteBookDto
    {
        public Book Book { get; set; }
        public bool IsAvailable { get; set; }
    }
}
