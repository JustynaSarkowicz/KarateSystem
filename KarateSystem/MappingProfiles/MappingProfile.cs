using AutoMapper;
using KarateSystem.Dto;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Degree, DegreeDto>().ReverseMap();
            CreateMap<Club, ClubDto>().ReverseMap();
            CreateMap<Mat, MatDto>().ReverseMap();
            CreateMap<KataCategory, KataCategoryDto>()
                 .ReverseMap()
                 .ForMember(dest => dest.TourCatKatas, opt => opt.Ignore());

            CreateMap<CatKataDegree, CatKataDegreeDto>()
                .ForMember(dest => dest.DegreeName, opt => opt.MapFrom(src => src.Degree.DegreeName))
                .ReverseMap();
        }
    }
}
