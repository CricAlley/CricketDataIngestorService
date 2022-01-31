namespace ElasticRepository.Entities
{
    public class Fielder
    {
        public string Name { get; set; }
        public Player FielderPlayer { get; set; }
        public bool IsSubstitute { get; set; }
    }
}