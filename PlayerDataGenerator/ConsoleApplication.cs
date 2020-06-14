namespace PlayerDataGenerator
{
    internal class ConsoleApplication
    {
        private readonly IPlayerExtractor _playerExtractor;

        public ConsoleApplication(IPlayerExtractor playerExtractor)
        {
            _playerExtractor = playerExtractor;
        }

        public void Run()
        {
            _playerExtractor.Start();
        }
    }
}