using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IPocketService
    {
        Task<IEnumerable<Pocket>> GetAllAsync();

        Task<Pocket> GetAsync(int id);

        Task<Pocket> AddAsync(Pocket pocket);

        Task<Pocket> UpdateAsync(int id, Pocket pocket);

        Task<Pocket> DeleteAsync(int id);
    }
}
