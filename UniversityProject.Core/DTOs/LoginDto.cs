namespace UniversityProject.Core.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public bool RememberMe { get; set; }
        public string Name { get; set; }
        public int? Id { get; set; }
        public int Code { get; set; }
    }
}
