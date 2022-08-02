using System;

namespace Cars.COMMON.DTOs
{
    public class RefreshTokenDTO
    {
        public string RefreshToken { get; set; }
        public DateTime Expiry { get; set; }
    }
}
