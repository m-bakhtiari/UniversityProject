namespace UniversityProject.Core.DTOs
{
    public class LoginDto
    {
        public int Username { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
