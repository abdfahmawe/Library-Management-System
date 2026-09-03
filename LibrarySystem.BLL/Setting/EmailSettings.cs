using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Setting
{
    public class EmailSettings
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmationUrl { get; set; } = null!;
    }
}
