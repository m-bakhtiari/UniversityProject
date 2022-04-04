using System.Collections.Generic;
using UniversityProject.Data.Entities;

namespace UniversityProject.Core.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public int? Penalty { get; set; }
        public int RoleId { get; set; }
        public int Username { get; set; }
        public List<Role> Roles { get; set; }
    }
}
