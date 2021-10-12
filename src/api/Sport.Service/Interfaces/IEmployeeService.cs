using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllAsync();

        Task<IEnumerable<Employee>> GetByComplexIdAsync(int complexId);

        Task<Employee> GetAsync(int id);

        Task<IEnumerable<Employee>> GetByPositionIdAsync(int complexId, int positionId);

        Task<Employee> GetByPersonIdAsync(int complexId, string personId);

        Task<Employee> AddAsync(Employee employee);

        Task<Employee> UpdateAsync(int id, Employee employee);

        Task<Employee> DeleteAsync(int id);
    }
}
