using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface ITraineeService
    {
        Task<IEnumerable<Trainee>> GetAllAsync();

        Task<IEnumerable<Trainee>> GetByComplexIdAsync(int complexId);

        Task<Trainee> GetAsync(int id);

        Task<IEnumerable<Trainee>> GetByGroupIdAsync(int complexId, int groupId);

        Task<IEnumerable<Trainee>> GetByPaymentStatusAsync(int complexId, bool isPaid);

        Task<IEnumerable<Trainee>> GetByPocketIdAsync(int complexId, int pocketId);

        Task<Trainee> GetByPersonIdAsync(int complexId, string personId);

        Task<Trainee> AddAsync(Trainee trainee);

        Task<Trainee> UpdateAsync(int id, Trainee trainee);

        Task<Trainee> DeleteAsync(int id);
    }
}
