using AutoMapper;
using BasicCMR.Application.DTOs.JobApplications;
using BasicCMR.Application.DTOs.Dashboard;
using BasicCMR.Domain.Entities;

namespace BasicCMR.Application.Mappings
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // 🧩 JobApplication-mappningar
            CreateMap<JobApplication, JobApplicationDto>().ReverseMap();
            CreateMap<CreateJobApplicationDto, JobApplication>();
            CreateMap<UpdateJobApplicationDto, JobApplication>();

            // 🧩 Dashboard-mappningar
            CreateMap<JobApplication, RecentJobApplicationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AppliedAt, opt => opt.MapFrom(src => src.AppliedAt));
        }
    }
}
