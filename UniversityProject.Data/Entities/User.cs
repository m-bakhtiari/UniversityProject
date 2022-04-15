using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityProject.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.)]
        //public int Username { get; set; }

        [MaxLength(300)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string Phone { get; set; }

        [MaxLength(300)]
        [Required]
        public string Password { get; set; }

        public double? Penalty { get; set; }

        public bool IsDelete { get; set; }

        public DateTime RegisterTime { get; set; }

        public int RoleId { get; set; }

        #region Relations

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        #endregion

    }
}
