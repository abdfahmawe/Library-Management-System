using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Setting
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public int DurationInMinutes { get; set; }

    }
}
