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
            CreateMap<KumiteCategory, KumiteCategoryDto>().ReverseMap();
            CreateMap<Competitor, CompetitorDto>()
                .ForMember(dest => dest.DegreeName, opt => opt.MapFrom(src => src.Degree.DegreeName))
                .ForMember(dest => dest.ClubName, opt => opt.MapFrom(src => src.Club.ClubName))
                .ReverseMap()
                .ForMember(dest => dest.Club, opt => opt.Ignore())
                .ForMember(dest => dest.Degree, opt => opt.Ignore()); 
            CreateMap<Tournament, TournamentDto>().ReverseMap();
            CreateMap<TourCompetitor, TourCompetitorDto>()
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tournament.TourName))
                .ForMember(dest => dest.TourPlace, opt => opt.MapFrom(src => src.Tournament.TourPlace))
                .ForMember(dest => dest.TourDate, opt => opt.MapFrom(src => src.Tournament.TourDate))
                .ForMember(dest => dest.KataCatName, opt => opt.MapFrom(src => src.TourCatKata != null ? src.TourCatKata.KataCategory.KataCatName : "Brak"))
                .ForMember(dest => dest.KumiteCatName, opt => opt.MapFrom(src => src.TourCatKumite != null ? src.TourCatKumite.KumiteCategory.KumiteCatName : "Brak"))
                .ReverseMap()
                .ForMember(dest => dest.Tournament, opt => opt.Ignore())
                .ForMember(dest => dest.Competitor, opt => opt.Ignore())
                .ForMember(dest => dest.TourCatKata, opt => opt.Ignore())
                .ForMember(dest => dest.TourCatKumite, opt => opt.Ignore());
            CreateMap<TourCatKumite, TourCatKumiteDto>()
                .ForMember(dest => dest.KumiteCatName, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatName))
                .ReverseMap()
                .ForMember(dest => dest.KumiteCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Mat, opt => opt.Ignore())
                .ForMember(dest => dest.Tour, opt => opt.Ignore());
            CreateMap<TourCatKata, TourCatKataDto>()
                .ForMember(dest => dest.KataCatName, opt => opt.MapFrom(src => src.KataCategory.KataCatName))
                .ReverseMap()
                .ForMember(dest => dest.KataCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Mat, opt => opt.Ignore())
                .ForMember(dest => dest.Tour, opt => opt.Ignore());
        }
    }
}
