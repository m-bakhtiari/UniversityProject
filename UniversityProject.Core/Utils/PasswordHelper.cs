using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace UniversityProject.Core.Utils
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMd5(string pass) //Encrypt using MD5   
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)   
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string   
            return BitConverter.ToString(encodedBytes);
        }

        public static int GetUserId(this IPrincipal principal)
        {
            var simplePrinciple = (ClaimsPrincipal)principal;
            if (simplePrinciple.Identity is ClaimsIdentity identity)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                return int.Parse(userIdClaim.Value);
            }
            throw new Exception("User id is not defined");
        }
        public static int GetRoleId(this IPrincipal principal)
        {
            var simplePrinciple = (ClaimsPrincipal)principal;
            if (simplePrinciple.Identity is ClaimsIdentity identity)
            {
                var roleIdClaim = identity.FindFirst(ClaimTypes.Role);

                return int.Parse(roleIdClaim.Value);
            }
            throw new Exception("User id is not defined");
        }
    }
}
