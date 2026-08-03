using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.DTOs.Response.Identity
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}
