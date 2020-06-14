namespace ElasticRepository.Entities
{
    public class Runs
    {
        public int Extras { get; set; }
        public int Total { get; set; }
        public int Batsman { get; set; }

        public int IsNonBoundary { get; set; }

        public int DotsTotal => Total == 0 ? 1 : 0;

        public int DotBatsman => Batsman == 0 ? 1 : 0;

        public int OneRunTotal => Total == 1 ? 1 : 0;

        public int OneRunBatsman => Batsman == 1 ? 1 : 0;

        public int OneRunExtra => Extras == 1 ? 1 : 0;

        public int TwoRunsTotal => Total == 2 ? 1 : 0;

        public int TwoRunsBatsman => Batsman == 2 ? 1 : 0;

        public int TwoRunsExtra => Extras == 2 ? 1 : 0;

        public int ThreeRunsTotal => Total == 3 ? 1 : 0;

        public int ThreeRunsBatsman => Batsman == 3 ? 1 : 0;

        public int ThreeRunsExtra => Extras == 3 ? 1 : 0;

        public int FourRunsTotal => Total == 4 ? 1 : 0;

        public int FourRunsBatsman => Batsman == 4 ? 1 : 0;
        public int FourBoundaryRunsBatsman => Batsman == 4 && IsNonBoundary == 0 ? 1 : 0;

        public int FourRunsExtra => Extras == 4 ? 1 : 0;

        public int FiveRunsTotal => Total == 5 ? 1 : 0;

        public int FiveRunsBatsman => Batsman == 5 ? 1 : 0;

        public int FiveRunsExtra => Extras == 5 ? 1 : 0;

        public int SixRunsTotal => Total == 6 ? 1 : 0;

        public int SixRunsBatsman => Batsman == 6 ? 1 : 0;
        public int SixBoundaryRunsBatsman => Batsman == 6 && IsNonBoundary == 0 ? 1 : 0;

        public int SixRunsExtra => Extras == 6 ? 1 : 0;
        public int SevenRunsTotal => Total == 7 ? 1 : 0;
        public int SevenRunsExtras => Extras == 7 ? 1 : 0;
    }
}