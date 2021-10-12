using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IPositionService
    {
        Task<IEnumerable<Position>> GetAllAsync();

        Task<Position> GetAsync(int id);

        Task<Position> AddAsync(Position position);

        Task<Position> UpdateAsync(int id, Position position);

        Task<Position> DeleteAsync(int id);
    }
}
