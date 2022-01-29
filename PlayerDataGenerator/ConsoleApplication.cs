namespace PlayerDataGenerator
{
    internal class ConsoleApplication
    {
        private readonly ICricketDataIngestor _cricketDataIngestor;

        public ConsoleApplication(ICricketDataIngestor cricketDataIngestor)
        {
            _cricketDataIngestor = cricketDataIngestor;
        }

        public void Run()
        {
            _cricketDataIngestor.ExtractPlayers();
           // _cricketDataIngestor.IngestMatchData();
        }
    }
}