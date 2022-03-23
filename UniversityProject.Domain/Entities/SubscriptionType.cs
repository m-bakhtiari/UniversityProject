using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Domain.Entities
{
    public class SubscriptionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        public string Description { get; set; }

        #region Relations

        public List<User> Users { get; set; }

        #endregion    
    }
}
