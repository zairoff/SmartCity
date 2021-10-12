using Microsoft.EntityFrameworkCore;
using Sport.Domain.Models;
using Sport.Infrastructure.Base;
using Sport.Service.Exceptions;
using Sport.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.Service
{
    public class ApplicantService : IApplicantService
    {
        private readonly IRepository<Applicant> _applicantRepository;

        public ApplicantService(IRepository<Applicant> applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        public async Task<Applicant> AddAsync(Applicant applicant)
        {
            var existing = await _applicantRepository.Get()
                                    .Where(a => a.PersonId == applicant.PersonId &&
                                    a.VacancyId == applicant.VacancyId).FirstOrDefaultAsync();

            if (existing != null)
                throw new ResourceExistException($"PersonId:{applicant.PersonId} already submitted");            

            await _applicantRepository.AddAsync(applicant);

            return applicant;
        }

        public async Task<Applicant> DeleteAsync(int id)
        {
            var applicant = await _applicantRepository.Get()
                                        .Where(a => a.Id == id)
                                        .FirstOrDefaultAsync();

            if (applicant == null)
                throw new NotFoundException("Not found");

            await _applicantRepository.DeleteAsync(applicant);

            return applicant;
        }

        // For large of databases we should use pagination
        public async Task<IEnumerable<Applicant>> GetAllAsync()
        {
            return await _applicantRepository.GetAll()
                                            .Include(a => a.Vacancy)
                                            .ThenInclude(a => a.Position)
                                            .ToListAsync();                                            
        }

        public async Task<Applicant> GetAsync(int id)
        {
            return await _applicantRepository.Get()
                                            .Where(a => a.Id == id)
                                            .Include(a => a.Vacancy)
                                            .ThenInclude(a => a.Position)
                                            .FirstOrDefaultAsync();
        }

        public async Task<Applicant> GetByPersonIdAsync(string personId)
        {
            return await _applicantRepository.Get()
                                            .Where(a => a.PersonId == personId)
                                            .Include(a => a.Vacancy)
                                            .ThenInclude(a => a.Position)
                                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Applicant>> GetByVacancyIdAsync(int vacancyId)
        {
            return await _applicantRepository.Get()
                                            .Where(a => a.VacancyId == vacancyId)
                                            .Include(a => a.Vacancy)
                                            .ThenInclude(a => a.Position)
                                            .ToListAsync();
        }
    }
}
