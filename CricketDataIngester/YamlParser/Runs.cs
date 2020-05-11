using YamlDotNet.Serialization;

namespace CricketDataIngester.YamlParser
{
    public class Runs
    {
        public int Extras { get; set; }
        public int Total { get; set; }
        public int Batsman { get; set; }

        [YamlMember(Alias = "non_boundary", ApplyNamingConventions = false)]
        public int IsNonBoundary { get; set; }

        [YamlIgnore] public int DotsTotal => Total == 0 ? 1 : 0;

        [YamlIgnore] public int DotBatsman => Batsman == 0 ? 1 : 0;

        [YamlIgnore] public int OneRunTotal => Total == 1 ? 1 : 0;

        [YamlIgnore] public int OneRunBatsman => Batsman == 1 ? 1 : 0;

        [YamlIgnore] public int OneRunExtra => Extras == 1 ? 1 : 0;

        [YamlIgnore] public int TwoRunsTotal => Total == 2 ? 1 : 0;

        [YamlIgnore] public int TwoRunsBatsman => Batsman == 2 ? 1 : 0;

        [YamlIgnore] public int TwoRunsExtra => Extras == 2 ? 1 : 0;

        [YamlIgnore] public int ThreeRunsTotal => Total == 3 ? 1 : 0;

        [YamlIgnore] public int ThreeRunsBatsman => Batsman == 3 ? 1 : 0;

        [YamlIgnore] public int ThreeRunsExtra => Extras == 3 ? 1 : 0;

        [YamlIgnore] public int FourRunsTotal => Total == 4 ? 1 : 0;

        [YamlIgnore] public int FourRunsBatsman => Batsman == 4 ? 1 : 0;
        [YamlIgnore] public int FourBoundaryRunsBatsman => Batsman == 4  && IsNonBoundary == 0 ? 1 : 0;

        [YamlIgnore] public int FourRunsExtra => Extras == 4 ? 1 : 0;

        [YamlIgnore] public int FiveRunsTotal => Total == 5 ? 1 : 0;

        [YamlIgnore] public int FiveRunsBatsman => Batsman == 5 ? 1 : 0;

        [YamlIgnore] public int FiveRunsExtra => Extras == 5 ? 1 : 0;

        [YamlIgnore] public int SixRunsTotal => Total == 6 ? 1 : 0;

        [YamlIgnore] public int SixRunsBatsman => Batsman == 6 ? 1 : 0;
        [YamlIgnore] public int SixBoundaryRunsBatsman => Batsman == 6 && IsNonBoundary == 0 ? 1 : 0;

        [YamlIgnore] public int SixRunsExtra => Extras == 6 ? 1 : 0;
        [YamlIgnore] public int SevenRunsTotal => Total == 7 ? 1 : 0;
        [YamlIgnore] public int SevenRunsExtras => Extras == 7 ? 1 : 0;
    }
}