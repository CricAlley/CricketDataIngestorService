using System.Linq;
using AutoMapper;
using PlayerDataGenerator.Data;
using PlayerDataGenerator.JsonParser;
using Elastic = ElasticRepository.Entities;

namespace PlayerDataGenerator
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Delivery, Elastic.Ball>()
                .ForMember(dest => dest.Batsman, opt => opt.Ignore())
                .ForMember(dest => dest.Bowler, opt => opt.Ignore())
                .ForMember(dest => dest.NonStriker, opt => opt.Ignore());
            CreateMap<Player, Elastic.Player>();
            CreateMap<Innings, Elastic.Inning>()
                .ForMember(dest => dest.IsDeclared, opt => opt.MapFrom(src => src.Declared));
            CreateMap<Info, Elastic.Match>();
            CreateMap<Officials, Elastic.Officials>();
            CreateMap<Event, Elastic.Event>();
            CreateMap<Outcome, Elastic.Outcome>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(outcome => outcome.Result ?? "Finished"));
            CreateMap<BowlOut, Elastic.BowlOut>();
            CreateMap<By, Elastic.By>();
            CreateMap<Extras, Elastic.Extras>();
            CreateMap<PenaltyRuns, Elastic.PenaltyRuns>();
            CreateMap<Match, Elastic.ReplacementMatch>();
            CreateMap<Role, Elastic.ReplacementRole>();
            CreateMap<Replacements, Elastic.Replacements>();
            CreateMap<Runs, Elastic.Runs>();
            CreateMap<Toss, Elastic.Toss>();
            CreateMap<Wicket, Elastic.Wicket>();
            CreateMap<Over, Elastic.Ball>();
            CreateMap<Fielder, Elastic.Fielder>();                
            CreateMap<Target, Elastic.Target>();
            CreateMap<Innings, Elastic.Ball>()
                .ForMember(x => x.IsSuperOver, opt => opt.MapFrom(src => src.SuperOver));
            CreateMap<Info, Elastic.Ball>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Dates.First()));
            CreateMap<Review, Elastic.Ball>()
                .ForMember(dest => dest.ReviewBy, opt => opt.MapFrom(src => src.By))
                .ForMember(dest => dest.ReviewUmpire, opt => opt.MapFrom(src => src.Umpire))
                .ForMember(dest => dest.ReviewBatter, opt => opt.MapFrom(src => src.Batter))
                .ForMember(dest => dest.IsUmpiresCall, opt => opt.MapFrom(src => src.UmpiresCall))
                .ForMember(dest => dest.ReviewDecision, opt => opt.MapFrom(src => src.Decision));
        }
    }
}