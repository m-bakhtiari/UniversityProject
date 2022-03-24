using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(300)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(300)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        [Required]
        public string Phone { get; set; }

        [MaxLength(300)]
        [Required]
        public string Password { get; set; }

        public int? Penalty { get; set; }

        [Required]
        public int SubscriptionTypeId { get; set; }

        public bool IsDelete { get; set; }

        public DateTime RegisterTime { get; set; }

        public int RoleId { get; set; }

        #region Relations

        [ForeignKey(nameof(SubscriptionTypeId))]
        public SubscriptionType SubscriptionType { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        #endregion

    }
}
