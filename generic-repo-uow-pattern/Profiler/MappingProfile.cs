using AutoMapper;
using generic_repo_uow_pattern.Dto;
using generic_repo_uow_pattern.Models;

namespace generic_repo_uow_pattern.Profiler
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ///CreateMap<entity, Dto>();
            CreateMap<Product, ProductRequest>()
                .ReverseMap();
        }
    }
}
