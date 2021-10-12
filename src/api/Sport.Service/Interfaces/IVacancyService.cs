using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IVacancyService
    {
        Task<IEnumerable<Vacancy>> GetAllAsync();

        Task<IEnumerable<Vacancy>> GetByComplexIdAsync(int complexId);

        Task<Vacancy> GetAsync(int id);

        Task<IEnumerable<Vacancy>> GetByStatusAsync(int complexId, bool status);

        Task<IEnumerable<Vacancy>> GetByPositionIdAsync(int complexId, int positionId);

        Task<Vacancy> AddAsync(Vacancy vacancy);

        Task<Vacancy> UpdateAsync(int id, Vacancy vacancy);

        Task<Vacancy> DeleteAsync(int id);
    }
}
