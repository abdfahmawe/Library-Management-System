using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.DAL.Data.Seed
{
    public interface IDataSeed
    {
        Task SeedDataAsync();
    }
}
