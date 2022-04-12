using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniversityProject.Core.DTOs;

namespace UniversityProject.WebApp.Controllers
{
    public class AuthenticationController : Controller
    {
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            //var claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //};
            return null;
        }
    }
}
