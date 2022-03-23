using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Core.DTOs
{
    public class CategorySearchDto : PagingInfo
    {
        public string Title { get; set; }
    }
}
