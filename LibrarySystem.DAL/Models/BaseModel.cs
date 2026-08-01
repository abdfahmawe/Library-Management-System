using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Models
{
   public abstract class BaseModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
