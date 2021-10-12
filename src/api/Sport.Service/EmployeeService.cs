using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetByPersonIdAsync(int complexId, string personId)
        {
            return await _employeeRepository.Get()
                                        .Where(e => e.PersonId == personId &&
                                                e.ComplexId == complexId)
                                        .Include(e => e.Position)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Employee> GetAsync(int id)
        {
            return await _employeeRepository.Get()
                                        .Where(e => e.Id == id)
                                        .Include(e => e.Position)
                                        .FirstOrDefaultAsync();
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            var existing = await _employeeRepository.Get()
                                        .Where(e => e.ComplexId == employee.ComplexId &&
                                        e.PersonId == employee.PersonId)
                                        .Include(e => e.Position)
                                        .FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException($"Employee with PersonID: {employee.PersonId} already exist");

            await _employeeRepository.AddAsync(employee);

            return employee;
        }

        public async Task<Employee> UpdateAsync(int id, Employee employee)
        {
            var existing = await _employeeRepository.Get()
                                            .Where(e => e.Id == id)
                                            .Include(e => e.Position)
                                            .FirstOrDefaultAsync();

            if (existing == null)
                throw new NotFoundException("Not found");

            existing.PositionId = employee.PositionId;

            await _employeeRepository.UpdateAsync(existing);

            return existing;
        }

        public async Task<Employee> DeleteAsync(int id)
        {
            var employee = await _employeeRepository.Get()
                                            .Where(e => e.Id == id)
                                            .Include(e => e.Position)
                                            .FirstOrDefaultAsync();

            if (employee == null)
                throw new NotFoundException("Not found");

            await _employeeRepository.DeleteAsync(employee);

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetByPositionIdAsync(int complexId, int positionId)
        {
            return await _employeeRepository.Get()
                                    .Where(e => e.ComplexId == complexId &&
                                    e.PositionId == positionId)
                                    .Include(e => e.Position)
                                    .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepository.GetAll()
                                       .Include(e => e.Position)
                                       .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByComplexIdAsync(int complexId)
        {
            return await _employeeRepository.GetAll()
                                        .Where(e => e.ComplexId == complexId)
                                        .Include(e => e.Position)
                                        .ToListAsync();
        }
    }
}
