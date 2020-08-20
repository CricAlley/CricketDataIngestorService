using System.Linq;
using AutoMapper;
using PlayerDataGenerator.Data;
using PlayerDataGenerator.YamlParser;
using Elastic = ElasticRepository.Entities;

namespace PlayerDataGenerator
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Deliveries, Elastic.Ball>()
                .ForMember(dest => dest.Batsman, opt => opt.Ignore())
                .ForMember(dest => dest.Bowler, opt => opt.Ignore())
                .ForMember(dest => dest.NonStriker, opt => opt.Ignore());
            CreateMap<Player, Elastic.Player>();
            CreateMap<Inning, Elastic.Inning>()
                .ForMember(dest => dest.BattingTeam, opt => opt.MapFrom(src => src.Team));
            CreateMap<MatchInfo, Elastic.Match>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(info => info.Dates.First()));
            CreateMap<Outcome, Elastic.Outcome>()
                .ForMember(dest => dest.Result, opt => opt.MapFrom(outcome => outcome.Result ?? "Finished"));
            CreateMap<BowlOutDeliveries, Elastic.BowlOutDeliveries>();
            CreateMap<By, Elastic.By>();
            CreateMap<Extras, Elastic.Extras>();
            CreateMap<PenaltyRuns, Elastic.PenaltyRuns>();
            CreateMap<ReplacementMatch, Elastic.ReplacementMatch>();
            CreateMap<ReplacementRole, Elastic.ReplacementRole>();
            CreateMap<Replacements, Elastic.Replacements>();
            CreateMap<Runs, Elastic.Runs>();
            CreateMap<Toss, Elastic.Toss>();
            CreateMap<Wicket, Elastic.Wicket>();
        }
    }
}