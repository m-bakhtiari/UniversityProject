using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityProject.Data.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey(nameof(ParentId))]
        public Category SubCategory { get; set; }
        #endregion
    }
}
