using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface ISportTypeService
    {
        Task<IEnumerable<SportType>> GetAllAsync();

        Task<SportType> GetAsync(int id);

        Task<SportType> AddAsync(SportType sportType);

        Task<SportType> UpdateAsync(int id, SportType sportType);

        Task<SportType> DeleteAsync(int id);
    }
}
