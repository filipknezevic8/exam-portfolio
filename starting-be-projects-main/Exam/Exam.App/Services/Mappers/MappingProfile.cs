using AutoMapper;
using Exam.App.Domain;
using Exam.App.Services.Dtos;

namespace Exam.App.Services.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, ProfileDto>();
            CreateMap<Project, ProjectDto>();
        }
    }
}
