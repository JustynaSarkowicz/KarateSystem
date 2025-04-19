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
                // Tournament info
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tournament.TourName))
                .ForMember(dest => dest.TourPlace, opt => opt.MapFrom(src => src.Tournament.TourPlace))
                .ForMember(dest => dest.TourDate, opt => opt.MapFrom(src => src.Tournament.TourDate))
                // Competitor basic info
                .ForMember(dest => dest.CompFirstName, opt => opt.MapFrom(src => src.Competitor.CompFirstName))
                .ForMember(dest => dest.CompLastName, opt => opt.MapFrom(src => src.Competitor.CompLastName))
                .ForMember(dest => dest.CompDateOfBirth, opt => opt.MapFrom(src => src.Competitor.CompDateOfBirth))
                .ForMember(dest => dest.CompGender, opt => opt.MapFrom(src => src.Competitor.CompGender))
                .ForMember(dest => dest.CompWeight, opt => opt.MapFrom(src => src.Competitor.CompWeight))
                // Degree and Club
                .ForMember(dest => dest.CompDegreeId, opt => opt.MapFrom(src => src.Competitor.CompDegreeId))
                .ForMember(dest => dest.DegreeName, opt => opt.MapFrom(src => src.Competitor.Degree.DegreeName))
                .ForMember(dest => dest.CompClubId, opt => opt.MapFrom(src => src.Competitor.CompClubId))
                .ForMember(dest => dest.ClubName, opt => opt.MapFrom(src => src.Competitor.Club.ClubName))
                // Categories
                .ForMember(dest => dest.KataCatName, opt => opt.MapFrom(src => src.TourCatKata != null ? src.TourCatKata.KataCategory.KataCatName : "Brak"))
                .ForMember(dest => dest.KumiteCatName, opt => opt.MapFrom(src => src.TourCatKumite != null ? src.TourCatKumite.KumiteCategory.KumiteCatName : "Brak"))
                .ReverseMap()
                .ForMember(dest => dest.Tournament, opt => opt.Ignore())
                .ForMember(dest => dest.Competitor, opt => opt.Ignore())
                .ForMember(dest => dest.TourCatKata, opt => opt.Ignore())
                .ForMember(dest => dest.TourCatKumite, opt => opt.Ignore());

            CreateMap<TourCatKumite, TourCatKumiteDto>()
                .ForMember(dest => dest.KumiteCatName, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatName))
                .ForMember(dest => dest.KumiteCatGender, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatGender))
                .ForMember(dest => dest.KumiteCatAgeMin, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatAgeMin))
                .ForMember(dest => dest.KumiteCatAgeMax, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatAgeMax))
                .ForMember(dest => dest.KumiteCatWeightMin, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatWeightMin))
                .ForMember(dest => dest.KumiteCatWeightMax, opt => opt.MapFrom(src => src.KumiteCategory.KumiteCatWeightMax))
                .ForMember(dest => dest.MatName, opt => opt.MapFrom(src => src.Mat.MatName))
                .ReverseMap()
                .ForMember(dest => dest.KumiteCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Mat, opt => opt.Ignore())
                .ForMember(dest => dest.Tour, opt => opt.Ignore());

            CreateMap<TourCatKata, TourCatKataDto>()
                .ForMember(dest => dest.KataCatName, opt => opt.MapFrom(src => src.KataCategory.KataCatName))
                .ForMember(dest => dest.KataCatGender, opt => opt.MapFrom(src => src.KataCategory.KataCatGender))
                .ForMember(dest => dest.KataCatAgeMin, opt => opt.MapFrom(src => src.KataCategory.KataCatAgeMin))
                .ForMember(dest => dest.KataCatAgeMax, opt => opt.MapFrom(src => src.KataCategory.KataCatAgeMax))
                .ForMember(dest => dest.MatName, opt => opt.MapFrom(src => src.Mat.MatName))
                .ForMember(dest => dest.CatKataDegrees, opt => opt.MapFrom(src => src.KataCategory.CatKataDegrees))
                .ReverseMap()
                .ForMember(dest => dest.KataCategory, opt => opt.Ignore())
                .ForMember(dest => dest.Mat, opt => opt.Ignore())
                .ForMember(dest => dest.Tour, opt => opt.Ignore());
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
