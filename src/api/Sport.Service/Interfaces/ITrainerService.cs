using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<Trainer>> GetAllAsync();

        Task<IEnumerable<Trainer>> GetByComplexIdAsync(int complexId);

        Task<Trainer> GetAsync(int id);

        Task<IEnumerable<Trainer>> GetBySportTypeIdAsync(int complexId, int sportTypeId);

        Task<Trainer> GetByEmployeeIdAsync(int complexId, int employeeId);

        Task<Trainer> AddAsync(Trainer trainer);

        Task<Trainer> UpdateAsync(int id, Trainer trainer);

        Task<Trainer> DeleteAsync(int id);
    }
}
