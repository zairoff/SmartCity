using Sport.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport.Service.Interfaces
{
    public interface IApplicantService
    {
        Task<IEnumerable<Applicant>> GetAllAsync();

        Task<Applicant> GetAsync(int id);

        Task<IEnumerable<Applicant>> GetByVacancyIdAsync(int vacancyId);

        Task<Applicant> GetByPersonIdAsync(string personId);

        Task<Applicant> AddAsync(Applicant applicant);

        Task<Applicant> DeleteAsync(int id);
    }
}
