using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Common.Enums
{
    public enum AddMemberStatus
    {
        Created,
        EmailAlreadyExists,
        UserNameAlreadyExists,
        CreationFailed
    }
}
