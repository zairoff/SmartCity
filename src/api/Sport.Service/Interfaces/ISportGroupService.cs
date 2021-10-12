using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface ISportGroupService
    {
        Task<IEnumerable<SportGroup>> GetAllAsync();

        Task<SportGroup> GetAsync(int id);

        Task<SportGroup> AddAsync(SportGroup group);

        Task<SportGroup> UpdateAsync(int id, SportGroup group);

        Task<SportGroup> DeleteAsync(int id);
    }
}
