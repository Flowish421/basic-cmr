using AutoMapper;
using BasicCMR.Application.DTOs.JobApplications;
using BasicCMR.Application.Interfaces;
using BasicCMR.Domain.Entities;
using BasicCMR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BasicCMR.Application.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public JobApplicationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobApplicationDto>> GetAllAsync(int userId)
        {
            var jobs = await _context.JobApplications
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.AppliedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobApplicationDto>>(jobs);
        }

        public async Task<JobApplicationDto> GetByIdAsync(int id, int userId)
        {
            var job = await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);

            if (job == null)
                throw new Exception("Job application not found.");

            return _mapper.Map<JobApplicationDto>(job);
        }

        public async Task<JobApplicationDto> CreateAsync(CreateJobApplicationDto dto, int userId)
        {
            var job = _mapper.Map<JobApplication>(dto);
            job.UserId = userId;
            job.AppliedAt = DateTime.UtcNow;

            _context.JobApplications.Add(job);
            await _context.SaveChangesAsync();

            return _mapper.Map<JobApplicationDto>(job);
        }

        public async Task<JobApplicationDto> UpdateAsync(int id, UpdateJobApplicationDto dto, int userId)
        {
            var job = await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);

            if (job == null)
                throw new Exception("Not found or unauthorized.");

            _mapper.Map(dto, job);
            await _context.SaveChangesAsync();

            return _mapper.Map<JobApplicationDto>(job);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var job = await _context.JobApplications
                .FirstOrDefaultAsync(j => j.Id == id && j.UserId == userId);

            if (job == null)
                throw new Exception("Not found or unauthorized.");

            _context.JobApplications.Remove(job);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
