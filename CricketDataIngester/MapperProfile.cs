using System.Linq;
using AutoMapper;
using CricketDataIngester.Elastic;
using CricketDataIngester.YamlParser;
using Inning = CricketDataIngester.YamlParser.Inning;
using Player = CricketDataIngester.Data.Player;

namespace CricketDataIngester
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Deliveries, Ball>()
                .ForMember(dest => dest.Batsman, opt => opt.Ignore())
                .ForMember(dest => dest.Bowler, opt => opt.Ignore())
                .ForMember(dest => dest.NonStriker, opt => opt.Ignore());
            CreateMap<Player, Elastic.Player>();
            CreateMap<Inning, Elastic.Inning>()
                .ForMember(dest => dest.BattingTeam, opt => opt.MapFrom(src => src.Team));
            CreateMap<MatchInfo, Elastic.Match>();

        }
    }
}
