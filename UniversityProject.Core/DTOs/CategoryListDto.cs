using System.Collections.Generic;
using UniversityProject.Domain.Entities;

namespace UniversityProject.Core.DTOs
{
    public class CategoryListDto: PagingInfo
    {
        public List<Category> Categories { get; set; }
    }
}
