using System;
using System.Collections.Generic;

namespace Cars.COMMON.DTOs
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string RefreshToken { get; set; }
        public string JWTToken { get; set; }
    }
}
