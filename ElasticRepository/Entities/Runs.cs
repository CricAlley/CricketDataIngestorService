namespace ElasticRepository.Entities
{
    public class Runs
    {
        public int Extras { get; set; }
        public int Total { get; set; }
        public int Batter { get; set; }

        public int IsNonBoundary { get; set; }

        public int DotsTotal => Total == 0 ? 1 : 0;

        public int DotBatsman => Batter == 0 ? 1 : 0;

        public int OneRunTotal => Total == 1 ? 1 : 0;

        public int OneRunBatsman => Batter == 1 ? 1 : 0;

        public int OneRunExtra => Extras == 1 ? 1 : 0;

        public int TwoRunsTotal => Total == 2 ? 1 : 0;

        public int TwoRunsBatsman => Batter == 2 ? 1 : 0;

        public int TwoRunsExtra => Extras == 2 ? 1 : 0;

        public int ThreeRunsTotal => Total == 3 ? 1 : 0;

        public int ThreeRunsBatsman => Batter == 3 ? 1 : 0;

        public int ThreeRunsExtra => Extras == 3 ? 1 : 0;

        public int FourRunsTotal => Total == 4 ? 1 : 0;

        public int FourRunsBatsman => Batter == 4 ? 1 : 0;
        public int FourBoundaryRunsBatsman => Batter == 4 && IsNonBoundary == 0 ? 1 : 0;

        public int FourRunsExtra => Extras == 4 ? 1 : 0;

        public int FiveRunsTotal => Total == 5 ? 1 : 0;

        public int FiveRunsBatsman => Batter == 5 ? 1 : 0;

        public int FiveRunsExtra => Extras == 5 ? 1 : 0;

        public int SixRunsTotal => Total == 6 ? 1 : 0;

        public int SixRunsBatsman => Batter == 6 ? 1 : 0;
        public int SixBoundaryRunsBatsman => Batter == 6 && IsNonBoundary == 0 ? 1 : 0;

        public int SixRunsExtra => Extras == 6 ? 1 : 0;
        public int SevenRunsTotal => Total == 7 ? 1 : 0;
        public int SevenRunsExtras => Extras == 7 ? 1 : 0;
    }
}