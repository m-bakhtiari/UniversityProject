using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityProject.Domain.Entities
{
    public class ShoppingCart
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public int BookId { get; set; }

        #region Relations

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; }
        #endregion

    }
}
