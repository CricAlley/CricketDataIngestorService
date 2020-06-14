using ElasticRepository.Entities;
using Nest;

namespace ElasticRepository.Indices
{
    public class IndexCreator
    {
        private readonly ElasticClient _elasticClient;

        public IndexCreator()
        {
            _elasticClient = new ElasticClientProvider().GetElasticClient();
        }

        public CreateIndexResponse CreateIplIndex()
        {
            return _elasticClient.Indices.Create("iplballs", c => c
                .Map<Ball>(m => m
                    .AutoMap<Ball>()
                    .AutoMap<Match>()
                    .AutoMap<Outcome>()
                    .AutoMap<Toss>()
                    .AutoMap<BowlOutDeliveries>()
                    .AutoMap<Inning>()
                    .AutoMap<Player>()
                    .AutoMap<Runs>()
                    .AutoMap<Extras>()
                    .AutoMap<Wicket>()
                    .AutoMap<Replacements>()
                    .AutoMap<PenaltyRuns>()
                    .AutoMap<ReplacementRole>()
                    .AutoMap<ReplacementMatch>()
                )
            );
        }
    }
}