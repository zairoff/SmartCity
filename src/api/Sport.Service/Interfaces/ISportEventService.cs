using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface ISportEventService
    {
        Task<IEnumerable<SportEvent>> GetAllAsync();

        Task<IEnumerable<SportEvent>> GetByComplexIdAsync(int complexId);

        Task<SportEvent> GetAsync(int id);

        Task<SportEvent> AddAsync(SportEvent sportEvent);

        Task<SportEvent> UpdateAsync(int id, SportEvent sportEvent);

        Task<SportEvent> DeleteAsync(int id);
    }
}
